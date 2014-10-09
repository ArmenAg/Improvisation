using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia;

namespace Improvisation.Library.Music
{
    internal static class MIDIExtensions
    {
        public static GeneralMidiInstrument Instrument(this ChannelMessage message)
        {
            message.NullCheck();

            return (GeneralMidiInstrument)Enum.ToObject(typeof(GeneralMidiInstrument), message.MidiChannel);
        }
    }
}
