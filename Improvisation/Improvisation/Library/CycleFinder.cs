using AForge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    public static class CycleFinder
    {
        public static HashSet<CycleData<T>> BruteForceAlgorithm<T>(List<T> list)
            where T : IEquatable<T>
        {
            list.NullCheck();

            HashSet<CycleData<T>> data = new HashSet<CycleData<T>>();

            int count = list.Count / 2;
            for (int startIndex = 0; startIndex < count; startIndex++)
            {
                int maxCount = (list.Count - startIndex) / 2;

                for (int i = 2; i < maxCount; i++)
                {
                    data.Add(CycleFinder.BruteForceSingle<T>(list, startIndex, i));
                }
            }

            return data;
        }
        private static CycleData<T> BruteForceSingle<T>(List<T> list, int start, int count)
            where T : IEquatable<T>
        {
            var sub = list.GetRange(start, count).ToArray();
            int hash = HashHelper.FNVHashCode(sub);
            int amount = 1;

            for (int i = start + count; i < list.Count; i++)
            {
                var a = list.GetRange(i, (i + count >= list.Count) ? list.Count - i : count).ToArray();
                int curHash = HashHelper.FNVHashCode(a);
                if (hash == curHash)
                {
                    if (a.DeepEquals(sub))
                    {
                        amount++;
                    }
                }
            }
            return new CycleData<T>(list.GetRange(start, count).ToArray(), list.Count / count, amount);
        }
    }

    public struct CycleData<T>
    {
        public readonly T[] Cycle;

        public readonly int PossibleCycles;
        public readonly int NumberOfCycles;

        public readonly float Amplitude;

        public CycleData(T[] ran, int possible, int amount)
        {
            this.Cycle = ran;
            this.PossibleCycles = possible;
            this.NumberOfCycles = amount;

            this.Amplitude = (float)this.NumberOfCycles / (float)this.PossibleCycles;
        }
    }
}
