using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Improvisation.Library.SmartWalkers;

namespace Improvisation.Library.Music
{
    public sealed class DiscreteDataRetriever
    {
        public INGramWeightAssigner<Chord> Assigner { get; set; }

        public List<NGram<Chord>[]> Bad { get { return this.badOkayGoodChords.Item1; } }
        public List<NGram<Chord>[]> Okay { get { return this.badOkayGoodChords.Item2; } }
        public List<NGram<Chord>[]> Good { get { return this.badOkayGoodChords.Item3; } }


        public readonly INGrams<Chord> HomogenousWindowingData;
        public readonly IReadOnlyList<Chord> Chords;

        public readonly int WindowSize;
        public readonly int NGramSize;

        private Tuple<List<NGram<Chord>[]>, List<NGram<Chord>[]>, List<NGram<Chord>[]>> badOkayGoodChords;

        public DiscreteDataRetriever(List<Chord> chordsInOrder, int clusterSize = 2, int windowSize = 20)
        {
            chordsInOrder.NullCheck();
            chordsInOrder.Any().AssertTrue();

            this.Chords = chordsInOrder.AsReadOnly();

            this.WindowSize = windowSize;
            this.NGramSize = clusterSize;

            this.HomogenousWindowingData = HomogenousNGrams<Chord>.BuildNGrams(this.NGramSize, this.Chords.ToList());

            this.badOkayGoodChords = Tuple.Create<List<NGram<Chord>[]>, List<NGram<Chord>[]>, List<NGram<Chord>[]>>(this.RetrieveBad(), this.RetrieveOkay(), this.RetrieveGood());
        }


        private List<NGram<Chord>[]> RetrieveBad()
        {
            List<NGram<Chord>[]> bad = new List<NGram<Chord>[]>();

            bad.AddRange(Enumerable.Range(0, 40).Select(x => Enumerable.Repeat(this.HomogenousWindowingData.PickRandom(), this.WindowSize).ToArray()));
            bad.AddRange(Enumerable.Range(0, 20).Select(x => Enumerable.Range(0, this.WindowSize).Select(y => this.HomogenousWindowingData.PickRandom()).ToArray()));

            return bad;
        }

        private List<NGram<Chord>[]> RetrieveOkay()
        {
            NGramGraphMarkovChain<Chord> graph = new NGramGraphMarkovChain<Chord>(this.HomogenousWindowingData);
            NGramGraphChainRetriever<Chord> mostProbable = new NGramGraphChainRetriever<Chord>(graph);

            return mostProbable
                .FromEachNodeRandom(
                        (this.Assigner == null) ? new NGramIDWeightAssigner<Chord>(graph) : this.Assigner,
                        this.WindowSize - 1,
                        10);
        }

        private List<NGram<Chord>[]> RetrieveGood()
        {
            var window = HomogenousNGrams<NGram<Chord>>.BuildNGrams(this.WindowSize, this.HomogenousWindowingData.ToList());

            return window.Select(x => x.ToArray()).ToList();
        }
    }

}
