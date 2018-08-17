using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathNet.Numerics.Random;
using MathNet.Numerics.Distributions;

namespace SuperIntelligence.NN
{
    public class WeightGenerator
    {
        public static double GenerateWeight() =>
            SystemRandomSource.Default.NextDouble();

        public static double GenerateWeight(int inputs) =>
            (SystemRandomSource.Default.NextDouble() * inputs) * 0.01;

        public static double GenerateWeight(int inputs, int layerCount) =>
            (layerCount + ((layerCount - inputs) * (SystemRandomSource.Default.NextDouble()))) * Math.Sqrt(2.0/inputs);
    }

    public class Perceptron
    {
        public static double CrossOverChance = 0.3;
        public static double MutationChance = 0.15;

        public double Bias;
        public List<double> Weights;

        public Perceptron()
        {
            Weights = new List<double>();
        }

        public Perceptron(int inputs)
        {
            Weights = new List<double>(inputs);
        }

        public Perceptron InitializeWeights(int inputs, int layerCount)
        {
            Weights.Clear();

            for (int i = 0; i < inputs; i++)
                Weights.Add(WeightGenerator.GenerateWeight(inputs, layerCount));
            Bias = WeightGenerator.GenerateWeight(inputs, layerCount);

            return this;
        }

        public double Net(List<double> x)
        {
            double value = 0;

            for (int i = 0; i < x.Count; i++)
                value += Weights[i] * x[i];

            value += Bias;

            return value;
        }

        public double Sigmoid(List<double> x) =>
            1 / (1 + Math.Exp(-Net(x)));

        public Perceptron Clone()
        {
            Perceptron perceptron = new Perceptron(Weights.Count);
            perceptron.Bias = Bias;

            foreach (double weight in Weights)
                perceptron.Weights.Add(weight);

            return perceptron;
        }

        public void NormalizeInputs(int inputs, int layerCount = 0)
        {
            if (inputs == Weights.Count)
                return;

            if (inputs > Weights.Count)
            {
                int diff = inputs - Weights.Count;
                for (int i = 0; i < diff; i++)
                    Weights.Add(layerCount > 0 ?
                        WeightGenerator.GenerateWeight(inputs, layerCount)
                        : WeightGenerator.GenerateWeight(inputs));
            }
            else if (inputs < Weights.Count)
            {
                int excess = Weights.Count - inputs;
                Weights.RemoveRange(Weights.Count - excess, excess);
            }
        }

        public static Perceptron GenerateChild(Perceptron a, Perceptron b)
        {
            // child generation step
            Perceptron child;

            var shouldCrossOver = SystemRandomSource.Default.NextDouble() <= CrossOverChance;
            if (shouldCrossOver)
            {
                int inputs = Math.Min(a.Weights.Count, b.Weights.Count);

                child = new Perceptron(inputs);
                int crossOverPoint = (int)(SystemRandomSource.Default.NextDouble() * inputs);

                Perceptron parent = SystemRandomSource.Default.NextBoolean() ? a : b;
                child.Bias = parent.Bias;
                for (int i = 0; i < inputs; i++)
                {
                    if (i == crossOverPoint)
                        parent = (parent == a) ? b : a;

                    child.Weights.Add(parent.Weights[i]);
                }
            }
            else
            {
                child = (SystemRandomSource.Default.NextBoolean() ? a : b).Clone();
            }

            // mutation step
            for (int i = 0; i < child.Weights.Count; i++)
            {
                if (SystemRandomSource.Default.NextDouble() <= MutationChance)
                    child.Weights[i] = -child.Weights[i]; // TODO: try other things
            }
            if (SystemRandomSource.Default.NextDouble() <= MutationChance)
                child.Bias = -child.Bias; // TODO: try other things

            return child;
        }
    }

    public class Layer
    {
        public static double MutationChance = 0.15;
        public List<Perceptron> Perceptrons;

        public Layer()
        {
            Perceptrons = new List<Perceptron>();
        }

        public Layer(int perceptrons)
        {
            Perceptrons = new List<Perceptron>(perceptrons);
        }

        public List<double> Sigmoid(List<double> input) =>
            Perceptrons.Select((p) => p.Sigmoid(input)).ToList();

        public Layer InitializePerceptrons(int perceptrons, int inputs)
        {
            for (int i = 0; i < perceptrons; i++)
            {
                Perceptrons.Add((new Perceptron(inputs)).InitializeWeights(inputs, perceptrons));
            }
            return this;
        }

        public Layer NormalizeInputs(int inputs)
        {
            foreach (Perceptron p in Perceptrons)
                p.NormalizeInputs(inputs, Perceptrons.Count);

            return this;
        }

        public void Resize(int perceptrons)
        {
            if (perceptrons == Perceptrons.Count)
                return;

            if (perceptrons > Perceptrons.Count)
            {
                int diff = perceptrons - Perceptrons.Count;
                for (int i = 0; i < diff; i++)
                    Perceptrons.Add(new Perceptron());
            }
            else if (perceptrons < Perceptrons.Count)
            {
                int diff = Perceptrons.Count - perceptrons;
                Perceptrons.RemoveRange(Perceptrons.Count - diff, diff);
            }
        }

        public Layer Clone()
        {
            Layer layer = new Layer(Perceptrons.Count);
            foreach (Perceptron perceptron in Perceptrons)
                layer.Perceptrons.Add(perceptron.Clone());
            return layer;
        }

        public static Layer GenerateChild(Layer a, Layer b)
        {
            // child creation step
            Layer alpha = SystemRandomSource.Default.NextBoolean() ? a : b;
            Layer beta = alpha == a ? b : a;

            Layer child = new Layer(alpha.Perceptrons.Count);

            // & as many perceptrons as possible
            int common = Math.Min(alpha.Perceptrons.Count, beta.Perceptrons.Count);
            for (int i = 0; i < common; i++)
            {
                child.Perceptrons.Add(Perceptron.GenerateChild(alpha.Perceptrons[i], beta.Perceptrons[i]));
            }

            // check if the child should have more perceptrons than it currently has
            if (child.Perceptrons.Count < alpha.Perceptrons.Count)
            {
                // in which case, fill the remaining spots with copies of alpha's perceptrons
                for (int i = common; i < alpha.Perceptrons.Count; i++)
                    child.Perceptrons.Add(alpha.Perceptrons[i].Clone());
            }

            // mutation
            // adds or removes a perceptron
            bool shouldMutate = SystemRandomSource.Default.NextDouble() <= MutationChance;
            if (shouldMutate)
            {
                if (SystemRandomSource.Default.NextBoolean())
                {
                    child.Perceptrons.Add(new Perceptron());
                }
                else if (child.Perceptrons.Count > 1)
                {
                    child.Perceptrons.RemoveAt((int)(SystemRandomSource.Default.NextDouble() * child.Perceptrons.Count));
                }
            }

            return child;
        }
    }

    public class Network
    {
        public static double MutationChance = 0.05;

        public List<Layer> Layers = new List<Layer>();

        public int InputCount { get; private set; }
        public int OutputCount { get; private set; }

        public Network()
        {
            Layers = new List<Layer>();
        }

        public Network(int layers)
        {
            Layers = new List<Layer>(layers);
        }

        public List<double> Sigmoid(List<double> input)
        {
            List<double> result = input;

            foreach (Layer layer in Layers)
                result = layer.Sigmoid(result);

            return result;
        }
        
        public Network Clone()
        {
            Network clone = new Network(Layers.Count);
            foreach (Layer layer in Layers)
                clone.Layers.Add(layer.Clone());
            return clone;
        }

        public Network Normalize(int inputs, int outputs)
        {
            if (Layers.Count <= 0)
                return this;

            // set variables
            InputCount = inputs;
            OutputCount = outputs;

            // resize last layer to fit output count
            Layers.Last().Resize(outputs);

            // normalize all layers in order
            int inputCount = inputs;
            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].NormalizeInputs(inputCount);
                inputCount = Layers[i].Perceptrons.Count;
            }

            return this;
        }

        public static Network GenerateChild(Network a, Network b)
        {
            // child generation step
            Network alpha = SystemRandomSource.Default.NextBoolean() ? a : b;
            Network beta = alpha == a ? b : a;

            Network child = new Network(alpha.Layers.Count);

            int common = Math.Min(alpha.Layers.Count, beta.Layers.Count);
            for (int i = 0; i < common; i++)
                child.Layers.Add(Layer.GenerateChild(alpha.Layers[i], beta.Layers[i]));

            if (child.Layers.Count != alpha.Layers.Count)
            {
                for (int i = common; i < alpha.Layers.Count; i++)
                {
                    child.Layers.Add(alpha.Layers[i].Clone());
                }
            }

            // mutation step
            bool shouldMutate = SystemRandomSource.Default.NextDouble() < MutationChance;
            if (shouldMutate)
            {
                // either removes or clones a random layer
                if (SystemRandomSource.Default.NextBoolean())
                {
                    // clones a random layer
                    int index = (int)(SystemRandomSource.Default.NextDouble() * child.Layers.Count);
                    Layer clone = child.Layers[index].Clone();
                    child.Layers.Insert(index, clone);
                }
                else
                {
                    // removes a random layer from the child (if it has more than one layer)
                    if (child.Layers.Count > 1)
                        child.Layers.RemoveAt((int)(SystemRandomSource.Default.NextDouble() * child.Layers.Count));
                }
            }
            return child;
        }

        public string TopologyString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (Layers.Count <= 0)
                return "";

            stringBuilder.Append(Layers[0].Perceptrons.Count);

            for (int i = 1; i < Layers.Count; i++)
            {
                stringBuilder.Append(" ");
                stringBuilder.Append(Layers[i].Perceptrons.Count);
            }

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return "Network[" + TopologyString() + "]";
        }
    }

}
