using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperIntelligence.Data
{
    public class Constants
    {
        public static int NetworkInputs = 8;
        public static int NetworkOutputs = 2;
        public enum SelectionAlgorithms : int { AllWithBest, NWithBest, Luhn }
    }
}
