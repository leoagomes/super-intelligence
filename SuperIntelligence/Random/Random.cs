using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.Random;

namespace SuperIntelligence.Random
{
    public class Random
    {
        public static SystemRandomSource RandomSource = SystemRandomSource.Default;

        public static double GenerateWeight() =>
            RandomSource.NextDouble();

        public static double GenerateWeight(int inputs) =>
            (RandomSource.NextDouble() * inputs) * 0.01;

        public static double GenerateWeight(int inputs, int layerCount) =>
            (layerCount + ((layerCount - inputs) * (RandomSource.NextDouble()))) * Math.Sqrt(2.0/inputs);

        public static bool InChance(double chance) =>
            (RandomSource.NextDouble() <= chance);

        public static T Choose<T>(T a, T b) =>
            (RandomSource.NextBoolean() ? a : b);

        public static T Choose<T>(params T[] options) =>
            (options[(int)(RandomSource.NextDouble() * options.Length)]);

        public static double Double() =>
            RandomSource.NextDouble();

        public static double Range(double start, double end) =>
            start + (RandomSource.NextDouble() * (end - start));
    }
}
