using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.SmartWalkers
{
    public class NGramMetaWeightAssigner<T> : INGramWeightAssigner<T>
        where T : IEquatable<T>
    {
        private Dictionary<INGramWeightAssigner<T>, float> functionsWithWeight;
        public NGramMetaWeightAssigner(Dictionary<INGramWeightAssigner<T>, float> func)
        {
            func.NullCheck();
            foreach (var item in func)
            {
                item.Key.NullCheck();
                (item.Value >= 0).AssertTrue();
            }
            this.functionsWithWeight = func.Normalize();
        }
        public Dictionary<NGram<T>, float> NextPossibleStateAssignment(NGram<T>[] past, NGramGraphMarkovChain<T> subGraph)
        {
            Dictionary<INGramWeightAssigner<T>, Dictionary<NGram<T>, float>> list = this.functionsWithWeight
                .ToDictionary(
                    item => item.Key,
                    item => item.Key.NextPossibleStateAssignment(past, subGraph));

            Dictionary<NGram<T>, float> built = new Dictionary<NGram<T>, float>();

            foreach (var assigner in list)
            {
                foreach (var item in assigner.Value)
                {
                    if (built.ContainsKey(item.Key))
                    {
                        built[item.Key] = built[item.Key] + (this.functionsWithWeight[assigner.Key] * item.Value);
                        continue;
                    }
                    built.Add(item.Key, (this.functionsWithWeight[assigner.Key] * item.Value));
                }
            }
            return built;
        }
    }
}
