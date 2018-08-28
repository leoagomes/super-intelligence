using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Drawing;
using System.Windows.Forms;

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
        public static int GameCount = 9;

        private static AutoResetEvent managerAvailableEvent = new AutoResetEvent(false);
        private static AutoResetEvent individualTestedEvent = new AutoResetEvent(false);


        public Program()
        {
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new GenomeViewForm());
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

    }
}
