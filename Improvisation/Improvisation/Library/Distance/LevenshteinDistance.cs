using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Distance
{
    public class LevenshteinDistance<T> : IDistance<T>
        where T : IEquatable<T>
    {
        public double Distance(T[] first, T[] second)
        {
            first.NullCheck();
            second.NullCheck();

            return Compute(first, second);
        }

        private static int Compute(T[] s, T[] t)
        {
            int n = s.Length;
            int m = t.Length;

            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    var jl = j - 1;
                    var il = i - 1;

                    int cost = (t[jl].Equals(s[il])) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[il, j] + 1, d[i, jl] + 1),
                        d[il, jl] + cost);
                }
            }

            return d[n, m];
        }
    }
}
