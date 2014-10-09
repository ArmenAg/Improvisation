using Improvisation.Library.Music;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.MusicViaMelody.MusicalTransforms
{
    public sealed class OctaveNoteToChord : BaseNoteToChord
    {
        public override int SizeOfChord
        {
            get { return 2; }
        }

        public override Chord Transform(Chord baseMelody)
        {
            Debug.Assert(baseMelody.Notes.Count != 1);
        }
        private Tuple<ChannelMessage,ChannelMessage> OnAndOff
    }
}
