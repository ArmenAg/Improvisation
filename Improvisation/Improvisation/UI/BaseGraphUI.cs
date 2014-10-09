using GraphX.Controls;
using GraphX.GraphSharp.Algorithms.Layout.Simple.FDP;
using GraphX.GraphSharp.Algorithms.OverlapRemoval;
using GraphX.Logic;
using Improvisation.Library;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Improvisation
{
    public abstract partial class BaseGraphUI<T>
        : Form
        where T : IEquatable<T>
    {

        public struct NGramProbabilityPair<T>
            where T : IEquatable<T>
        {
            public readonly NGram<T> Node;
            public readonly double Probability;

            public NGramProbabilityPair(NGram<T> node, double prob)
            {
                this.Node = node;
                this.Probability = prob;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in this.Node)
                {
                    builder.Append(item.ToString());
                    builder.Append(" ");
                }
                builder.Append(": ");
                builder.Append(this.Probability);

                return builder.ToString();
            }
        }

        public NGramGraphMarkovChain<T> MarkovGraph { get; set; }
        public NGramGraphMarkovChain<T> CurrentMarkovGraph { get; private set; }
        public INGrams<T> PathOfRandomWalk { get; set; }
        public int Depth
        {
            get
            {
                return (int)this.depthNumeric.Value;
            }
        }
        public abstract int WalkerDepth { get; }
        public abstract bool IsHeterogenous { get; }

        public ZoomControl ZoomControl { get; private set; }
        public GraphAreaExample GraphArea { get; private set; }


        public BaseGraphUI()
        {
            InitializeComponent();
        }

        public abstract void FormLoad(object sender, System.EventArgs e);

        private NGram<T> CreateNGRamFromSearchTextBox()
        {
            var query = this.MarkovGraph.Where(x => x.Key.ToString() == this.serachTextBox.Text);
            if (null != query && query.Any())
            {
                return query.First().Key;
            }
            else
            {
                return NGram<T>.Empty;
            }
        }

        public virtual void GenerateButtonClick(object sender, System.EventArgs e)
        {
            this.NGramListBox.Enabled = true;

            var dataVertex = new DataVertex();
            if (this.serachTextBox.Text != string.Empty)
            {
                NGram<T> s = this.CreateNGRamFromSearchTextBox();
                if (s != NGram<T>.Empty)
                {
                    this.CurrentMarkovGraph = this.MarkovGraph.GetSubGraphFromNGram(s, this.Depth);
                    if (this.MarkovGraph.ValidNode(s))
                    {
                        this.wpfContainer.Child = this.GenerateWpfVisuals(GraphUIHelper.GenerateGraphUI(this.CurrentMarkovGraph));
                    }
                }
            }

            GraphArea.GenerateGraph(true);
            GraphArea.SetVerticesDrag(true, true);
            ZoomControl.ZoomToFill();

            this.UpdateListBox();

        }

        public virtual void SearchTextChanged(object sender, System.EventArgs e)
        {
            if (this.serachTextBox.Text != string.Empty && this.serachTextBox.Text.Where(x => x != ' ').Any())
            {
                if (this.MarkovGraph.AsParallel().Any(x => x.Key.ToString() == this.serachTextBox.Text))
                {
                    this.EnableButtons();
                }
                else
                {
                    this.DisableButtons();
                }
            }
            else
            {
                this.DisableButtons();
            }
        }

        public virtual void ReloadButtonClick(object sender, System.EventArgs e)
        {
            GraphArea.RelayoutGraph();
        }

        public virtual void NGramListBoxSelectedIndexChanged(object sender, System.EventArgs e)
        {
            var item = GetStartingNode();

            var chain = this.UpdateSentenceTextBox();
            this.PathOfRandomWalk = chain;
            this.UpdateGraph(new NGramGraphMarkovChain<T>(this.PathOfRandomWalk));
        }

        public virtual NGram<T>[] RetrieveChain()
        {
            NGramRandomGraphMarkovChainWalker<T> walker = new NGramRandomGraphMarkovChainWalker<T>(this.MarkovGraph, this.GetStartingNode().Node, new Random());
            return walker.WalkMarkovChain(this.WalkerDepth, NGramRandomGraphMarkovChainWalker<T>.NextStateType.PossibleHeterogenous);
        }

        public NGramProbabilityPair<T> GetStartingNode()
        {
            return (NGramProbabilityPair<T>)this.NGramListBox.SelectedItem;
        }

        public UIElement GenerateWpfVisuals(GraphExample example)
        {
            ZoomControl = new ZoomControl();
            ZoomControl.SetViewFinderVisibility(ZoomControl, System.Windows.Visibility.Visible);
            /* ENABLES WINFORMS HOSTING MODE --- >*/
            var logic = new GXLogicCore<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>();
            GraphArea = new GraphAreaExample() { EnableWinFormsHostingMode = true, LogicCore = logic };
            logic.Graph = example;
            logic.DefaultLayoutAlgorithm = GraphX.LayoutAlgorithmTypeEnum.KK;
            logic.DefaultLayoutAlgorithmParams = logic.AlgorithmFactory.CreateLayoutParameters(GraphX.LayoutAlgorithmTypeEnum.KK);
            ((KKLayoutParameters)logic.DefaultLayoutAlgorithmParams).MaxIterations = 100;
            logic.DefaultOverlapRemovalAlgorithm = GraphX.OverlapRemovalAlgorithmTypeEnum.FSA;
            logic.DefaultOverlapRemovalAlgorithmParams = logic.AlgorithmFactory.CreateOverlapRemovalParameters(GraphX.OverlapRemovalAlgorithmTypeEnum.FSA);
            ((OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams).HorizontalGap = 50;
            ((OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams).VerticalGap = 50;
            logic.DefaultEdgeRoutingAlgorithm = GraphX.EdgeRoutingAlgorithmTypeEnum.None;
            logic.AsyncAlgorithmCompute = false;
            ZoomControl.Content = GraphArea;
            GraphArea.RelayoutFinished += GAreaRelayoutFinished;



            var myResourceDictionary = new ResourceDictionary { Source = new Uri("Templates\\template.xaml", UriKind.Relative) };
            ZoomControl.Resources.MergedDictionaries.Add(myResourceDictionary);


            return ZoomControl;
        }

        public void GAreaRelayoutFinished(object sender, EventArgs e)
        {
            this.ZoomControl.ZoomToFill();
        }

        public void EnableButtons()
        {
            this.generateButton.BeginInvoke(new Action(() => { this.generateButton.Enabled = true; }));
            this.reloadButton.BeginInvoke(new Action(() => { this.reloadButton.Enabled = true; }));
        }
        public void DisableButtons()
        {
            this.generateButton.BeginInvoke(new Action(() => { this.generateButton.Enabled = false; }));
            this.reloadButton.BeginInvoke(new Action(() => { this.reloadButton.Enabled = false; }));
        }

        private void UpdateListBox()
        {
            this.NGramListBox.Items.Clear();


            if (this.CurrentMarkovGraph != null)
            {
                var ngrams = this.CurrentMarkovGraph.Select(x => x.Key);
                var distrubution = new NGramDistribution<T>(ngrams);

                var data = new HashSet<NGramProbabilityPair<T>>(from item in this.CurrentMarkovGraph.AsParallel()
                                                                select (new NGramProbabilityPair<T>(item.Key, this.CurrentMarkovGraph.Distrubution.RetrieveProbability(item.Key))))
                                                                .OrderBy(x => x.Probability);
                //FIX::
                this.NGramListBox.Items.AddRange(data.Select(x => (object)x).ToArray());
            }
        }

        private void UpdateGraph()
        {
            this.wpfContainer.Child = GenerateWpfVisuals(GraphUIHelper.GenerateGraphUI<T>(this.CurrentMarkovGraph));

            GraphArea.GenerateGraph(true);
            GraphArea.SetVerticesDrag(true, true);
            ZoomControl.ZoomToFill();
        }
        private void UpdateGraph(NGramGraphMarkovChain<T> chain)
        {
            this.wpfContainer.Child = GenerateWpfVisuals(GraphUIHelper.GenerateGraphUI<T>(chain));

            GraphArea.GenerateGraph(true);
            GraphArea.SetVerticesDrag(true, true);
            ZoomControl.ZoomToFill();
        }
        public INGrams<T> UpdateSentenceTextBox()
        {
            var chain = RetrieveChain();

            this.sentenceTextBox.Text = NGramHelper.ShowNGram(chain);

            return HomogenousNGrams<T>.DirectBuiltUnsafe(chain, chain.First().N);
        }


    }
}
