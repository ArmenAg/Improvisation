using Improvisation.Library;
using Improvisation.Library.Music;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Improvisation.FinalUI
{
    public partial class FInalUICreateStatModel : Form
    {
        private string[] files;
        private bool homogeneous { get { return this.leftRangeNumericUpDown.Value == this.rightRangeNumericUpDown.Value; } }
        public FInalUICreateStatModel()
        {
            InitializeComponent();

            this.beginGenerationButton.Enabled = false;
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
                this.beginGenerationButton.Enabled = true;
            }
        }

        private void beginGenerationButton_Click(object sender, EventArgs e)
        {
            try
            {
                PianoNoteRetriever retriever = new PianoNoteRetriever();
                var midiEvents = new InstrumentMidiEventProducer(this.files.Select(x => new Sequence(x)));
                var midi = midiEvents.GetOrderedMessages(GeneralMidiInstrument.AcousticGrandPiano);
                Chord.AllowForComplexSimplification = this.checkBox1.Checked;
                var accords = Chord.RetrieveChords(midi, retriever);

                INGrams<Chord> grams = null;
                if (this.homogeneous)
                {
                    grams = HomogenousNGrams<Chord>.BuildNGrams((int)this.leftRangeNumericUpDown.Value, accords);
                }
                else
                {
                    grams = HeterogenousNGrams<Chord>.BuildNGrams((int)this.leftRangeNumericUpDown.Value, (int)this.rightRangeNumericUpDown.Value, accords);
                }
                NGramGraphMarkovChain<Chord> graph = new NGramGraphMarkovChain<Chord>(grams);
                this.Save(graph);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void leftRangeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (this.rightRangeNumericUpDown.Value < this.leftRangeNumericUpDown.Value)
            {
                MessageBox.Show("left must be less or equal to right");
                this.leftRangeNumericUpDown.Value = this.rightRangeNumericUpDown.Value;
            }
        }

        private void rightRangeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (this.rightRangeNumericUpDown.Value < this.leftRangeNumericUpDown.Value)
            {
                MessageBox.Show("left must be less or equal to right");
                this.leftRangeNumericUpDown.Value = this.rightRangeNumericUpDown.Value;
            }
        }


        private void Save(NGramGraphMarkovChain<Chord> a)
        {
            TemperaryVariables.Graph = a;
            this.Dispose();

            return;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "model files (*.smf)|*.smf";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.CreatePrompt = true;
            saveFileDialog1.ShowDialog();

            //a.Save(saveFileDialog1.FileName);


        }
    }
}
