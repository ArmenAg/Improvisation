using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Distance
{
    public class EuclideanDistance<T> : IDistance<T>
    {
        public double Distance(T[] first, T[] second)
        {
            first.NullCheck();
            second.NullCheck();

            int min = Math.Min(first.Length, second.Length);
            double cost = 0;
            for (int i = 0; i < min; i++)
            {
                cost += Math.Pow((double)((dynamic)first[i] + (dynamic)second[i]), 2);
            }
            if (first.Length != min)
            {
                for (int i = min; i < first.Length; i++)
                {
                    cost += Math.Pow((double)((dynamic)first[i]), 2);
                }
                return cost;
            }
            for (int i = min; i < second.Length; i++)
            {
                cost += Math.Pow((double)((dynamic)second[i]), 2);
            }
            return cost;
        }
    }
}
