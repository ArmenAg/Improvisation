using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    [Serializable]
    public class NGramGraphMarkovChain<T>
        : IEnumerable<KeyValuePair<NGram<T>, List<NonRecursiveNGramProbabilisticEdge<T>>>>
         where T : IEquatable<T>
    {
        public readonly bool IsHeterogenous;

        public readonly NGramDistribution<T> Distrubution;

        public readonly INGrams<T> Grams;

        private readonly Dictionary<NGram<T>, List<NonRecursiveNGramProbabilisticEdge<T>>> graph;

        public NGramGraphMarkovChain(INGrams<T> grams)
        {
            grams.NullCheck();


            var array = new HashSet<NGram<T>>(grams).ToArray();
            this.Distrubution = new NGramDistribution<T>(grams);
            var typeOfNGram = (grams as IHeterogenousNGrams<T>);
            this.IsHeterogenous = typeOfNGram != null;
            this.graph = new Dictionary<NGram<T>, List<NonRecursiveNGramProbabilisticEdge<T>>>();
            this.Grams = grams;
            if (!this.IsHeterogenous)
            {
                this.BuildFromHomogenousNGrams(grams, array, this.Distrubution);
            }
            else
            {
                this.BuildFromHeterogenousNGrams(typeOfNGram, array, this.Distrubution);
            }
        }
        private NGramGraphMarkovChain(bool het)
        {
            this.graph = new Dictionary<NGram<T>, List<NonRecursiveNGramProbabilisticEdge<T>>>();
            this.IsHeterogenous = het;
            this.Distrubution = new NGramDistribution<T>(new NGram<T>[0]);
            this.Grams = HomogenousNGrams<T>.DirectBuiltUnsafe(new NGram<T>[0], 1);
        }

        private NGramGraphMarkovChain(Dictionary<NGram<T>, List<NonRecursiveNGramProbabilisticEdge<T>>> dictionary, bool p, NGramDistribution<T> orginal)
        {
            this.graph = dictionary;
            this.IsHeterogenous = p;
            this.Distrubution = orginal;
        }

        public bool ValidNode(NGram<T> node)
        {
            return this.graph.ContainsKey(node);
        }

        public IReadOnlyList<NonRecursiveNGramProbabilisticEdge<T>> Edges(NGram<T> gram)
        {
            this.ValidNode(gram).AssertTrue();
            return this.graph[gram].AsReadOnly();
        }

        public NGram<T>? GetLowerConstruct(NGram<T> curState)
        {
            if (curState.N > 1)
            {
                var lowerConstructData = curState
                    .AsEnumerable()
                    .Skip(1)
                    .ToArray();

                NGram<T> lowerConstruct = new NGram<T>(lowerConstructData);

                if (this.ValidNode(lowerConstruct))
                {
                    return lowerConstruct;
                }
                return null;
            }
            return null;
        }

        public NGramGraphMarkovChain<T> GetSubGraphFromNGram(NGram<T> gram, int depth)
        {
            this.ValidNode(gram).AssertTrue();

            var set = this.RetrieveAppearedNGramsToSomeDepth(gram, depth);

            Dictionary<NGram<T>, List<NonRecursiveNGramProbabilisticEdge<T>>> dictionary = new Dictionary<NGram<T>, List<NonRecursiveNGramProbabilisticEdge<T>>>();

            foreach (var item in set)
            {
                dictionary.Add(item, new List<NonRecursiveNGramProbabilisticEdge<T>>());

                foreach (var sub in this.graph[item])
                {
                    if (set.Contains(sub.Edge))
                    {
                        dictionary[item].Add(sub);
                    }
                    else
                    {
                        dictionary[item].Add(sub);

                        if (!dictionary.ContainsKey(sub.Edge))
                        {
                            dictionary.Add(sub.Edge, new List<NonRecursiveNGramProbabilisticEdge<T>>());
                        }
                    }
                }
            }
            return new NGramGraphMarkovChain<T>(dictionary, this.IsHeterogenous, this.Distrubution.Filter(new Predicate<KeyValuePair<NGram<T>, int>>(x => dictionary.ContainsKey(x.Key))));
        }

        /*
        public bool Save(string filePath)
        {
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static NGramGraphMarkovChain<T> Load(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var obj = (NGramGraphMarkovChain<T>)formatter.Deserialize(stream);
                return obj;
            }
        }
         */

        private HashSet<NGram<T>> RetrieveAppearedNGramsToSomeDepth(NGram<T> gram, int depth, int curDepth = 0)
        {
            if (curDepth >= depth)
            {
                return new HashSet<NGram<T>>();
            }
            HashSet<NGram<T>> set = new HashSet<NGram<T>>();
            set.Add(gram);
            foreach (var item in this.graph[gram])
            {
                foreach (var sub in RetrieveAppearedNGramsToSomeDepth(item.Edge, depth, ++curDepth))
                {
                    set.Add(sub);
                }
            }
            return set;
        }

        private void BuildFromHeterogenousNGrams(IHeterogenousNGrams<T> grams, NGram<T>[] array, NGramDistribution<T> distrubution)
        {

            var blocked = NGramHeterogenousMatrixMarkovChainHelper<T>.BuildBlockedNGrams((from item in grams.SubHomogenousNGramConstructs()
                                                                                          select item.ToArray()).ToList(),
                                                                                          grams.MinN,
                                                                                          grams.MaxN);
            foreach (var item in blocked)
            {
                var data = NGramHeterogenousMatrixMarkovChainHelper<T>.CalculateProbability(item, distrubution);
                var final = new List<NonRecursiveNGramProbabilisticEdge<T>>(data.Zip(item.Value, (x, y) => new NonRecursiveNGramProbabilisticEdge<T>(x, y)));

                this.graph.Add(item.Key, final);
            }

        }
        private void BuildFromHomogenousNGrams(INGrams<T> grams, NGram<T>[] array, NGramDistribution<T> distrubution)
        {
            var blockedGrams = NGramHomogenousMatrixMarkovChainHelper<T>.BuildBlockedNGrams(grams.ToArray());
            var transitionMatrix = Matrix<float>.Build.Sparse(
                array.Length,
                array.Length,
                new Func<int, int, float>(
                        (x, y) => NGramHomogenousMatrixMarkovChainHelper<T>.CalculateProbability(blockedGrams, distrubution, array, x, y)));

            int rowIndex = 0;
            foreach (var row in transitionMatrix.EnumerateRows())
            {
                this.graph.Add(array[rowIndex], new List<NonRecursiveNGramProbabilisticEdge<T>>());

                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i] != 0)
                    {
                        this.graph[array[rowIndex]].Add(new NonRecursiveNGramProbabilisticEdge<T>(row[i], array[i]));
                    }
                }
                rowIndex++;
            }
        }

        public IEnumerator<KeyValuePair<NGram<T>, List<NonRecursiveNGramProbabilisticEdge<T>>>> GetEnumerator()
        {
            return this.graph.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        public static NGramGraphMarkovChain<T> Empty(bool heterogenous)
        {
            return new NGramGraphMarkovChain<T>(heterogenous);
        }
    }
    [Serializable]
    public struct NonRecursiveNGramProbabilisticEdge<T>
        where T : IEquatable<T>
    {
        public readonly float Probability;
        public readonly NGram<T> Edge;

        public NonRecursiveNGramProbabilisticEdge(float prob, NGram<T> edge)
        {
            (prob <= 1F && prob >= 0).AssertTrue();

            this.Probability = prob;
            this.Edge = edge;
        }
    }
}
