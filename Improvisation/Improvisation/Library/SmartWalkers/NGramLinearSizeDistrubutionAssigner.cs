using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.SmartWalkers
{
    public class NGramLinearSizeDistrubutionAssigner<T> : INGramWeightAssigner<T>
        where T : IEquatable<T>
    {
        public readonly int WindowSize;
        public readonly Dictionary<int, int> Distrubution;

        public NGramLinearSizeDistrubutionAssigner(NGramGraphMarkovChain<T> fullGraph, int windowSize = 20)
        {
            (windowSize > 5).AssertTrue();
            fullGraph.Any().AssertTrue();

            this.WindowSize = windowSize;
            this.Distrubution = RetrieveDistrubution(fullGraph.Grams);
        }

        private static Dictionary<int, int> RetrieveDistrubution(IEnumerable<NGram<T>> fullGraph)
        {
            var k = new Dictionary<int, int>();

            foreach (var item in fullGraph)
            {
                if (!k.ContainsKey(item.N))
                {
                    k.Add(item.N, 1);
                    continue;
                }
                k[item.N]++;
            }
            return k;
        }
        private static Dictionary<int, int> SubtractedDistrubutions(Dictionary<int, int> total, Dictionary<int, int> small)
        {
            Dictionary<int, int> subtractedD = new Dictionary<int, int>();
            foreach (var item in total)
            {
                int smallValue = (small.TryGetValue(item.Key, out smallValue)) ? smallValue : 0;
                subtractedD.Add(item.Key, item.Value - smallValue);
            }
            return subtractedD;
        }

        public Dictionary<NGram<T>, float> NextPossibleStateAssignment(NGram<T>[] past, NGramGraphMarkovChain<T> subGraph)
        {
            var possibleChoices = subGraph.Select(x => x.Key).ToList();

            var num = Math.Min(possibleChoices.Count, past.Length);

            var tempDictionary = NGramLinearSizeDistrubutionAssigner<T>.RetrieveDistrubution(possibleChoices.GetRange(possibleChoices.Count - num, num));
            var subtracted = NGramLinearSizeDistrubutionAssigner<T>.SubtractedDistrubutions(this.Distrubution, tempDictionary);
            var normalizationFactor = subtracted.Sum(x => Math.Abs(x.Value));
            var shiftFactor = subtracted.Where(x => x.Value < 0).Select(x => x.Value).Sum();

            return possibleChoices.ToDictionary(
                item => item,
                item => ((float)this.Distrubution[item.N] + (float)shiftFactor) / normalizationFactor);
        }
    }
}











