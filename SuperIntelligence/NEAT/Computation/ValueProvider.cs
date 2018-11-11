using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperIntelligence.NEAT.Computation
{
    public class ConstantProvider<T> : IValueProvider<T>
    {
        public T Constant;

        public ConstantProvider()
        {
        }

        public ConstantProvider(T constant)
        {
            Constant = constant;
        }

        public T GetValue() =>
            Constant;
    }

    public interface IValueProvider<T>
    {
        T GetValue();
    }
}
