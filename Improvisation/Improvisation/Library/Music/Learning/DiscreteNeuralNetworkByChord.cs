using AForge.Neuro;
using AForge.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Music
{
    [Serializable]
    public sealed class DiscreteNeuralNetworkByChord
    {
        public const double BADWEIGHT = 0D;
        public static double OkayWeight { get; set; }
        public static int HiddenLayerSize { get; set; }

        public const double GOODWEIGHT = 1D;

        public int InputsCount
        {
            get
            {
                return this.ActivationNetwork.InputsCount;
            }
        }

        public readonly ActivationNetwork ActivationNetwork;

        [NonSerialized]
        public readonly ISupervisedLearning LearningMethod;

        public readonly double Max;

        private readonly Tuple<double[], double[]>[] trainingData;

        public DiscreteNeuralNetworkByChord(List<NGram<Chord>[]> bad, List<NGram<Chord>[]> okay, List<NGram<Chord>[]> good, IActivationFunction function)
        {
            bad.NullCheck();
            okay.NullCheck();
            good.NullCheck();

            bad.Any().AssertTrue();
            okay.Any().AssertTrue();
            good.Any().AssertTrue();

            List<Tuple<double[], double[]>> input = new List<Tuple<double[], double[]>>(bad.Count + okay.Count + good.Count);

            input.AddRange(
                bad.Select(x => new Tuple<double[], double[]>(
                    x.SelectMany(y => y.SelectMany(p => ConvertChordIntoTrainingInput(p))).ToArray(),
                    Enumerable.Repeat<double>(DiscreteNeuralNetworkByChord.BADWEIGHT, bad.Count).ToArray())));

            input.AddRange(
                okay.Select(x => new Tuple<double[], double[]>(
                    x.SelectMany(y => y.SelectMany(p => ConvertChordIntoTrainingInput(p))).ToArray(),
                    Enumerable.Repeat<double>(OkayWeight, okay.Count).ToArray())));

            input.AddRange(
                good.Select(x => new Tuple<double[], double[]>(
                    x.SelectMany(y => y.SelectMany(p => ConvertChordIntoTrainingInput(p))).ToArray(),
                    Enumerable.Repeat<double>(DiscreteNeuralNetworkByChord.GOODWEIGHT, good.Count).ToArray())));

            this.Max = input.Max(x => x.Item1.Max());
            int minIndex = input.Min(x => x.Item1.Length);

            var normalized = input.Select(item => Tuple.Create(item.Item1.Take(minIndex).Select(x => x / this.Max).ToArray(), item.Item2.Take(minIndex).ToArray())).ToArray();

            this.trainingData = normalized.ToArray();

            this.ActivationNetwork = new ActivationNetwork(function, this.trainingData.Max(y => y.Item1.Length), (HiddenLayerSize == 0) ? 23 : HiddenLayerSize, 1);
            this.LearningMethod = new ResilientBackpropagationLearning(this.ActivationNetwork);
            this.ActivationNetwork.Randomize();

        }

        public double Train()
        {
            double debugError = this.LearningMethod.RunEpoch(this.trainingData.Select(x => x.Item1).ToArray(), this.trainingData.Select(x => x.Item2).ToArray());
            Debug.WriteLine(debugError);
            return debugError;
        }

        public double Compute(double[] training)
        {
            training.NullCheck();

            return this.ActivationNetwork.Compute(training).First();
        }

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
            catch
            {
                return false;
            }
            return true;
        }
        public static DiscreteNeuralNetworkByChord Load(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var obj = (DiscreteNeuralNetworkByChord)formatter.Deserialize(stream);
                return obj;
            }
        }

        public static double[] ConvertChordIntoTrainingInput(Chord chord)
        {
            if (!chord.Notes.Any())
            {
                return null;
            }
            if (chord.Notes.Count == 1)
            {
                var first = chord.Notes.First();
                return new double[] { FromFullNote(first) };
            }
            double[] ret = new double[chord.Notes.Count + 2];
            ret[0] = -1;
            for (int i = 0; i < chord.Notes.Count; i++)
            {
                ret[i + 1] = FromFullNote(chord.Notes[i]);
            }
            ret[chord.Notes.Count + 1] = -1;
            return ret;
        }
        private static double FromFullNote(FullNote first)
        {
            return first.Octave * 11 + (double)first.Note;
        }

    }
}
