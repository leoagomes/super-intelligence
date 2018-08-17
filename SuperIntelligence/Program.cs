using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

using Newtonsoft.Json;

using SuperIntelligence.Game;
using SuperIntelligence.NN;

namespace SuperIntelligence
{
    class Program
    {
        public static List<GameManager> GameManagers = new List<GameManager>();
        public static ConcurrentQueue<GameManager> AvailableGameManagers = new ConcurrentQueue<GameManager>();
        public static ConcurrentQueue<Individual> TestedIndividuals = new ConcurrentQueue<Individual>();
        public static ConcurrentQueue<Individual> UntestedIndividuals = new ConcurrentQueue<Individual>();
        public static int GameCount = 9;

        private static AutoResetEvent managerAvailableEvent = new AutoResetEvent(false);
        private static AutoResetEvent individualTestedEvent = new AutoResetEvent(false);

        public static void StartGameInstances(string gamePath, string gameWd, int count)
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

        public static void KillGameInstances()
        {
            foreach (GameManager manager in GameManagers)
                manager.Kill();
        }

        public static List<Individual> NewGenerationWithElitism(List<Individual> elite, int generationId)
        {
            // TODO: I could probably do this with a couple of linq expressions
            List<Individual> nextGen = new List<Individual>(elite.Count * elite.Count);

            int generationIndex = 0;
            foreach (Individual individual in elite)
            {
                nextGen.Add(individual);

                foreach (Individual other in elite)
                {
                    if (individual == other)
                        continue;

                    Individual child = Individual.GenerateChild(individual, other);
                    child.Generation = generationId;
                    child.Index = generationIndex++;

                    nextGen.Add(child);
                }
            }

            return nextGen;
        }

        public static List<Individual> CreateFirstGeneration()
        {
            Network mNet = new Network(2);
            mNet.Layers.Add((new Layer(6)).InitializePerceptrons(6, 12));
            mNet.Layers.Add((new Layer(2)).InitializePerceptrons(2, 6));
            mNet.Normalize(AIRunner.NetworkInputs, AIRunner.NetworkOutputs);
            Individual michelangelo = new Individual(mNet, 0);
            michelangelo.Index = 0;

            Network dNet = new Network(2);
            dNet.Layers.Add((new Layer(7)).InitializePerceptrons(7, 12));
            dNet.Layers.Add((new Layer(2)).InitializePerceptrons(2, 7));
            dNet.Normalize(AIRunner.NetworkInputs, AIRunner.NetworkOutputs);
            Individual donatello = new Individual(dNet, 0);
            michelangelo.Index = 1;

            Network rNet = new Network(2);
            rNet.Layers.Add((new Layer(8)).InitializePerceptrons(8, 12));
            rNet.Layers.Add((new Layer(2)).InitializePerceptrons(2, 8));
            rNet.Normalize(AIRunner.NetworkInputs, AIRunner.NetworkOutputs);
            Individual raphael = new Individual(rNet, 0);
            michelangelo.Index = 2;

            Network lNet = new Network(2);
            lNet.Layers.Add((new Layer(9)).InitializePerceptrons(9, 12));
            lNet.Layers.Add((new Layer(2)).InitializePerceptrons(2, 9));
            lNet.Normalize(AIRunner.NetworkInputs, AIRunner.NetworkOutputs);
            Individual leonardo = new Individual(lNet, 0);
            michelangelo.Index = 3;

            Network cNet = new Network(1);
            cNet.Layers.Add((new Layer(2)).InitializePerceptrons(2, 12));
            cNet.Normalize(AIRunner.NetworkInputs, AIRunner.NetworkOutputs);
            Individual caravaggio = new Individual(cNet, 0);
            caravaggio.Index = 4;

            Network bNet = new Network(1);
            bNet.Layers.Add((new Layer(2)).InitializePerceptrons(4, 12));
            bNet.Normalize(AIRunner.NetworkInputs, AIRunner.NetworkOutputs);
            Individual boticcelli = new Individual(bNet, 0);
            boticcelli.Index = 5;

            List<Individual> first = new List<Individual>(4)
            {
                michelangelo,
                donatello,
                raphael,
                leonardo,
                caravaggio,
                boticcelli,
            };

            return NewGenerationWithElitism(first, 1);
        }

        static void Main(string[] args)
        {
            string hexagonDirectory = "C:\\Users\\Leonardo\\Desktop\\super_hexagon-windows";
            string hexagonFile = "superhexagon.exe";

            // start game instances
            Console.Write("Starting {0} Super Hexagon instances... ", GameCount);
            string gamePath = Path.Combine(hexagonDirectory, hexagonFile);
            string gameWd = hexagonDirectory;
            StartGameInstances(gamePath, gameWd, GameCount);
            Console.WriteLine("OK!");

            // create directory for run files
            Console.Write("Creating run directory... ");
            DateTime now = DateTime.Now;
            string runDirectoryName = "run-" +
                now.Year + "-" + now.Month + "-" + now.Day + "-" +
                now.Hour + "-" + now.Minute + "-" + now.Second;
            string runDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), runDirectoryName);
            Directory.CreateDirectory(runDirectoryPath);
            Console.WriteLine("OK!");

            // create first generation
            Console.Write("Creating generation 0... ");
            List<Individual> firstGen = CreateFirstGeneration();
            Console.WriteLine("OK!");

            // do the test-rank-reproduce loop
            List<Individual> generation = firstGen;
            int generationId = 1;
            GameModes mode = GameModes.Hexagon;
            while (true) // TODO: think of a better condition
            {
                // add the generation to the untested queue
                generation.ForEach(i => UntestedIndividuals.Enqueue(i));

                // test all the individuals in the generation
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
                        runner.DoGameRun(); // do the game run

                        // add the individual to the list of tested individuals
                        TestedIndividuals.Enqueue(runner.Individual);
                        individualTestedEvent.Set(); // signal that an individual was tested

                        AvailableGameManagers.Enqueue(manager); // then add the manager back to the queue
                        managerAvailableEvent.Set(); // then signal that managers are available
                    });
                    Thread thread = new Thread(start);
                    thread.Start(); // start the thread
                }

                // now we know all individuals are either testing or tested
                // let's wait until all individuals have been tested
                while (TestedIndividuals.Count != generation.Count)
                {
                    individualTestedEvent.WaitOne();
                }

                // now we know all individuals have been tested, so we can decide on the
                // next generation

                // rank the next generation
                List<Individual> best = generation
                    .OrderByDescending(i => i.Fitness)
                    .Take(4)
                    .ToList();

                // print and save generation data
                foreach (Individual i in best)
                {
                    Console.WriteLine("Generation {0} Individual {1}", i.Generation, i.Index);
                    Console.WriteLine("\tFitness: {0}", i.Fitness);
                    Console.Write("\tTopology: ");

                    foreach (Layer l in i.Net.Layers)
                    {
                        Console.Write(l.Perceptrons.Count + " ");
                    }
                    Console.WriteLine("");
                }

                string genFileName = "gen-" + generationId + ".json";
                var genFile = File.Create(Path.Combine(runDirectoryPath, genFileName));
                var buf = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(best));
                genFile.Write(buf, 0, buf.Length);
                genFile.Close();
                Console.WriteLine("Generation file saved: " + genFileName);

                // generate next generation
                generation = NewGenerationWithElitism(best, ++generationId);
                // clean up this generation's data
                TestedIndividuals = new ConcurrentQueue<Individual>();
            }

            // terminate all game processes
            Console.WriteLine("Press enter to kill all game processes.");
            Console.ReadLine();
            KillGameInstances();
        }
    }
}
