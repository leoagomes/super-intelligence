using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperIntelligence.NEAT;

using Newtonsoft.Json;

namespace SuperIntelligence
{
    class Individual
    {
        public Genome Genome;
        [JsonIgnore]
        public Network Net;

        public int Index
        {
            get => Genome.Id;
            set => Genome.Id = value;
        }
        public int Generation;

        public double Fitness
        {
            get => Genome.Fitness;
            set => Genome.Fitness = value;
        }

        public Individual(Genome genome,  int generation)
        {
            Genome = genome;
            Generation = generation;
        }

        public void Prepare() =>
            Net = new Network(Genome);

        public List<double> Forward(List<double> inputs) =>
            Net.Forward(inputs);

        public static Individual GenerateChild(Individual a, Individual b)
        {
            int generation = Math.Max(a.Generation, b.Generation) + 1;

            Genome fit = (a.Fitness > b.Fitness) ? a.Genome : b.Genome;
            Genome other = (fit == a.Genome) ? b.Genome : a.Genome;

            Individual child = new Individual(Genome.CrossOver(fit, other), generation);
            return child;
        }
    }
}
