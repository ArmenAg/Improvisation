using AForge.Neuro;
using Improvisation.Library;
using Improvisation.Library.Clustering;
using Improvisation.Library.Distance;
using Improvisation.Library.Music;
using Improvisation.Library.Search;
using Improvisation.Library.SmartWalkers;
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
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Improvisation
{
    public partial class MoreComplexNoteGraph
        : BaseGraphUI<Chord>
    {
        private AsyncOutputPlayer asyncPlayer = new AsyncOutputPlayer();

        private async void GenerateThemeTask()
        {
            if (this.asyncPlayer.Playing)
            {
                this.asyncPlayer.Stop();
            }
            this.asyncPlayer.Play(this.PathOfRandomWalk);
        }

        public override int WalkerDepth
        {
            get { return 20; }
        }

        public override bool IsHeterogenous
        {
            get { return true; }
        }

        public override void FormLoad(object sender, EventArgs e)
        {
            PianoNoteRetriever retriever = new PianoNoteRetriever();

            var midiEvents = new InstrumentMidiEventProducer(
                Directory.EnumerateFiles(@"C:\Users\armen_000\Documents\Visual Studio 2013\Projects\Improvisation\Improvisation\bin\Debug\MusicSeperated\RayCharles").Select(x => new Sequence(x)));
            var midi = midiEvents.GetOrderedMessages(GeneralMidiInstrument.AcousticGrandPiano);
            var accords = Chord.RetrieveChords(midi, retriever);

            //    var mahboyeminem = Chord.RetrieveChords(
            //      new InstrumentMidiEventProducer(Directory.EnumerateFiles("MusicSeperated\\Eminem").Select(x => new Sequence(x))).GetOrderedMessages(GeneralMidiInstrument.AcousticGrandPiano),
            //    retriever);


            var grams = HeterogenousNGrams<Chord>.BuildNGrams(3, 7, accords.ToList());
            this.MarkovGraph = new NGramGraphMarkovChain<Chord>(grams);

            this.wpfContainer.Child = GenerateWpfVisuals(GraphUIHelper.GenerateGraphUI<Chord>(new NGramGraphMarkovChain<Chord>(HomogenousNGrams<Chord>.DirectBuiltUnsafe(new NGram<Chord>().AsEnumerableObject(), 1))));

            this.ZoomControl.ZoomToFill();

            this.GraphArea.GenerateGraph(true);
            this.GraphArea.SetVerticesDrag(true, true);

            this.UpdateThemeListBox();
        }

        public override void NGramListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            base.NGramListBoxSelectedIndexChanged(sender, e);

            Task task = Task.Factory.StartNew(() => this.GenerateThemeTask());
        }

        public override NGram<Chord>[] RetrieveChain()
        {
            INGramWeightAssigner<Chord> sizeTime = new NGramLinearSizeVsTimeWeightAssigner<Chord>((float)this.trackBar.Value / (float)this.trackBar.Maximum);
            INGramWeightAssigner<Chord> size = new NGramLinearSizeDistrubutionAssigner<Chord>(this.MarkovGraph, 20);

            Dictionary<INGramWeightAssigner<Chord>, float> assigners = new Dictionary<INGramWeightAssigner<Chord>, float>()
                {
                    {sizeTime,0.5F},
                    {size,0.5F}
                };

            INGramWeightAssigner<Chord> meta = new NGramMetaWeightAssigner<Chord>(assigners);

            NGramBaseRandomGraphWalker<Chord> walker = new NGramBaseRandomGraphWalker<Chord>(meta);

            walker.LoadGraph(this.MarkovGraph);
            var chain = walker.NextMultiple(this.GetStartingNode().Node.AsEnumerableObject().ToArray(), this.WalkerDepth).ToArray();
            return chain;
        }

        public IReadOnlyCollection<NGram<Chord>[]> Themes(INGramWeightAssigner<Chord> assigner)
        {
            NGramGraphChainRetriever<Chord> mostProbable = new NGramGraphChainRetriever<Chord>(this.MarkovGraph);

            var chains = mostProbable.FromEachNodeRandom(assigner, this.WalkerDepth, 3);


            //KMeans<NGram<Chord>> cluster = new KMeans<NGram<Chord>>(chains.ToArray(), new LevenshteinDistance<NGram<Chord>>(), 500);
            //cluster.RunFrames(2);

            DiscreteNeuralNetworkByChord teacher = DiscreteNeuralNetworkByChord.Load(@"C:\Users\armen_000\Documents\Visual Studio 2013\Projects\Improvisation\Improvisation\bin\Debug\Learning\eminemNN.txt");

            ChordChainGeneticFunction function = new ChordChainGeneticFunction(
                 teacher,
                 this.MarkovGraph,
                 assigner,
                 ChordChainGeneticFunction.ChordRandomFunctionType.AllowRandomSelection,
                 ChordChainGeneticFunction.ChordCrossFunctionType.DiscreteChoice) { RandomSelectionCoefficient = 0.3D };

            GeneticAlgorithm<NGram<Chord>[]> genetic = new GeneticAlgorithm<NGram<Chord>[]>(
                function,
                new GeneticSettings(0.1F, 0.05f, 5000, GeneticSettings.OrderOfEvolution.MutateCrossover),
                chains.Take(500));

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var l = genetic.SingleEvolutionaryCycle();
                }
                catch (Exception e)
                {
                    continue;
                }

                // KMeans<NGram<Chord>> chords = new KMeans<NGram<Chord>>(
                // genetic.CurrentPopulation.Select(x => x.Value).RandomValues(1000),
                // new GaussianNoiseDistance<NGram<Chord>>(new ChordHammingDistance(), 010F), 1000);

                // chords.RunFrames(3);
                // genetic.SubstitutePopulation(chords.Centers);
            }

            return genetic.ToList().AsReadOnly();
        }


        public async virtual Task UpdateThemeListBox()
        {
            this.themeListBox.Items.Clear();

            INGramWeightAssigner<Chord> sizeTime = new NGramLinearSizeVsTimeWeightAssigner<Chord>((float)this.trackBar.Value / (float)this.trackBar.Maximum);
            INGramWeightAssigner<Chord> size = new NGramLinearSizeDistrubutionAssigner<Chord>(this.MarkovGraph, 20);

            Dictionary<INGramWeightAssigner<Chord>, float> assigners = new Dictionary<INGramWeightAssigner<Chord>, float>()
                    {
                    {sizeTime,0.5F},
                    {size,0.5F}
                };

            INGramWeightAssigner<Chord> meta = new NGramMetaWeightAssigner<Chord>(assigners);

            var chords = this.Themes(new NGramIDWeightAssigner<Chord>(this.MarkovGraph));
            foreach (var item in chords)
            {
                this.themeListBox.Items.Add(item);
            }
        }
    }
    public partial class MoreComplexNoteGraph
    {
        private TrackBar trackBar;
        private System.Windows.Forms.ListBox themeListBox;

        public override void InitializeComponent()
        {
            this.trackBar = new TrackBar();
            this.trackBar.Minimum = 0;
            this.trackBar.Maximum = 100;
            this.trackBar.Location = new Point(12, 375);
            this.trackBar.Size = new System.Drawing.Size(159, 20);

            this.Controls.Add(this.trackBar);

            this.themeListBox = new System.Windows.Forms.ListBox();
            this.themeListBox.Location = new Point(12, 75 + this.trackBar.Location.Y);
            this.themeListBox.Size = new Size(159, 290);
            this.themeListBox.SelectedValueChanged += ThemeListBoxSelectedValueChanged;
            this.Controls.Add(this.themeListBox);

            base.InitializeComponent();
        }

        private async void ThemeListBoxSelectedValueChanged(object sender, EventArgs e)
        {
            var item = (NGram<Chord>[])(this.themeListBox.SelectedItem);
            this.PathOfRandomWalk = HomogenousNGrams<Chord>.DirectBuiltUnsafe(item, 1);

            Task task = Task.Factory.StartNew(() => this.GenerateThemeTask());

            LilyPondBuilder builder = new LilyPondBuilder(this.PathOfRandomWalk.SelectMany(x => x).ToList()) { NoteByNote = true, HasOctavePrefix = true };
            this.sentenceTextBox.Text = builder.ToString();
        }
    }
}
