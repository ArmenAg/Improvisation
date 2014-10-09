using Improvisation.FinalUI;
using Improvisation.Library;
using Improvisation.Library.Distance;
using Improvisation.Library.GraphOperations;
using Improvisation.Library.Music;
using Improvisation.Library.Search;
using Improvisation.Library.SmartWalkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Improvisation
{
    public partial class FinalUIBase : Form
    {
        private DiscreteNeuralNetworkByChord neuralNetwork;
        private NGramGraphMarkovChain<Chord> model;
        private Thread trainingThread;

        private AsyncOutputPlayer asyncPlayer = new AsyncOutputPlayer();


        public FinalUIBase()
        {
            // var a = Library.CycleFinder.BruteForceAlgorithm(new List<char>() { 'b', 'a', 'c', 'b', 'a', 'c', 'a', 'c' });
            InitializeComponent();

            this.songListBox.Enabled = false;
        }


        private void loadNeuralNetworkButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    this.neuralNetwork = DiscreteNeuralNetworkByChord.Load(file);
                    this.loadNeuralNetworkTextbox.Text = FinalUIHelperMethods.FileFriendlyString(openFileDialog1.FileName);
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Could Not Load NN", ex.Message);
                }
            }

        }
        private void createNeuralNet_Click(object sender, System.EventArgs e)
        {
            FinalUI.FinalUICreateNN m = new FinalUI.FinalUICreateNN();
            m.Show();
        }
        private void createStatModel_Click(object sender, EventArgs e)
        {
            FinalUI.FInalUICreateStatModel m = new FinalUI.FInalUICreateStatModel();
            m.Show();
        }
        private void loadStatModelButton_Click(object sender, EventArgs e)
        {
            //Temp; BUGBUG;
            this.createStatModel_Click(null, null);
            return;
            /*
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    this.model = NGramGraphMarkovChain<Chord>.Load(file);
                    this.loadStatModelTexbox.Text = FinalUIHelperMethods.FileFriendlyString(openFileDialog1.FileName);
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Could Not Load Statistical Model", ex.Message);
                }
            }
             */

        }
        private async void startMusicGeneration_Click(object sender, EventArgs e)
        {
            this.model = TemperaryVariables.Graph;
            if (null == this.model || null == this.neuralNetwork)
            {
                MessageBox.Show("Load neccessary files");
            }
            INGramWeightAssigner<Chord> assi = null;

            if (this.useWeightAssignerCheckBox.Checked)
            {
                NGramLinearSizeVsTimeWeightAssigner<Chord> sizetime = new NGramLinearSizeVsTimeWeightAssigner<Chord>(1F);
                NGramLinearSizeDistrubutionAssigner<Chord> size = new NGramLinearSizeDistrubutionAssigner<Chord>(this.model);

                Dictionary<INGramWeightAssigner<Chord>, float> dic = new Dictionary<INGramWeightAssigner<Chord>, float>()
                    {
                        {sizetime,this.sizeVsWeightAssigner.Value / 100F},
                        {size,this.countDistAssigner.Value / 100F}
                    };
                assi = new NGramMetaWeightAssigner<Chord>(dic);
            }
            else
            {
                assi = new NGramIDWeightAssigner<Chord>(this.model);
            }
            this.trainingThread = new Thread(this.GeneticSearch);
            this.trainingThread.Start(assi);
        }

        private void useWeightAssignerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.useWeightAssignerCheckBox.Checked)
            {
                this.countDistAssigner.Enabled = false;
                this.sizeVsWeightAssigner.Enabled = false;
            }
            else
            {

                this.countDistAssigner.Enabled = true;
                this.sizeVsWeightAssigner.Enabled = true;
            }
        }

        private void sizeVsWeightAssigner_Scroll(object sender, EventArgs e)
        {
            this.countDistAssigner.Value = 100 - this.sizeVsWeightAssigner.Value;
        }
        private void countDistAssigner_Scroll(object sender, EventArgs e)
        {
            this.sizeVsWeightAssigner.Value = 100 - this.countDistAssigner.Value;
        }

        private async void songListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var current = (GeneticIndividualUIElement<NGram<Chord>[]>)this.songListBox.SelectedItem;

            this.ShowMelodyDifference(current.Item);
            this.GenerateThemeTask(current.Item);
        }


        private void GeneticSearch(object a)
        {
            var assigner = (INGramWeightAssigner<Chord>)(a);
            NGramGraphChainRetriever<Chord> mostProbable = new NGramGraphChainRetriever<Chord>(this.model) { Max = 200 };

            var chains = mostProbable.FromEachNodeRandom(assigner, (int)this.walkerDepth.Value, (int)this.fromEachNode.Value);

            ChordChainGeneticFunction function = new ChordChainGeneticFunction(
                 this.neuralNetwork,
                 this.model,
                 assigner,
                 (this.allowRandomChordSelection.Checked) ? ChordChainGeneticFunction.ChordRandomFunctionType.AllowRandomSelection : ChordChainGeneticFunction.ChordRandomFunctionType.NoRandomSelection,
                 (this.mergeChordsCheckBox.Checked) ? ChordChainGeneticFunction.ChordCrossFunctionType.Merge : ChordChainGeneticFunction.ChordCrossFunctionType.DiscreteChoice) { RandomSelectionCoefficient = 0.3D };

            GeneticAlgorithm<NGram<Chord>[]> genetic = new GeneticAlgorithm<NGram<Chord>[]>(
                function,
                new GeneticSettings(0.1F, 0.05f, (int)this.geneticInitialPopulation.Value, (this.mutateCrossCheckBox.Checked) ?
                                                                                            GeneticSettings.OrderOfEvolution.MutateCrossover : GeneticSettings.OrderOfEvolution.CrossoverMutate),
                chains.Take(500));
            int iMax = (int)this.geneticEpochs.Value;

            this.progressBar1.Invoke((Action)(() => this.progressBar1.Maximum = iMax));
            for (int i = 0; i < iMax; i++)
            {
                this.progressBar1.Invoke((Action)(() => this.progressBar1.Value = i));

                var text = genetic.SingleEvolutionaryCycle().ToString();

                this.geneticErrorTextBox.Invoke((Action)(() => this.geneticErrorTextBox.Text = text));
            }
            this.progressBar1.Invoke((Action)(() => this.progressBar1.Value = this.progressBar1.Maximum));
            this.songListBox.Invoke((Action)(() => this.songListBox.Enabled = true));

            this.AddItemsToListBoxFromGenetic(genetic.CurrentPopulation);
        }
        private void AddItemsToListBoxFromGenetic(IReadOnlyList<GeneticAlgorithm<NGram<Chord>[]>.Individual> readOnlyList)
        {
            this.songListBox.Invoke((Action)(() => this.songListBox.Items.AddRange(readOnlyList.Select(x => (object)(new GeneticIndividualUIElement<NGram<Chord>[]>(x.Value, NGramHelper.ShowNGram(x.Value)))).ToArray())));
        }

        private async void GenerateThemeTask(NGram<Chord>[] path)
        {
            if (this.asyncPlayer.Playing)
            {
                this.asyncPlayer.Stop();
            }
            this.asyncPlayer.Play(path);
        }
        private async void ShowMelodyDifference(NGram<Chord>[] nGram)
        {
            NGramGraphMarkovChain<Chord> melodyGraph = new NGramGraphMarkovChain<Chord>(HomogenousNGrams<Chord>.DirectBuiltUnsafe(nGram, 1));
            NGramSemanticGraphDistance<Chord> a = new Library.GraphOperations.NGramSemanticGraphDistance<Chord>();

            string text = a.Distance(
                melodyGraph,
                TemperaryVariables.Graph,
                        (this.useSubGraphCheckBox.Checked) ?
                            NGramGraphDistanceType.SubGraph :
                            NGramGraphDistanceType.CompleteGraph
                ).ToString();

            this.graphDifferenceTextBox.Invoke((Action)(() => this.graphDifferenceTextBox.Text = text));
        }

        private void stopMusic_Click(object sender, EventArgs e)
        {
            this.asyncPlayer.Stop();
        }
    }
    internal struct GeneticIndividualUIElement<T>
    {
        public readonly T Item;
        private readonly string toShow;
        public GeneticIndividualUIElement(T a, string s)
        {
            this.Item = a;
            this.toShow = s;
        }
        public override int GetHashCode()
        {
            return this.Item.GetHashCode();
        }
        public override string ToString()
        {
            return this.toShow;
        }
    }
}
