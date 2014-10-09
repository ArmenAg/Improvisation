using Improvisation.Library.Distance;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Music
{
    public class ChordHammingDistance : IDistance<NGram<Chord>>
    {
        private readonly HammingDistance<FullNote> distance = new HammingDistance<FullNote>();

        public double Distance(NGram<Chord>[] first, NGram<Chord>[] second)
        {
            return distance.Distance(first.SelectMany(x => x.SelectMany(y => y.Notes)).ToArray(), second.SelectMany(x => x.SelectMany(y => y.Notes)).ToArray());
        }
    }
    public class ChordLevenshteinDistance : IDistance<NGram<Chord>>
    {
        private readonly LevenshteinDistance<FullNote> distance = new LevenshteinDistance<FullNote>();

        public double Distance(NGram<Chord>[] first, NGram<Chord>[] second)
        {
            return distance.Distance(first.SelectMany(x => x.SelectMany(y => y.Notes)).ToArray(), second.SelectMany(x => x.SelectMany(y => y.Notes)).ToArray());
        }
    }
    public class NoteLevenshteinDistance : IDistance<NGram<Chord>>
    {
        private readonly LevenshteinDistance<int> distance = new LevenshteinDistance<int>();

        public double Distance(NGram<Chord>[] first, NGram<Chord>[] second)
        {
            return distance.Distance(
                first.SelectMany(x => x.SelectMany(y => y.Notes)).Select(z => z.Octave * 11 + (int)z.Note).ToArray(),
                 second.SelectMany(x => x.SelectMany(y => y.Notes)).Select(z => (int)(z.Octave * 11 + (int)z.Note)).ToArray());
        }
    }
    public class NoteEuclideanDistance : IDistance<NGram<Chord>>
    {
        private readonly EuclideanDistance<int> distance = new EuclideanDistance<int>();

        public double Distance(NGram<Chord>[] first, NGram<Chord>[] second)
        {
            return distance.Distance(
                first.SelectMany(x => x.SelectMany(y => y.Notes)).Select(z => z.Octave * 11 + (int)z.Note).ToArray(),
                 second.SelectMany(x => x.SelectMany(y => y.Notes)).Select(z => (int)(z.Octave * 11 + (int)z.Note)).ToArray());
        }
    }
}
