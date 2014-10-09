using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.SmartWalkers
{
    public class NGramGraphChainRetriever<T>
        where T : IEquatable<T>
    {
        public int NodeDepthBeforeGreedy { get; set; }
        public int Max { get; set; }
        private readonly NGramGraphMarkovChain<T> graph;

        public NGramGraphChainRetriever(NGramGraphMarkovChain<T> a)
        {
            a.NullCheck();

            this.graph = a;
            this.NodeDepthBeforeGreedy = 100;
            this.Max = int.MaxValue;
        }

        public List<NGram<T>[]> FromEachNodeGreedy(int depth)
        {
            (depth > 0).AssertTrue();

            List<NGram<T>[]> ret = new List<NGram<T>[]>(this.graph.Count());
            foreach (var item in this.graph)
            {
                ret.Add(this.NodeGreedy(item.Key, depth));
            }
            return ret;
        }
        public List<NGram<T>[]> FromEachNodeRandom(INGramWeightAssigner<T> assigner, int depth)
        {
            return this.FromEachNodeRandom(assigner, depth, 1);
        }
        public List<NGram<T>[]> FromEachNodeRandom(INGramWeightAssigner<T> assigner, int depth, int perNode)
        {
            ConcurrentBag<NGram<T>[]> ret = new ConcurrentBag<NGram<T>[]>();
            Parallel.ForEach(this.graph, (item, breaka) =>
            {
                if (ret.Count < this.Max)
                {
                    for (int i = 0; i < perNode; i++)
                    {
                        ret.Add(this.NodeRandom(assigner, item.Key, depth));
                    }
                }
                else
                {
                    breaka.Break();
                }
            });

            return ret.ToList();
        }


        private NGram<T>[] NodeGreedy(NGram<T> nGram, int depth)
        {
            List<NGram<T>> ret = new List<NGram<T>>(depth);
            ret.Add(nGram);
            if (depth <= 0)
            {
                return ret.ToArray();
            }
            else
            {
                ret.AddRange(this.NodeGreedy(this.graph.Edges(nGram).OrderByDescending(x => x.Probability).First().Edge, --depth));
                return ret.ToArray();
            }
        }
        private NGram<T>[] NodeRandom(INGramWeightAssigner<T> assigner, NGram<T> nGram, int depth)
        {
            List<NGram<T>> ret = new List<NGram<T>>(depth);
            ret.Add(nGram);
            if (depth <= 0)
            {
                return ret.ToArray();
            }
            else
            {
                var assign = assigner.NextPossibleStateAssignment(nGram.AsEnumerableObject().ToArray(), this.graph);
                if (!assign.Any())
                {
                    return ret.ToArray();
                }
                ret.AddRange(this.NodeRandom(assigner, assign.PickRandomFromProbabilityDistrubutionsSafe(), --depth));

                return ret.ToArray();
            }
        }
    }
}
