using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Distance
{
    public sealed class HammingDistance<T> : IDistance<T> where T : IEquatable<T>
    {
        public double Distance(T[] first, T[] second)
        {
            first.NullCheck();
            second.NullCheck();

            double distance = 0;
            int min = Math.Min(first.Length, second.Length);
            for (int i = 0; i < min; i++)
            {
                distance += (first[i].Equals(second[i])) ? 1 : 0;
            }
            return distance + Math.Max(first.Length, second.Length) - min;
        }
    }
}
