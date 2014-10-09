using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    public static class BasicExtensions
    {
        private static Random random = new Random();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("DEBUG")]
        public static void NullCheck<T>(this T t) where T : class
        {
            if (null == t)
            {
                throw new ArgumentNullException();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertTrue(this bool a)
        {
            if (!a)
            {
                throw new ArgumentException();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForceDefferedExecution<T>(this IEnumerable<T> collection)
        {
            collection.Count();
        }
        public static IEnumerable<T> AsEnumerableObject<T>(this T @object)
        {
            yield return @object;
        }
        public static Dictionary<Y, List<T>> ReverseDictionary<T, Y>(this Dictionary<T, Y> dictionary)
        {
            Dictionary<Y, List<T>> newDict = new Dictionary<Y, List<T>>();
            foreach (var item in dictionary)
            {
                if (newDict.ContainsKey(item.Value))
                {

                    newDict[item.Value].Add(item.Key);
                }
                else
                {
                    newDict.Add(item.Value, new List<T>() { item.Key });
                }
            }
            return newDict;
        }
        public static int Count<T>(this IEnumerable<T> t, Func<T, int> func)
        {
            t.NullCheck();
            func.NullCheck();

            int count = 0;
            foreach (var item in t)
            {
                count += func(item);
            }

            return count;
        }
        public static T PickRandomFromProbabilityDistrubutionsSafe<T>(this Dictionary<T, float> a)
        {
            a.NullCheck();
            if (!a.Any())
            {
                return default(T);
            }
            var rand = random.NextDouble();
            double err = 0;
            foreach (var item in a)
            {
                err += item.Value;
                if (rand < err)
                {
                    return item.Key;
                }
            }
            return a.PickRandom().Key;
        }
        public static T PickRandom<T>(this IEnumerable<T> t)
        {
            int count = t.Count();
            return t.Skip(random.Next(0, count)).First();
        }
        public static Dictionary<T, float> Normalize<T>(this Dictionary<T, float> key)
        {
            float sum = key.Sum(x => x.Value);
            return key.ToDictionary(item => item.Key, item => item.Value / sum);
        }
        public static IEnumerable<double> Normalize(this IEnumerable<double> l)
        {
            l.NullCheck();
            double max = l.Max();
            foreach (var item in l)
            {
                yield return item / max;
            }
        }
        public static IEnumerable<double[]> Normalize(this IEnumerable<IEnumerable<double>> l)
        {
            double max = l.Max(x => x.Max());
            foreach (var item in l)
            {
                yield return item.Select(x => x / max).ToArray();
            }
        }
        public static IEnumerable<T> RandomValues<T>(this IEnumerable<T> enu, int count)
        {
            enu.NullCheck();
            List<T> list = enu.ToList();
            int completeCount = list.Count();
            List<T> set = new List<T>();
            int count1 = 0;
            while (set.Count < count && set.Count < completeCount && count1 < count * 3)
            {
                T t = list[random.Next(0, completeCount)];
                set.Add(t);
                count1++;
            }
            return set;
        }
        public static T MaxValue<T>(this IEnumerable<T> a, Func<T, double> func)
        {
            a.NullCheck();

            double min = double.MinValue;
            T value = default(T);

            foreach (var item in a)
            {
                var cur = func(item);
                if (cur > min)
                {
                    min = cur;
                    value = item;
                }
            }
            return value;
        }
        public static T MinValue<T>(this IEnumerable<T> a, Func<T, double> func)
        {
            a.NullCheck();

            double min = double.MaxValue;
            T value = default(T);

            foreach (var item in a)
            {
                var cur = func(item);
                if (cur < min)
                {
                    min = cur;
                    value = item;
                }
            }
            return value;
        }
        public static void Shuffle<T>(this T[] list)
        {
            Random rng = new Random();
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        public static bool DeepEquals<T>(this T[] left, T[] right)
            where T : IEquatable<T>
        {
            if (null == right)
            {
                return false;
            }
            if (left.Length != right.Length)
            {
                return false;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (!left[i].Equals(right[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}





