using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.IO;

using Newtonsoft.Json;

using NLua;

using SuperIntelligence.Game;
using SuperIntelligence.NEAT;

using static SuperIntelligence.Random.Random;


namespace SuperIntelligence
{
    class Runner
    {
        public static slf4net.ILogger logger = slf4net.LoggerFactory.GetLogger(typeof(Runner));

        public InnovationGenerator InnovationGenerator;
        public InnovationGenerator GenomeInnovationGenerator;
        public string GameExecutablePath;

        public bool ShouldStop = false;

        public List<GameManager> GameManagers = new List<GameManager>();
        public ConcurrentQueue<GameManager> AvailableGameManagers = new ConcurrentQueue<GameManager>();
        public ConcurrentQueue<Individual> TestedIndividuals = new ConcurrentQueue<Individual>();
        public ConcurrentQueue<Individual> UntestedIndividuals = new ConcurrentQueue<Individual>();
        public int GameCount;

        private AutoResetEvent managerAvailableEvent = new AutoResetEvent(false);
        private AutoResetEvent individualTestedEvent = new AutoResetEvent(false);

        public delegate void IndividualDelegate(Individual individual);
        public delegate void GenerationDelegate(Generation generation);
        public delegate void LoopFinishDelegate();

        public event IndividualDelegate OnUntestedIndividual = delegate { };
        public event IndividualDelegate OnIndividualTested = delegate { };
        public event GenerationDelegate OnNextGeneration = delegate { };
        public event GenerationDelegate OnGenerationFinished = delegate { };
        public event LoopFinishDelegate OnLoopFinish = delegate { };

        public Lua LuaState;

        public Runner(string gamePath, int gameInstances, string luaScriptPath = "")
        {
            InnovationGenerator = new InnovationGenerator();
            GenomeInnovationGenerator = new InnovationGenerator();
            GameExecutablePath = gamePath;
            GameCount = gameInstances;

            LuaState = new Lua();
            LuaState.LoadCLRPackage();
            LuaState["runner"] = this;
            if (luaScriptPath != string.Empty && File.Exists(luaScriptPath))
                LuaState.DoFile(luaScriptPath);
        }

        #region Static Methods
        public Generation MakeFirstGeneration(InnovationGenerator generator, InnovationGenerator genomeGenerator,
            int initialPopulationSize)
        {
            // check if there is a Lua implementation of this
            LuaFunction func = LuaState["MakeFirstGeneration"] as LuaFunction;
            if (func != null)
            {
                try
                {
                    return (Generation)func.Call(generator, initialPopulationSize)?.First();
                }
                catch (Exception e)
                {
                    logger.Error(e, "Error running lua code for first generation.");
                }
            }

            Generation generation = new Generation(0);
            Genome genome = new Genome(0);

            int inputs = Data.Constants.NetworkInputs;
            int outputs = Data.Constants.NetworkOutputs;

            List<int> inputIds = new List<int>(inputs);

            // input nodes
            for (int i = 0; i < inputs; i++)
            {
                int currentId = generator.Innovate();
                inputIds.Add(currentId);
                genome.AddNode(new Node(currentId, NodeType.Input, int.MinValue));
            }

            // output nodes
            for (int i = 0; i < outputs; i++)
            {
                int currentId = generator.Innovate();
                genome.AddNode(new Node(currentId, NodeType.Output, int.MaxValue));

                foreach (int hiddenId in inputIds)
                    genome.AddConnection(new Connection(hiddenId, currentId, Double() * 4f - 2f, true, generator.Innovate()));
            }

            Species original = new Species(genome);
            generation.Species.Add(original);

            for (int i = 0; i < initialPopulationSize; i++)
            {
                Genome g = genome.Copy();
                g.Mutate(generator);
                g.Id = genomeGenerator.Innovate();

                original.AddGenome(g);
            }

            return generation;
        }
        #endregion

        public void StartGameInstances(string gamePath, string gameWd, int count)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(gamePath);
            startInfo.WorkingDirectory = gameWd;

            for (int i = 0; i < count; i++)
            {
                Process process = Process.Start(startInfo);
                Thread.Sleep(1000);

                GameInstance instance = new GameInstance(process);
                GameManager manager = new GameManager(instance, GameStates.MainMenu, GameModes.Hexagon);
                AvailableGameManagers.Enqueue(manager);
                GameManagers.Add(manager);
            }
        }

        public void KillGameInstances()
        {
            foreach (GameManager manager in GameManagers)
                manager.Kill();
        }

        public void DoRun(GameModes mode, Generation firstGeneration)
        {
            string hexagonDirectory = Path.GetDirectoryName(GameExecutablePath);
            string hexagonFile = Path.GetFileName(GameExecutablePath);

            // start game instances
            logger.Trace("Starting {0} Super Hexagon instances... ", GameCount);
            string gamePath = Path.Combine(hexagonDirectory, hexagonFile);
            string gameWd = hexagonDirectory;
            StartGameInstances(gamePath, gameWd, GameCount);

            // create directory for run files
            logger.Trace("Creating run directory... ");
            DateTime now = DateTime.Now;
            string runDirectoryName = "run-" +
                now.Year + "-" + now.Month + "-" + now.Day + "-" +
                now.Hour + "-" + now.Minute + "-" + now.Second;
            string runDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), runDirectoryName);
            Directory.CreateDirectory(runDirectoryPath);

            // do the run
            InnovationGenerator generator = InnovationGenerator;
            InnovationGenerator genomeGenerator = GenomeInnovationGenerator;
            Generation generation = firstGeneration;
            while (!ShouldStop)
            {
                logger.Trace("Generation " + generation.Number + " starting (" + generation.Species.Sum(s => s.Members.Count) + ")...");

                // add the individuals to the list of untested individuals
                int generationCount = 0;
                foreach (Species s in generation.Species)
                {
                    foreach (Genome member in s.Members)
                    {
                        Individual individual = new Individual(member, generation.Number);
                        //individual.Prepare();
                        individual.Index = generationCount;

                        UntestedIndividuals.Enqueue(individual);
                        OnUntestedIndividual(individual);
                        generationCount++;
                    }
                }

                // test all individuals in generation
                while (!UntestedIndividuals.IsEmpty)
                {
                    // in case there are no available game managers, wait for a signal on
                    // managerAvailableEvent
                    if (AvailableGameManagers.IsEmpty)
                    {
                        managerAvailableEvent.WaitOne();
                        continue; // restart the loop (in order to check again if there are game managers available)
                    }

                    // there are still untested individuals in the generation and there
                    // is a manager available, so start a thread with a runner on this manager
                    GameManager manager;
                    AvailableGameManagers.TryDequeue(out manager);

                    if (manager == null)
                        continue; // not sure what happened, try again

                    Individual individual;
                    UntestedIndividuals.TryDequeue(out individual);

                    if (individual == null)
                    {
                        // put the manager back in queue and try the loop again
                        AvailableGameManagers.Enqueue(manager);
                        continue;
                    }

                    AIRunner runner = new AIRunner(manager, individual, mode);
                    ThreadStart start = new ThreadStart(() =>
                    {
                        runner.DoSafeRun(); // do the game run

                        // add the individual to the list of tested individuals
                        TestedIndividuals.Enqueue(runner.Individual);
                        OnIndividualTested(runner.Individual);
                        individualTestedEvent.Set(); // signal that an individual was tested

                        AvailableGameManagers.Enqueue(manager); // then add the manager back to the queue
                        managerAvailableEvent.Set(); // then signal that managers are available
                    });
                    Thread thread = new Thread(start);
                    thread.Start(); // start the thread
                }

                // now we know all individuals are either testing or tested
                // let's wait until all individuals have been tested
                while (TestedIndividuals.Count != generationCount)
                    individualTestedEvent.WaitOne();

                string genFileName = "gen-" + generation.Number + ".json";
                var genFile = File.Create(Path.Combine(runDirectoryPath, genFileName));
                var buf = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(generation));
                genFile.Write(buf, 0, buf.Length);
                genFile.Close();
                logger.Trace("Generation file saved: " + genFileName);

                // make next generation
                OnGenerationFinished(generation);

                LuaFunction func = LuaState["MakeNextGeneration"] as LuaFunction;
                bool error = false;
                if (func != null)
                {
                    try
                    {
                        generation = (Generation)func.Call(generation, generator, genomeGenerator).First();
                    }
                    catch (Exception e)
                    {
                        error = true;
                        logger.Error(e, "Error running lua code for next generation.");
                    }
                }

                if (func == null || error)
                    generation = generation.Next(generator, genomeGenerator);

                OnNextGeneration(generation);

                // ajustes da mutação

                // clean up this generation's data
                TestedIndividuals = new ConcurrentQueue<Individual>();
            }

            OnLoopFinish();
            // terminate all game processes
            KillGameInstances();
        }
    }
}
