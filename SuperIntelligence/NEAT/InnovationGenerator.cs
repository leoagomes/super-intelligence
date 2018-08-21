using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperIntelligence.NEAT
{
    class InnovationGenerator
    {
        public int Innovation { get; private set; }

        public int Innovate() =>
            Innovation++;
    }
}
