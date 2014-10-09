using GraphX.Controls;
using GraphX.GraphSharp.Algorithms.Layout.Simple.FDP;
using GraphX.GraphSharp.Algorithms.OverlapRemoval;
using GraphX.Logic;
using Improvisation.Library;
using Improvisation.Library.Data;
using Improvisation.Library.Music;
using QuickGraph;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Improvisation
{
    public class NoteGraph
        : BaseGraphUI<SimplisticPianoNote>
    {
        private async void GenerateThemeTask()
        {
            NGramRandomGraphMarkovChainWalker<SimplisticPianoNote> note = new NGramRandomGraphMarkovChainWalker<SimplisticPianoNote>(this.MarkovGraph, this.MarkovGraph.First().Key, new Random());
            var path = note.WalkMarkovChain(100, NGramRandomGraphMarkovChainWalker<SimplisticPianoNote>.NextStateType.Homogenous);

            //OutputConstants.Play(path.SelectMany(x => x.Select(y => y.Message)));
        }

        public override int WalkerDepth
        {
            get { return 100; }
        }

        public override bool IsHeterogenous
        {
            get { return true; }
        }

        public override void FormLoad(object sender, EventArgs e)
        {
            PianoNoteRetriever retriever = new PianoNoteRetriever();

            var midiEvents = new InstrumentMidiEventProducer(Directory.EnumerateFiles("Music").Skip(4).Take(1).Select(x => new Sequence(x)));

            var k = midiEvents.GetOrderedMessages(GeneralMidiInstrument.AcousticGrandPiano);
            var accords = Chord.RetrieveChords(k, retriever);
            accords.Count();

            var prod = new InstrumentChannelMessengerProducer(Directory.EnumerateFiles("Music").Select(x => new Sequence(x)))
                .GetOrderedMessages(GeneralMidiInstrument.AcousticGrandPiano)
                .Where(x => retriever.ValidNoteId(x))
                .Select(x => new SimplisticPianoNote(x))
                .ToList();

            var grams = HeterogenousNGrams<SimplisticPianoNote>.BuildNGrams(1, 5, prod);
            this.MarkovGraph = new NGramGraphMarkovChain<SimplisticPianoNote>(grams);

            this.wpfContainer.Child = GenerateWpfVisuals(GraphUIHelper.GenerateGraphUI<SimplisticPianoNote>(this.MarkovGraph));
            this.ZoomControl.ZoomToFill();

            this.GraphArea.GenerateGraph(true);
            this.GraphArea.SetVerticesDrag(true, true);
        }

        public override void NGramListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            base.NGramListBoxSelectedIndexChanged(sender, e);
            this.GenerateThemeTask();
        }
    }
}
