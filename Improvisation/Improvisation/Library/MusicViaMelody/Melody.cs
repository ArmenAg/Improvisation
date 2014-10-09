using Improvisation.Library.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.MusicViaMelody
{
    public sealed class Melody
        : IEnumerable<Chord>
    {
        public IReadOnlyList<Chord> Chords { get { return this.innerChords.AsReadOnly(); } }

        private readonly List<Chord> innerChords;

        public Melody(NGram<Chord>[] stream)
            : this(stream.SelectMany(x => x).ToList()) { }
        public Melody(List<Chord> chords)
        {
            chords.NullCheck();

            this.innerChords = chords;
        }

        public IEnumerator<Chord> GetEnumerator()
        {
            return this.Chords.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return HashHelper.FNVHashCode(this.Chords.ToArray());
        }
    }
}
