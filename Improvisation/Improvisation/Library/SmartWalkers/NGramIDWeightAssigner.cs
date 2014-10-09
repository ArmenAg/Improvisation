using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.SmartWalkers
{
    public sealed class NGramIDWeightAssigner<T> : INGramWeightAssigner<T>
        where T : IEquatable<T>
    {
        private readonly NGramGraphMarkovChain<T> graph;
        public NGramIDWeightAssigner(NGramGraphMarkovChain<T> graph)
        {
            this.graph = graph;
        }
        public Dictionary<NGram<T>, float> NextPossibleStateAssignment(NGram<T>[] past, NGramGraphMarkovChain<T> subGraph)
        {
            var start = past.Last();
            return graph.Edges(start).ToDictionary(item => item.Edge, item => item.Probability);
        }
    }
}
