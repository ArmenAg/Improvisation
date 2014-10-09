using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Music
{
    public abstract class BaseNoteRetriever
    {
        internal const int MAXINDEXFORNOTE = 11;

        public abstract int MinIndex { get; }
        public abstract int MaxIndex { get; }

        public abstract int LowNoteID { get; }
        public abstract int HighNoteID { get; }

        public bool IndexInRange(int i)
        {
            return (i >= this.MinIndex && i < this.MaxIndex);
        }
        public bool ValidNoteId(ChannelMessage message)
        {
            int realId = message.Data1 - this.LowNoteID;
            return realId >= 0;
        }
        public virtual FullNote RetrieveNote(ChannelMessage message)
        {
            this.IndexInRange((int)message.Instrument());

            int realId = message.Data1 - this.LowNoteID;

            if (realId < 0)
            {
                throw new ArgumentException();
            }
            int note = 0;
            for (int i = 0; i <= realId; i++)
            {
                if (note == MAXINDEXFORNOTE)
                {
                    note = 0;
                }
                else
                {
                    note++;
                }
            }
            return new FullNote(this.Transcribe(note), realId / MAXINDEXFORNOTE);
        }

        public abstract Note Transcribe(int note);
    }
    public struct FullNote : IEquatable<FullNote>
    {
        public readonly Note Note;

        public readonly int Octave;

        public FullNote(Note note, int octave)
        {
            (octave >= 0).AssertTrue();

            this.Note = note;
            this.Octave = octave;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(FullNote other)
        {
            return this.Note == other.Note
                && this.Octave == other.Octave;
        }

        public static bool operator ==(FullNote first, FullNote second)
        {
            return first.Equals(second);
        }
        public static bool operator !=(FullNote first, FullNote second)
        {
            return !first.Equals(second);
        }

        public override bool Equals(object obj)
        {
            if (obj is FullNote)
            {
                return this.Equals((FullNote)obj);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashHelper.FNVHashCode((int)this.Note, this.Octave);
        }
        public override string ToString()
        {
            return string.Format("[{0} {1}]", this.Note, this.Octave);
        }
    }
}
