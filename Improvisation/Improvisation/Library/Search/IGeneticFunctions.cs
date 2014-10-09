using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Improvisation.Library.Search
{
    public interface IGeneticFunctions<T>
    {
        T Mutate(T t);
        T Cross(T first, T second);
        T Random();
        double Fitness(T t);
    }
}
