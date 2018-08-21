using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

using Newtonsoft.Json;

using SuperIntelligence.Game;
using SuperIntelligence.NEAT;

using static SuperIntelligence.Random.Random;

namespace SuperIntelligence
{
    class Program
    {
        public static List<GameManager> GameManagers = new List<GameManager>();
        public static ConcurrentQueue<GameManager> AvailableGameManagers = new ConcurrentQueue<GameManager>();
        public static ConcurrentQueue<Individual> TestedIndividuals = new ConcurrentQueue<Individual>();
        public static ConcurrentQueue<Individual> UntestedIndividuals = new ConcurrentQueue<Individual>();
        public static int GameCount = 5;

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

        public static Generation MakeFirstGeneration(InnovationGenerator generator, int initialPopulationSize)
        {
            Generation generation = new Generation(0);
            Genome genome = new Genome(0);

            int inputs = 13;
            int outputs = 2;

            // input nodes
            for (int i = 0; i < inputs; i++)
            {
                genome.AddNode(new Node(i, NodeType.Input, int.MinValue));
            }

            // output nodes
            for (int i = 0; i < outputs; i++)
            {
                genome.AddNode(new Node(i + inputs, NodeType.Output, int.MaxValue));

                for (int j = 0; j < inputs; j++)
                {
                    genome.AddConnection(new Connection(j, i + inputs, Double() * 4f - 2f, true, generator.Innovate()));
                }
            }
            genome.NextNodeId = inputs + outputs;

            Species original = new Species(genome);
            generation.Species.Add(original);

            for (int i = 0; i < initialPopulationSize; i++)
            {
                Genome g = genome.Copy();
                g.Mutate(generator);

                original.AddGenome(g);
            }

            return generation;
        }

        static void Main(string[] args)
        {
            string hexagonDirectory = "C:\\Users\\Leonardo\\Desktop\\super_hexagon-windows";
            string hexagonFile = "superhexagon.exe";
            GameModes mode = GameModes.Hexagonest;
            int initialSize = 30;

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


            InnovationGenerator generator = new InnovationGenerator();
            Generation generation = MakeFirstGeneration(generator, initialSize);
            while (true)
            {
                Console.WriteLine("Generation " + generation.Number + " starting (" + generation.Species.Sum(s => s.Members.Count) + ")...");

                // add the individuals to the list of untested individuals
                int generationCount = 0;
                foreach (Species s in generation.Species)
                {
                    foreach (Genome member in s.Members)
                    {
                        Individual individual = new Individual(member, generation.Number);
                        individual.Prepare();
                        individual.Index = generationCount;

                        UntestedIndividuals.Enqueue(individual);
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
                while (TestedIndividuals.Count != generationCount)
                    individualTestedEvent.WaitOne();

                List<Genome> bestInGeneration = generation.Species
                    .Select(s => s.Members.OrderByDescending(m => m.Fitness).First())
                    .ToList();

                // print them
                Console.WriteLine("Generation " + generation.Number);
                foreach (Species s in generation.Species)
                {
                    Genome genome = s.Members.First();
                    Console.WriteLine("\tFitness: " + genome.Fitness);
                }

                string genFileName = "gen-" + generation.Number + ".json";
                var genFile = File.Create(Path.Combine(runDirectoryPath, genFileName));
                var buf = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(generation));
                genFile.Write(buf, 0, buf.Length);
                genFile.Close();
                Console.WriteLine("Generation file saved: " + genFileName);

                // make next generation
                generation = generation.Next(generator);

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
