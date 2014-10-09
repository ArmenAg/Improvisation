using Sanford.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Music
{
    public class LilyPondBuilder
    {
        public bool NoteByNote { get; set; }
        public bool HasOctavePrefix { get; set; }

        public string Title { get; set; }
        public string SubTitle { get; set; }

        public IReadOnlyList<Chord> Chords { get { return this.innerChords.AsReadOnly(); } }

        private List<Chord> innerChords;
        public LilyPondBuilder()
        {
            this.innerChords = new List<Chord>();
            this.NoteByNote = true;
        }
        public LilyPondBuilder(List<Chord> a)
        {
            a.NullCheck();

            this.innerChords = a;
        }

        public void Append(Chord chord)
        {
            chord.NullCheck();

            this.innerChords.Add(chord);
        }

        private string Header()
        {
            bool titlee = this.Title.IsNullOrEmpty();
            bool subtitlee = this.SubTitle.IsNullOrEmpty();

            if (titlee && subtitlee)
            {
                return string.Empty;
            }

            const string header = "\\header{";
            StringBuilder builder = new StringBuilder();
            builder.Append(header);
            if (!titlee)
            {
                builder.Append(string.Format("title = \"{0}\" ", this.Title));
            }
            if (!subtitlee)
            {
                builder.Append(string.Format("subtitle = \"{0}\" ", this.SubTitle));
            }
            builder.Append("}");
            return builder.ToString();
        }
        private string SingleChord(Chord c)
        {
            if (c == null || c.Notes == null || c.Notes.Count == 0)
            {
                return string.Empty;
            }
            if (c.Notes.Count == 1)
            {
                return c.Notes.First().Note.ToString() + this.OctavePrefix(c.Notes.First());
            }
            StringBuilder builder = new StringBuilder();

            if (!this.NoteByNote)
            {
                builder.Append("<");
                foreach (var item in c.Notes)
                {
                    builder.Append(this.PureNoteString(item) + this.OctavePrefix(item) + " ");
                }
                builder.Append(">");
            }
            else
            {
                foreach (var item in c.Notes)
                {
                    lock (this)
                    {
                        builder.Append(this.PureNoteString(item) + this.OctavePrefix(item) + " ");
                    }
                }
            }
            return builder.ToString();
        }
        private string OctavePrefix(FullNote c)
        {
            if (this.HasOctavePrefix)
            {
                if (c.Octave == 4)
                {
                    return string.Empty;
                }
                else if (c.Octave < 4)
                {
                    return new string(Enumerable.Repeat(',', 4 - c.Octave).ToArray());
                }
                return new string(Enumerable.Repeat('\'', c.Octave - 4).ToArray());
            }
            return string.Empty;
        }
        private string PureNoteString(FullNote c)
        {
            const string flat = "es";
            const string sharp = "is";

            var note = c.Note.ToString().ToLower();

            if (note.Length == 1)
            {
                return note;
            }
            var firstClean = note.Replace("sharp", sharp).Replace("flat", flat);

            return firstClean.ToLower();
        }
        public override string ToString()
        {
            const string common = "\\relative c' {";

            StringBuilder builder = new StringBuilder();
            builder.Append("\version \"2.16.0\"");
            builder.Append(this.Header());

            builder.Append(common);
            foreach (var item in this.innerChords)
            {
                builder.Append(this.SingleChord(item) + " ");
            }
            builder.Append("}");

            return builder.ToString();
        }
    }
}
