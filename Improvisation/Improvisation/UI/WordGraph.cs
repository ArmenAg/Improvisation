using GraphX;
using GraphX.Controls;
using GraphX.GraphSharp.Algorithms.Layout.Simple.FDP;
using GraphX.GraphSharp.Algorithms.OverlapRemoval;
using GraphX.Logic;
using Improvisation.Library;
using Improvisation.Library.Data;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Improvisation
{
    public class WordGraph : BaseGraphUI<string>
    {
        public override int WalkerDepth
        {
            get { return 6; }
        }

        public override void FormLoad(object sender, EventArgs e)
        {
            this.DisableButtons();
            this.serachTextBox.ReadOnly = true;
            this.Size = this.Size;
            // List<string> corp = new List<string>() { "a", "b", "c", "a", "e", "f", "g", "h", "e", "f", "h", "b", "b", "c", "h", "g" };
            List<string> corp = TextCorpus.RetrieveAnyCompleteWordCorpus();

            HeterogenousNGrams<string> grams = HeterogenousNGrams<string>.BuildNGrams(1, 3, corp);
            //HomogenousNGrams<string> grams = HomogenousNGrams<string>.BuildNGrams(1, corp);

            //NGramGraphMarkovChain<string> graphHetero = new NGramGraphMarkovChain<string>(heterograms);
            NGramGraphMarkovChain<string> graph = new NGramGraphMarkovChain<string>(grams);

            this.MarkovGraph = graph;

            this.wpfContainer.Child = GenerateWpfVisuals(GraphUIHelper.GenerateGraphUI<string>(NGramGraphMarkovChain<string>.Empty(true)));
            ZoomControl.ZoomToFill();

            this.serachTextBox.ReadOnly = false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public override bool IsHeterogenous
        {
            get { throw new NotImplementedException(); }
        }
    }
}
