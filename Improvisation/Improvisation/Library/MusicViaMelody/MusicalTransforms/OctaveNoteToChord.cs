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
            throw new NotImplementedException();
        }
        private Tuple<ChannelMessage, ChannelMessage> OnAndOff()
        {
            throw new NotImplementedException();
        }
    }
}
