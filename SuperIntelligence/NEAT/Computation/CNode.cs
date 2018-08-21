using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperIntelligence.NEAT.Computation
{
    class CNode : IValueProvider<double>
    {
        private bool recalculate;
        private double value;

        public int Id { get; private set; }
        public List<IValueProvider<double>> Dependencies;
        public List<double> Weights;

        private Func<double, double> _activation;
        public Func<double, double> ActivationFunction
        {
            get => _activation;
            set
            {
                _activation = value;
                recalculate = true;
            }
        }

        public CNode(int id, Func<double, double> activation)
        {
            value = 0;
            recalculate = true;

            Id = id;
            ActivationFunction = activation;

            Dependencies = new List<IValueProvider<double>>();
            Weights = new List<double>();
        }

        public double GetValue()
        {
            if (!recalculate)
                return value;

            if (Dependencies.Count != Weights.Count)
                throw new Exception("CNode " + Id + " has different dependency and weight counts.");

            double val = 0.0;
            for (int i = 0; i < Dependencies.Count; i++)
                val += Dependencies[i].GetValue() * Weights[i];

            value = ActivationFunction(val);
            recalculate = false;

            return val;
        }

        public void MarkRecalculate() =>
            recalculate = true;

        public void AddPair(IValueProvider<double> dependency, double weight)
        {
            Dependencies.Add(dependency);
            Weights.Add(weight);
        }
    }
}
