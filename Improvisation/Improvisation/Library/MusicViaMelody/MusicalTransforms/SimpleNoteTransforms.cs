using Improvisation.Library.Music;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.MusicViaMelody.MusicalTransforms
{
    public static class SimpleNoteTransforms
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FullNote OctaveTransform(FullNote note)
        {
            return new FullNote(note.Note, note.Octave + 1);
        }
    }
}
