using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperIntelligence.NN;

namespace SuperIntelligence
{
    public class Individual
    {
        public Network Net;
        public int Fitness;
        public int Index;
        public int Generation;

        public Individual(Network net,  int generation)
        {
            Net = net;
            Generation = generation;
        }

        public static Individual GenerateChild(Individual a, Individual b)
        {
            int generation = Math.Max(a.Generation, b.Generation) + 1;
            Individual child = new Individual(Network.GenerateChild(a.Net, b.Net), generation);
            return child;
        }
    }
}
