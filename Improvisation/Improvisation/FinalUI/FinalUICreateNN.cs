using Improvisation.Library.Music;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Improvisation.FinalUI
{
    public partial class FinalUICreateNN : Form
    {
        private string[] files;
        private string[] okayFiles;

        public FinalUICreateNN()
        {
            InitializeComponent();

            this.hiddelLayerSize.Value = 23M;
            this.okayWeight.Value = 0.5M;
            this.numericUpDown1.Value = 10;

            this.trainingButton.Enabled = false;
        }

        private void loadMidiFilesButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Midi Files (*.mid)|*.mid";
            openFileDialog1.Multiselect = true;

            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.files = openFileDialog1.FileNames;

                this.midiFileListView.Items.AddRange(this.files.Select(x => new ListViewItem(FinalUIHelperMethods.FileFriendlyString(x))).ToArray());
            }
            this.trainingButton.Enabled = true;
            this.errotextBox.ReadOnly = true;
        }

        private void loadOkayButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Midi Files (*.mid)|*.mid";
            openFileDialog1.Multiselect = true;

            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.okayFiles = openFileDialog1.FileNames;

                try
                {
                    this.okayMidiFilesListView.Items.AddRange(this.files.Select(x => new ListViewItem(FinalUIHelperMethods.FileFriendlyString(x))).ToArray());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could Not Load Midi Files", ex.Message);
                }
            }
        }

        private DiscreteNeuralNetworkByChord nnByChord;

        private void trainingButton_Click(object sender, EventArgs e)
        {
            PianoNoteRetriever retriever = new PianoNoteRetriever();
            var midiEvents = new InstrumentMidiEventProducer(this.files.Select(x => new Sequence(x)));
            var midi = midiEvents.GetOrderedMessages(GeneralMidiInstrument.AcousticGrandPiano);
            Chord.AllowForComplexSimplification = this.checkBox.Checked;
            var accords = Chord.RetrieveChords(midi, retriever);

            DiscreteDataRetriever data = new DiscreteDataRetriever(accords.ToList());
            DiscreteNeuralNetworkByChord.OkayWeight = (double)this.okayWeight.Value;
            DiscreteNeuralNetworkByChord.HiddenLayerSize = (int)hiddelLayerSize.Value;

            if (this.okayFiles != null)
            {
                var midiEvents1 = new InstrumentMidiEventProducer(this.okayFiles.Select(x => new Sequence(x)));
                var midi1 = midiEvents1.GetOrderedMessages(GeneralMidiInstrument.AcousticGrandPiano);
                Chord.AllowForComplexSimplification = this.checkBox.Checked;
                var accords1 = Chord.RetrieveChords(midi1, retriever);
                DiscreteDataRetriever data1 = new DiscreteDataRetriever(accords1.ToList());

                this.nnByChord = new DiscreteNeuralNetworkByChord(data.Good, data1.Good, data.Bad.Union(data1.Bad).Take(data.Bad.Count).ToList(),
                    new AForge.Neuro.BipolarSigmoidFunction());
            }
            else
            {
                this.nnByChord = new DiscreteNeuralNetworkByChord(data.Good, data.Okay, data.Bad, new AForge.Neuro.SigmoidFunction());
            }

            foreach (var item in this.Controls.OfType<Control>())
            {
                item.Enabled = false;
            }
            threadTrain = new Thread(Start);
            threadTrain.Start();
        }

        private void Start(object obj)
        {
            //stabilizing
            for (int i = 0; i < 10; i++)
            {
                this.nnByChord.Train();
            }
            double error = this.nnByChord.Train();
            this.progressBar1.BeginInvoke((Action)(() => this.progressBar1.Maximum = (int)(error * 10D * 5D)));
            this.progressBar1.BeginInvoke((Action)(() => this.progressBar1.Minimum = (int)((double)this.numericUpDown1.Value * 10D)));
            do
            {
                error = this.nnByChord.Train();

                this.progressBar1.BeginInvoke((Action)(() => this.progressBar1.Value = this.progressBar1.Maximum - (int)(error * 10D)));
                this.errotextBox.BeginInvoke((Action)(() => this.errotextBox.Text = error.ToString()));
            } while (error > (double)this.numericUpDown1.Value);

            this.progressBar1.BeginInvoke((Action)(() => this.progressBar1.Value = this.progressBar1.Maximum));
            this.BeginInvoke((Action)(() => this.SaveAndExit()));
        }
        private void SaveAndExit()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Neural Net files (*.nn)|*.nn";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.CreatePrompt = true;
            saveFileDialog1.ShowDialog();

            if (this.nnByChord.Save(saveFileDialog1.FileName))
            {
                this.BeginInvoke((Action)(() => this.Dispose()));
            }
        }

        private Thread threadTrain;
    }
}
