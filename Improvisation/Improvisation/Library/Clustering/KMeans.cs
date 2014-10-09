using Improvisation.Library.Distance;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Improvisation.Library.Clustering
{
    public sealed class KMeans<T>
        where T : IEquatable<T>
    {
        private static Random random = new Random();
        private object lockObject = new object();

        public IReadOnlyCollection<T[]> Centers { get { return this.centers.Select(x => x.Key).ToList().AsReadOnly(); } }

        public int K { get; private set; }
        public IDistance<T> Function { get; private set; }
        private T[][] values;
        private Dictionary<T[], List<T[]>> centers;
        public KMeans(IEnumerable<T[]> vectors, IDistance<T> func, int k, int[] centers)
        {
            vectors.NullCheck();
            centers.NullCheck();
            func.NullCheck();

            (centers.Length == k).AssertTrue();

            this.values = vectors.ToArray();
            this.Function = func;
            this.K = k;
            this.centers = new Dictionary<T[], List<T[]>>();
            foreach (var item in centers)
            {
                if (!this.centers.ContainsKey(this.values[item]))
                {
                    this.centers.Add(this.values[item], new List<T[]>());
                }
            }
            this.AssignToNewCenters();
        }

        public KMeans(IEnumerable<T[]> vectors, IDistance<T> func, int k)
            : this(vectors, func, k,
                  (from iten in Enumerable.Range(0, k) select random.Next(0, vectors.Count())).ToArray()) { }

        public void RunSingleFrame()
        {
            this.ShiftToNewCenters();
            this.AssignToNewCenters();
        }
        public void RunFrames(int k)
        {
            for (int i = 0; i < k && this.centers.Count > 1; i++)
            {
                this.RunSingleFrame();
            }
        }

        public float Classify(T[] vector)
        {
            var k = (from item in this.centers.AsParallel()
                     orderby this.Function.Distance(vector, item.Key) ascending
                     select item.Key).First();

            int index = 0;
            foreach (var item in this.centers)
            {
                if (item.Key == k)
                {
                    return index;
                }
                else
                {
                    index++;
                }
            }
            throw new InvalidProgramException();
        }

        private void ShiftToNewCenters()
        {
            List<T[]> newCentros = new List<T[]>(this.K);

            foreach (var item in this.centers)
            {
                newCentros.Add(this.FindCenter(item.Value));
            }
            this.centers = new Dictionary<T[], List<T[]>>();
            foreach (var item in newCentros)
            {
                if (null != item)
                {
                    this.centers.Add(item, new List<T[]>());
                }
            }
        }
        private void AssignToNewCenters()
        {
            foreach (var item in this.centers)
            {
                item.Value.Clear();
            }

            int count = 0;
            Parallel.ForEach(values, (item) =>
            {
                var fastMin = this.centers.Keys.MinValue(x => this.Function.Distance(x, item));

                /*
                var min = (from x in this.centers.Keys.AsParallel()
                           orderby this.Function.Distance(x, item) ascending
                           select x).First();
                */
                Interlocked.Increment(ref count);
                lock (this.lockObject)
                {
                    if (count > 0)
                    {
                        this.centers[fastMin].Add(item);
                    }
                }
            });
        }

        private T[] FindCenter(List<T[]> vec)
        {
            T[] v = default(T[]);
            double min = float.MaxValue;

            Parallel.For(0, vec.Count, y =>
            {
                double currentMin = (float)vec.Sum((x) => this.Function.Distance(vec[y], x));
                if (currentMin < min)
                {
                    lock (this)
                    {
                        min = currentMin;
                        v = vec[y];
                    }
                }
            });
            return v;
        }
    }
}
