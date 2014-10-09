using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    [Serializable]
    /// <summary>
    /// homogenous meaning that N is the same for all NGrams
    /// </summary>
    /// <typeparam name="T">Type that can be equated with same type</typeparam>
    public sealed class HomogenousNGrams<T>
        : INGrams<T>
        where T : IEquatable<T>
    {
        public readonly int N;

        private readonly IEnumerable<NGram<T>> nGrams;

        private HomogenousNGrams(IEnumerable<NGram<T>> grams, int n)
        {
            grams.NullCheck();

            this.nGrams = grams;
            this.N = n;
        }

        public static HomogenousNGrams<T> BuildNGrams(int n, List<T> entities)
        {
            (n > 0).AssertTrue();
            (entities.Count >= n).AssertTrue();

            entities.NullCheck();
            List<NGram<T>> list = new List<NGram<T>>();
            int count = entities.Count - n;
            for (int i = 0; i < count; i++)
            {
                T[] ngram = new T[n];
                entities.CopyTo(i, ngram, 0, n);

                list.Add(new NGram<T>(ngram));
            }

            list.Add(new NGram<T>(entities.Skip(count).ToArray()));

            return new HomogenousNGrams<T>(list, n);

        }
        public static HomogenousNGrams<T> BuildFromSingleNGrams(NGram<T> a)
        {
            return new HomogenousNGrams<T>(a.AsEnumerableObject(), a.N);
        }

        /// <summary>
        /// Not recommended to use
        /// </summary>
        /// <param name="past"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static HomogenousNGrams<T> DirectBuiltUnsafe(IEnumerable<NGram<T>> past, int n)
        {
            past.NullCheck();

            return new HomogenousNGrams<T>(past, n);
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.nGrams.GetEnumerator();
        }

        IEnumerator<NGram<T>> IEnumerable<NGram<T>>.GetEnumerator()
        {
            return this.nGrams.GetEnumerator();
        }
    }

    [Serializable]
    /// <summary>
    /// Hetero meaning different n's
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class HeterogenousNGrams<T>
        : IHeterogenousNGrams<T>
        where T : IEquatable<T>
    {
        public int MinN { get; private set; }
        public int MaxN { get; private set; }

        private readonly IReadOnlyList<HomogenousNGrams<T>> homogenousConstruct;
        [NonSerialized]
        private readonly IEnumerable<NGram<T>> enumerationImplementation;

        private HeterogenousNGrams(List<HomogenousNGrams<T>> homogenousNGrams, int minn, int maxn)
        {
            this.homogenousConstruct = homogenousNGrams;

            this.MinN = minn;
            this.MaxN = maxn;

            this.enumerationImplementation = this.EnumerationFunction();
        }

        public IReadOnlyList<HomogenousNGrams<T>> SubHomogenousNGramConstructs()
        {
            return this.homogenousConstruct;
        }
        private IEnumerable<NGram<T>> EnumerationFunction()
        {
            foreach (var construct in homogenousConstruct)
            {
                foreach (var gram in construct)
                {
                    yield return gram;
                }
            }
        }


        public IEnumerator<NGram<T>> GetEnumerator()
        {
            return this.enumerationImplementation.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        public static HeterogenousNGrams<T> BuildNGrams(int p1, int p2, List<T> corp)
        {
            corp.NullCheck();

            (p1 > 0).AssertTrue();
            (p2 > p1).AssertTrue();

            return new HeterogenousNGrams<T>(Enumerable.Range(p1, p2 - p1).Select(x => HomogenousNGrams<T>.BuildNGrams(x, corp)).ToList(), p1, p2);
        }

        public static HeterogenousNGrams<T> BuildSingle(NGram<T> nGram)
        {
            return new HeterogenousNGrams<T>(HomogenousNGrams<T>.BuildFromSingleNGrams(nGram).AsEnumerableObject().ToList(), nGram.N, nGram.N + 1);
        }

    }

    [Serializable]
    public struct NGram<T>
        : IEquatable<NGram<T>>, IEnumerable<T>
        where T : IEquatable<T>
    {
        public static NGram<T> Empty = new NGram<T>(new T[1]);

        private readonly T[] Grams;
        private readonly int hashCode;

        public T this[int index]
        {
            get { return this.Grams[index]; }
        }
        public int N
        {
            get
            {
                if (this.Grams == null)
                {
                    return 0;
                }
                return this.Grams.Length;
            }
        }

        public NGram(T[] grams)
        {
            grams.NullCheck();

            this.Grams = grams;
            this.hashCode = HashHelper.FNVHashCode(this.Grams);
        }

        public static bool operator ==(NGram<T> first, NGram<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(NGram<T> first, NGram<T> second)
        {
            return !first.Equals(second);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NGram<T>))
            {
                return false;
            }
            return this.Equals((NGram<T>)obj);
        }

        public override int GetHashCode()
        {
            return this.hashCode;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in this.AsEnumerable())
            {
                builder.Append(item.ToString());
                builder.Append(" ");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public bool Equals(NGram<T> other)
        {
            if (this.hashCode != other.hashCode)
            {
                return false;
            }
            if (other.N != this.N)
            {
                return false;
            }

            for (int i = 0; i < other.N; i++)
            {
                if (null == other.Grams[i])
                {
                    if (this.Grams[i] == null)
                    {
                        continue;
                    }
                    return false;
                }
                if (!other.Grams[i].Equals(this.Grams[i]))
                {
                    return false;
                }
            }

            return true;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Grams.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (this.Grams == null)
            {
                return new T[0].AsEnumerable().GetEnumerator();
            }
            return this.Grams.AsEnumerable().GetEnumerator();
        }
    }

    public interface INGrams<T> : IEnumerable<NGram<T>>
        where T : IEquatable<T> { }
    public interface IHeterogenousNGrams<T> : INGrams<T>
        where T : IEquatable<T>
    {
        int MinN { get; }
        int MaxN { get; }
        IReadOnlyList<HomogenousNGrams<T>> SubHomogenousNGramConstructs();
    }
}
