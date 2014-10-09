using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Music
{
    public class SimplisticPianoNote
        : IEquatable<SimplisticPianoNote>
    {
        public static readonly PianoNoteRetriever PianoNoteRetriever;

        public readonly Note Note;
        public readonly ChannelMessage Message;
        public ChannelCommand Type { get { return this.Message.Command; } }

        static SimplisticPianoNote()
        {
            SimplisticPianoNote.PianoNoteRetriever = new PianoNoteRetriever();
        }
        public SimplisticPianoNote(ChannelMessage message)
        {
            message.NullCheck();
            SimplisticPianoNote.PianoNoteRetriever.IndexInRange((int)message.MidiChannel).AssertTrue();

            this.Note = SimplisticPianoNote.PianoNoteRetriever.RetrieveNote(message).Note;
            this.Message = message;
        }

        public override int GetHashCode()
        {
            return this.Note.GetHashCode() ^ (this.Type.GetHashCode() * 29);
        }
        public override string ToString()
        {
            return this.Note.ToString();
        }

        public static bool operator ==(SimplisticPianoNote one, SimplisticPianoNote second)
        {
            return one.Equals(second);
        }
        public static bool operator !=(SimplisticPianoNote one, SimplisticPianoNote second)
        {
            return !one.Equals(second);
        }

        public bool Equals(SimplisticPianoNote other)
        {
            return this.Note == other.Note && this.Type == other.Type;
        }

    }
}
