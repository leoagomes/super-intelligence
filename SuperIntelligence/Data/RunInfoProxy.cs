using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperIntelligence.NEAT;

namespace SuperIntelligence.Data
{
    public class RunInfoProxy
    {
        public Runner CurrentRunner;
        public List<Generation> Generations;

        public Generation NextGeneration;
        public Generation FinishedGeneration;

        public RunInfoProxy(Runner runner)
        {
            CurrentRunner = runner;
            Generations = new List<Generation>();
        }

    }
}
