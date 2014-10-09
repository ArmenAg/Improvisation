using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System.Diagnostics;
namespace Improvisation.Library
{
    public class HomogenousNGramMatrixMarkovChain<T>
        where T : IEquatable<T>
    {
        public const int NumberOfDecimalPlacesForApproximation = 2;

        private readonly Matrix<float> transitionMatrix;
        private readonly Dictionary<int, NGram<T>> indices;
        private readonly NGramDistribution<T> distrubution;

        public HomogenousNGramMatrixMarkovChain(HomogenousNGrams<T> grams)
        {
            grams.NullCheck();
            (grams as IHeterogenousNGrams<T> == null).AssertTrue();

            var array = new HashSet<NGram<T>>(grams).ToArray();
            var blockedGrams = NGramHomogenousMatrixMarkovChainHelper<T>.BuildBlockedNGrams(grams.ToArray());

            this.distrubution = new NGramDistribution<T>(grams);

            this.transitionMatrix = Matrix<float>.Build.Sparse(
                array.Length,
                array.Length,
                new Func<int, int, float>(
                        (x, y) => NGramHomogenousMatrixMarkovChainHelper<T>.CalculateProbability(blockedGrams, distrubution, array, x, y)));

            this.indices = new Dictionary<int, NGram<T>>();

            for (int i = 0; i < array.Length; i++)
            {
                this.indices.Add(i, array[i]);
            }
        }

        public virtual Vector<float> RetrieveStateZero(HashSet<NGram<T>> possibleGrams)
        {
            possibleGrams.NullCheck();

            var validGrams = new HashSet<NGram<T>>(
                 possibleGrams
                .Where(x => this.indices.ContainsValue(x)));

            if (possibleGrams.Count == 1)
            {
                float[] array = new float[this.indices.Count];
                int index = this.indices.Where(x => x.Value == possibleGrams.First()).First().Key;
                array[index] = 1F;

                return Vector<float>.Build.SparseOfArray(array);
            }

            float sum = this.distrubution
                .Where(x => validGrams.Contains(x.Key))
                .Sum(y => y.Value);

            return Vector<float>.Build.Sparse(
                this.indices.Count,
                new Func<int, float>(
                    x =>
                    {
                        if (validGrams.Contains(this.indices[x]))
                        {
                            return this.distrubution.RetrieveCount(this.indices[x]) / sum;
                        }
                        return 0F;
                    }));
        }
        public virtual Vector<float> RetrieveNextState(Vector<float> state)
        {
            state.NullCheck();

            return state * this.transitionMatrix;
        }
        public virtual Vector<float> RetrieveTState(Vector<float> zeroState, int t)
        {
            zeroState.NullCheck();
            (t > 0).AssertTrue();

            Matrix<float> newTMatrix = Matrix<float>.Build.Sparse(this.transitionMatrix.RowCount, this.transitionMatrix.ColumnCount);
            this.transitionMatrix.CopyTo(newTMatrix);

            for (int i = 1; i < t; i++)
            {
                newTMatrix *= newTMatrix;
            }
            return zeroState * newTMatrix;
        }

        public virtual bool ValidState(Vector<float> possibleState)
        {
            return this.indices.Count == possibleState.Count
                && possibleState
                    .Sum()
                    .AlmostEqual(1F, HomogenousNGramMatrixMarkovChain<T>.NumberOfDecimalPlacesForApproximation);
        }

        public NGram<T> RetrieveNodeAtIndex(int i)
        {
            return this.indices[i];
        }
    }

    public static class NGramHomogenousMatrixMarkovChainHelper<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// Assuming NGram[] is homogenous
        /// </summary>
        public static float CalculateProbability(
                  Dictionary<NGram<T>, HashSet<NGram<T>>> blockedGrams,
                  NGramDistribution<T> distrubution,
                  NGram<T>[] array,
                  int x,
                  int y)
        {
            if (!blockedGrams.ContainsKey(array[x]))
            {
                return 0;
            }
            var value = blockedGrams[array[x]];
            if (!value.Contains(array[y]))
            {
                return 0;
            }

            float sumCount = distrubution
                .AsParallel()
                .Where(z => value.Contains(z.Key))
                .Sum(q => q.Value);

            return distrubution.RetrieveCount(array[y]) / sumCount;
        }

        /// <summary>
        /// Assuming the NGram[] is homogenous
        /// </summary>
        /// <param name="grams"></param>
        /// <returns></returns>
        public static Dictionary<NGram<T>, HashSet<NGram<T>>> BuildBlockedNGrams(NGram<T>[] grams)
        {
            var blockedGram = new Dictionary<NGram<T>, HashSet<NGram<T>>>();
            int n = grams.First().N;

            for (int i = n; i < grams.Length; i++)
            {
                var subject = grams[i - n];
                if (blockedGram.ContainsKey(subject))
                {
                    blockedGram[subject].Add(grams[i]);
                    continue;
                }
                blockedGram.Add(subject, new HashSet<NGram<T>>() { grams[i] });
            }
            return blockedGram;
        }
    }
    public static class NGramHeterogenousMatrixMarkovChainHelper<T>
        where T : IEquatable<T>
    {
        public static Dictionary<NGram<T>, HashSet<NGram<T>>> BuildBlockedNGrams(List<NGram<T>[]> grams, int min, int max)
        {
            grams.NullCheck();

            var blockedNGrams = new Dictionary<NGram<T>, HashSet<NGram<T>>>();
            foreach (var item in grams)
            {
                foreach (var gram in item)
                {
                    if (!blockedNGrams.ContainsKey(gram))
                    {
                        blockedNGrams.Add(gram, BuildSingleBlock(grams, gram, min, max));
                    }
                }
            }

            return blockedNGrams;
        }
        public static List<float> CalculateProbability(KeyValuePair<NGram<T>, HashSet<NGram<T>>> set, NGramDistribution<T> distrubtion)
        {
            distrubtion.NullCheck();

            float sum = set.Value.Select(x => distrubtion.RetrieveCount(x)).Sum();

            return set.Value
                .Select(x => (float)distrubtion.RetrieveCount(x) / sum)
                .ToList();
        }
        private static HashSet<NGram<T>> BuildSingleBlock(List<NGram<T>[]> grams, NGram<T> node, int min, int max)
        {
            HashSet<NGram<T>> blockedGram = new HashSet<NGram<T>>();
            List<int> indices = new List<int>(20);

            var first = grams[node.N - min];
            var n = node.N;

            for (int i = n; i < first.Length; i++)
            {
                if (first[i - n] == node)
                {
                    indices.Add(i);
                    blockedGram.Add(first[i]);
                }
            }

            for (int i = min; i < max; i++)
            {
                if (i == n)
                {
                    continue;
                }
                var gram = grams[i - min];

                foreach (var item in indices)
                {
                    if (item < gram.Length)
                    {
                        blockedGram.Add(gram[item]);
                    }
                }
            }
            return blockedGram;
        }


    }
}
