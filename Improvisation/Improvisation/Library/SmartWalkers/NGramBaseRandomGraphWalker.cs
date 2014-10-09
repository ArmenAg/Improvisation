using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Improvisation;
using Improvisation.Library;

namespace Improvisation.Library.SmartWalkers
{
    public class NGramBaseRandomGraphWalker<T> : INGramRandomGraphWalker<T>
        where T : IEquatable<T>
    {
        public int Depth { get; set; }

        public NGramGraphMarkovChain<T> Graph { get; private set; }

        public readonly INGramWeightAssigner<T> WeightAssigner;

        public NGramBaseRandomGraphWalker(INGramWeightAssigner<T> assighner)
        {
            assighner.NullCheck();

            this.WeightAssigner = assighner;
            this.Depth = 3;
        }

        public void LoadGraph(NGramGraphMarkovChain<T> graph)
        {
            graph.NullCheck();

            this.Graph = graph;
        }

        public NGram<T> Next(NGram<T>[] chain)
        {
            chain.NullCheck();
            this.GraphCheck();

            var key = this.WeightAssigner.NextPossibleStateAssignment(chain, this.Graph.GetSubGraphFromNGram(chain.Last(), this.Depth));
            return key.Normalize().PickRandomFromProbabilityDistrubutionsSafe();
        }

        public IEnumerable<NGram<T>> NextMultiple(NGram<T>[] chain, int k)
        {
            chain.NullCheck();
            this.GraphCheck();
            List<NGram<T>> list = new List<NGram<T>>(chain);
            for (int i = 0; i < k; i++)
            {
                list.Add(this.Next(list.ToArray()));
            }
            return list.GetRange(list.Count - k, k);
        }
        public void GraphCheck()
        {
            if (null == this.Graph)
            {
                throw new ArgumentException("Must Load Graph First");
            }
        }
    }
}
