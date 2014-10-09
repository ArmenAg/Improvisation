using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.SmartWalkers
{
    public class NGramLinearSizeVsTimeWeightAssigner<T> : INGramWeightAssigner<T>
        where T : IEquatable<T>
    {
        public readonly float SizeWeight;
        public readonly float TimeWeight;

        public NGramLinearSizeVsTimeWeightAssigner(float sizeWeight)
        {
            (sizeWeight >= 0 && sizeWeight <= 1).AssertTrue();

            this.SizeWeight = sizeWeight;
            this.TimeWeight = 1 - this.SizeWeight;
        }


        public Dictionary<NGram<T>, float> NextPossibleStateAssignment(NGram<T>[] past, NGramGraphMarkovChain<T> subGraph)
        {
            return this.NextPossibleStateAssignment(subGraph);
        }

        private Dictionary<NGram<T>, float> NextPossibleStateAssignment(NGramGraphMarkovChain<T> subGraph)
        {
            int timeMax = subGraph.Max(x => x.Value.Any() ? x.Value.Count(y => y.Edge.N) : 0);
            int sizeMax = subGraph.Max(y => y.Key.N);

            Func<int, int, float, float> func = new Func<int, int, float, float>((max, value, weight) => ((float)value / (float)max) * weight);

            var k = subGraph
                    .Skip(1)
                        .ToDictionary(
                                x => x.Key,
                                y => func(sizeMax, y.Key.N, this.SizeWeight)
                                    + func(timeMax, y.Value.Any() ? y.Value.Max(z => z.Edge.N) : 0, this.TimeWeight));

            var lastmax = k.Max(x => x.Value);
            return k.ToDictionary(x => x.Key,
                y => y.Value / lastmax);
        }
    }
}
