using Sanford.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Music
{
    public class PianoNoteRetriever
        : BaseNoteRetriever
    {
        public override int MinIndex
        {
            get { return 0; }
        }

        public override int MaxIndex
        {
            get { return 6; }
        }

        public override int LowNoteID
        {
            get { return 21; }
        }

        public override int HighNoteID
        {
            get { return 109; }
        }

        public override Note Transcribe(int note)
        {
            (note >= 0 && note <= BaseNoteRetriever.MAXINDEXFORNOTE).AssertTrue();
            int note1 = (int)(note);
            switch (note1)
            {
                case 4:
                    return Note.C;
                case 5:
                    return Note.CSharp;
                case 6:
                    return Note.D;
                case 7:
                    return Note.EFlat;
                case 8:
                    return Note.E;
                case 9:
                    return Note.F;
                case 10:
                    return Note.GFlat;
                case 11:
                    return Note.G;
                case 0:
                    return Note.GSharp;
                case 1:
                    return Note.A;
                case 2:
                    return Note.BFlat;
                case 3:
                    return Note.B;
            }

            throw new NotImplementedException();
        }

    }
}
