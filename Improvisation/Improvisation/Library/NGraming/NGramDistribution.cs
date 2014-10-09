using Improvisation.Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    [Serializable]
    public class NGramDistribution<T>
        : IEnumerable<KeyValuePair<NGram<T>, int>>
        where T : IEquatable<T>
    {
        public readonly int Size;

        private readonly Dictionary<NGram<T>, int> distrubution;

        public NGramDistribution(INGrams<T> ngrams)
            : this(ngrams.AsEnumerable()) { }

        public NGramDistribution(IEnumerable<NGram<T>> ngrams)
        {
            ngrams.NullCheck();

            this.distrubution = new Dictionary<NGram<T>, int>();
            int count = 0;
            foreach (NGram<T> item in ngrams)
            {
                if (this.distrubution.ContainsKey(item))
                {
                    this.distrubution[item]++;
                    continue;
                }
                this.distrubution.Add(item, 1);
                count++;
            }

            this.Size = count;
        }
        private NGramDistribution(Dictionary<NGram<T>, int> dist, int size)
        {
            this.distrubution = dist;
            this.Size = size;
        }
        public NGramDistribution<T> Filter(Predicate<KeyValuePair<NGram<T>, int>> predicate)
        {
            var filtered = this.Where(x => predicate(x));
            int newSize = filtered.Sum(x => x.Value);

            Dictionary<NGram<T>, int> dic = new Dictionary<NGram<T>, int>();
            foreach (var item in filtered)
            {
                dic.Add(item.Key, item.Value);
            }
            return new NGramDistribution<T>(dic, newSize);
        }
        
        public int RetrieveCount(NGram<T> t)
        {
            if (this.distrubution.ContainsKey(t))
            {
                return this.distrubution[t];
            }
            return 0;
        }
        public double RetrieveProbability(NGram<T> t)
        {
            return (double)this.RetrieveCount(t) / (double)this.Size;
        }

        public Dictionary<NGram<T>, int> Copy()
        {
            return new Dictionary<NGram<T>, int>(this.distrubution);
        }

        public IEnumerator<KeyValuePair<NGram<T>, int>> GetEnumerator()
        {
            return this.distrubution.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.distrubution.GetEnumerator();
        }
    }
}
