using Improvisation.Library.Search;
using Improvisation.Library.SmartWalkers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Music
{
    public sealed class ChordChainGeneticFunction
        : IGeneticFunctions<NGram<Chord>[]>
    {
        public enum ChordRandomFunctionType
            : byte
        {
            NoRandomSelection = 0,
            AllowRandomSelection = 1
        }
        public enum ChordCrossFunctionType
            : byte
        {
            DiscreteChoice = 0,
            Merge = 1
        }

        public int ChordDepth { get; set; }
        public int NGramDepth { get; set; }

        public double MutateCoefficient { get; set; }

        public double RandomSelectionCoefficient
        {
            get
            {
                return this.randomSelectionCoefficient;
            }
            set
            {
                if (this.RandomFunctionType == ChordRandomFunctionType.NoRandomSelection)
                {
                    this.randomSelectionCoefficient = value;
                    return;
                }
                this.randomSelectionCoefficient = value;
            }
        }

        public readonly ChordRandomFunctionType RandomFunctionType;
        public readonly ChordCrossFunctionType CrossFunctionType;

        public readonly INGramWeightAssigner<Chord> Assigner;

        public readonly DiscreteNeuralNetworkByChord NeuralNetwork;
        public readonly NGramGraphMarkovChain<Chord> NGramGraph;

        private double randomSelectionCoefficient;
        private Random random;

        public ChordChainGeneticFunction(
            DiscreteNeuralNetworkByChord dnn,
            NGramGraphMarkovChain<Chord> chord,
            INGramWeightAssigner<Chord> assigner,
            ChordRandomFunctionType type = ChordRandomFunctionType.NoRandomSelection,
            ChordCrossFunctionType cross = ChordCrossFunctionType.DiscreteChoice)
        {
            dnn.NullCheck();
            chord.NullCheck();
            assigner.NullCheck();

            this.NeuralNetwork = dnn;
            this.NGramGraph = chord;
            this.RandomFunctionType = type;
            this.CrossFunctionType = cross;
            this.ChordDepth = this.NeuralNetwork.InputsCount * 2;
            this.NGramDepth = 10;
            this.MutateCoefficient = 0.05D;
            this.Assigner = assigner;

            this.random = new Random();

            switch (type)
            {
                case ChordRandomFunctionType.AllowRandomSelection:
                    this.RandomSelectionCoefficient = .05D;
                    return;
            }
        }

        public NGram<Chord>[] Mutate(NGram<Chord>[] t)
        {
            NGram<Chord>[] mutated = new NGram<Chord>[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                if (this.random.NextDouble() < this.MutateCoefficient)
                {
                    switch (this.RandomFunctionType)
                    {
                        case ChordRandomFunctionType.NoRandomSelection:
                            mutated[i] = this.Assigner.NextPossibleStateAssignment(mutated.Take(i).ToArray(), this.NGramGraph.GetSubGraphFromNGram(t[i], this.NGramDepth)).PickRandomFromProbabilityDistrubutionsSafe();
                            break;
                        case ChordRandomFunctionType.AllowRandomSelection:
                            if (random.NextDouble() < this.RandomSelectionCoefficient)
                            {
                                mutated[i] = this.NGramGraph.PickRandom().Key;
                                break;
                            }
                            break;
                        //goto case ChordRandomFunctionType.NoRandomSelection;
                    }
                    continue;
                }
                mutated[i] = t[i];
            }
            return mutated;
        }

        public NGram<Chord>[] Cross(NGram<Chord>[] left, NGram<Chord>[] right)
        {
            if (left == null && right == null)
            {
                return this.Random();
            }
            if (left == null)
            {
                return right;
            }
            if (right == null)
            {
                return left;
            }
            int min = Math.Min(left.Length, right.Length);
            if (min == 0)
            {
                return this.Random();
            }
            var chords = Enumerable.Range(0, min).Select(x => this.SubCross(left[x], right[x])).ToList();
            int dif = Math.Abs(left.Length - right.Length);

            if (dif == 0)
            {
                return chords.ToArray();
            }

            var leftOrRight = (left.Length < right.Length) ? right : left;
            try
            {
                chords.AddRange(Enumerable.Range(min, this.random.Next(min, leftOrRight.Length)).Where(x => x < leftOrRight.Length).Select(x => leftOrRight[x]));
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
            return chords.ToArray();
        }

        public NGram<Chord>[] Random()
        {
            List<NGram<Chord>> a = new List<NGram<Chord>>(this.ChordDepth);
            var node = this.NGramGraph.PickRandom().Key;
            while (!this.NGramGraph.ValidNode(node))
            {
                node = this.NGramGraph.PickRandom().Key;
            }
            for (int i = 0; i < this.ChordDepth; i++)
            {
                a.Add(node);
                switch (this.RandomFunctionType)
                {
                    case ChordRandomFunctionType.NoRandomSelection:
                        node = this.Assigner.NextPossibleStateAssignment(a.ToArray(), this.NGramGraph.GetSubGraphFromNGram(node, this.NGramDepth)).PickRandomFromProbabilityDistrubutionsSafe();
                        break;
                    case ChordRandomFunctionType.AllowRandomSelection:
                        if (random.NextDouble() < this.RandomSelectionCoefficient)
                        {
                            node = this.NGramGraph.PickRandom().Key;
                            break;
                        }
                        break;
                    //goto case ChordRandomFunctionType.NoRandomSelection;
                }
            }
            return a.ToArray();
        }

        public double Fitness(NGram<Chord>[] t)
        {
            var training = t.SelectMany(x => x.SelectMany(y => DiscreteNeuralNetworkByChord.ConvertChordIntoTrainingInput(y))).ToList();
            if (training.Count < this.NeuralNetwork.InputsCount)
            {
                training.AddRange(Enumerable.Repeat(-2D, this.NeuralNetwork.InputsCount - training.Count));
                return this.NeuralNetwork.Compute(training.ToArray());
            }

            var data = this.Partition(training);
            var query = data.Where(x => x.Length == this.NeuralNetwork.InputsCount).ToList();
            var error = query.Select(x => this.NeuralNetwork.Compute(x)).Sum();

            if (data.Count != query.Count)
            {
                int count = data.Last().Length;
                float percent = (float)count / (float)query.Count * (float)this.NeuralNetwork.InputsCount;
                error *= percent;
            }
            return error;
        }

        private NGram<Chord> SubCross(NGram<Chord> left, NGram<Chord> right)
        {
            switch (this.CrossFunctionType)
            {
                case ChordCrossFunctionType.DiscreteChoice:
                    if (this.random.NextDouble() < 0.5D)
                    {
                        return left;
                    }
                    return right;
                case ChordCrossFunctionType.Merge:
                    int min = Math.Min(left.N, right.N);
                    List<Chord> chords = Enumerable.Range(0, min).Select(x => (this.random.NextDouble() < 0.5) ? left[x] : right[x]).ToList();
                    int dif = Math.Abs(left.N - right.N);
                    if (dif == 0)
                    {
                        return new NGram<Chord>(chords.ToArray());
                    }
                    if (left.N != right.N)
                    {
                        var leftOrRight = (left.N < right.N) ? right : left;

                        chords.AddRange(Enumerable.Range(min, this.random.Next(min, leftOrRight.N)).Where(x => x < leftOrRight.N).Select(x => leftOrRight[x]));
                    }
                    return new NGram<Chord>(chords.ToArray());
            }
            throw new NotImplementedException();
        }
        private List<double[]> Partition(List<double> t)
        {
            int local = this.NeuralNetwork.InputsCount;
            int amountSplit = t.Count / local;
            List<double[]> ret = new List<double[]>(amountSplit);

            for (int i = 0; i < amountSplit; i++)
            {
                ret.Add(t.GetRange(i * local, local).ToArray());
            }
            int last = (amountSplit - 1) * local;
            ret.Add(t.GetRange(last, t.Count - last).ToArray());
            return ret;
        }
    }
}
