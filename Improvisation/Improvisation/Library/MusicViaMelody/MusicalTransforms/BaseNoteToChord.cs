using Improvisation.Library.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.MusicViaMelody.MusicalTransforms
{
    public abstract class BaseNoteToChord : IChordTransformRule
    {
        public virtual string TransformName
        {
            get { return "Note to Chord"; }
        }

        public abstract int SizeOfChord { get; }
        public abstract Chord Transform(Chord baseMelody);
    }
}
