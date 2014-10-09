using System;
namespace Improvisation
{
    partial class FinalUIBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            this.asyncPlayer.Dispose();

            if (this.trainingThread != null && this.trainingThread.ThreadState != System.Threading.ThreadState.Aborted)
            {
                this.trainingThread.Abort();
            }
        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinalUIBase));
            this.loadNeuralNetworkTextbox = new System.Windows.Forms.TextBox();
            this.loadNeuralNetworkButton = new System.Windows.Forms.Button();
            this.createNeuralNet = new System.Windows.Forms.ToolStripButton();
            this.helper = new System.Windows.Forms.ToolStrip();
            this.createStatModel = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.geneticInitialPopulation = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.geneticEpochs = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.allowRandomChordSelection = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mergeChordsCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.useWeightAssignerCheckBox = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.sizeVsWeightAssigner = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.countDistAssigner = new System.Windows.Forms.TrackBar();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.startMusicGeneration = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.walkerDepth = new System.Windows.Forms.NumericUpDown();
            this.fromEachNode = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.mutateCrossCheckBox = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.geneticErrorTextBox = new System.Windows.Forms.TextBox();
            this.songListBox = new System.Windows.Forms.ListBox();
            this.graphDifferenceTextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.useSubGraphCheckBox = new System.Windows.Forms.CheckBox();
            this.stopMusic = new System.Windows.Forms.Button();
            this.helper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.geneticInitialPopulation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.geneticEpochs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeVsWeightAssigner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.countDistAssigner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.walkerDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromEachNode)).BeginInit();
            this.SuspendLayout();
            // 
            // loadNeuralNetworkTextbox
            // 
            this.loadNeuralNetworkTextbox.Location = new System.Drawing.Point(12, 28);
            this.loadNeuralNetworkTextbox.Name = "loadNeuralNetworkTextbox";
            this.loadNeuralNetworkTextbox.ReadOnly = true;
            this.loadNeuralNetworkTextbox.Size = new System.Drawing.Size(111, 20);
            this.loadNeuralNetworkTextbox.TabIndex = 0;
            // 
            // loadNeuralNetworkButton
            // 
            this.loadNeuralNetworkButton.Location = new System.Drawing.Point(129, 25);
            this.loadNeuralNetworkButton.Name = "loadNeuralNetworkButton";
            this.loadNeuralNetworkButton.Size = new System.Drawing.Size(103, 23);
            this.loadNeuralNetworkButton.TabIndex = 1;
            this.loadNeuralNetworkButton.Text = "Load Neural Net";
            this.loadNeuralNetworkButton.UseVisualStyleBackColor = true;
            this.loadNeuralNetworkButton.Click += new System.EventHandler(this.loadNeuralNetworkButton_Click);
            // 
            // createNeuralNet
            // 
            this.createNeuralNet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.createNeuralNet.Image = ((System.Drawing.Image)(resources.GetObject("createNeuralNet.Image")));
            this.createNeuralNet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createNeuralNet.Name = "createNeuralNet";
            this.createNeuralNet.Size = new System.Drawing.Size(105, 22);
            this.createNeuralNet.Text = "Create Neural Net";
            this.createNeuralNet.Click += new System.EventHandler(this.createNeuralNet_Click);
            // 
            // helper
            // 
            this.helper.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNeuralNet,
            this.createStatModel});
            this.helper.Location = new System.Drawing.Point(0, 0);
            this.helper.Name = "helper";
            this.helper.Size = new System.Drawing.Size(1294, 25);
            this.helper.TabIndex = 2;
            this.helper.Text = "toolStrip1";
            // 
            // createStatModel
            // 
            this.createStatModel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.createStatModel.Image = ((System.Drawing.Image)(resources.GetObject("createStatModel.Image")));
            this.createStatModel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createStatModel.Name = "createStatModel";
            this.createStatModel.Size = new System.Drawing.Size(105, 22);
            this.createStatModel.Text = "Create Stat Model";
            this.createStatModel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.createStatModel.Click += new System.EventHandler(this.createStatModel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(9, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "Genetic Search Variables";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Starting Population";
            // 
            // geneticInitialPopulation
            // 
            this.geneticInitialPopulation.Location = new System.Drawing.Point(114, 164);
            this.geneticInitialPopulation.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.geneticInitialPopulation.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.geneticInitialPopulation.Name = "geneticInitialPopulation";
            this.geneticInitialPopulation.Size = new System.Drawing.Size(118, 20);
            this.geneticInitialPopulation.TabIndex = 7;
            this.geneticInitialPopulation.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "# of Epoch\'s";
            // 
            // geneticEpochs
            // 
            this.geneticEpochs.Location = new System.Drawing.Point(114, 196);
            this.geneticEpochs.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.geneticEpochs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.geneticEpochs.Name = "geneticEpochs";
            this.geneticEpochs.Size = new System.Drawing.Size(118, 20);
            this.geneticEpochs.TabIndex = 9;
            this.geneticEpochs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Complete Random Chord Selection";
            // 
            // allowRandomChordSelection
            // 
            this.allowRandomChordSelection.AutoSize = true;
            this.allowRandomChordSelection.Checked = true;
            this.allowRandomChordSelection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allowRandomChordSelection.Location = new System.Drawing.Point(217, 230);
            this.allowRandomChordSelection.Name = "allowRandomChordSelection";
            this.allowRandomChordSelection.Size = new System.Drawing.Size(15, 14);
            this.allowRandomChordSelection.TabIndex = 11;
            this.allowRandomChordSelection.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 261);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Merge Chords During Cross";
            // 
            // mergeChordsCheckBox
            // 
            this.mergeChordsCheckBox.AutoSize = true;
            this.mergeChordsCheckBox.Checked = true;
            this.mergeChordsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mergeChordsCheckBox.Location = new System.Drawing.Point(217, 261);
            this.mergeChordsCheckBox.Name = "mergeChordsCheckBox";
            this.mergeChordsCheckBox.Size = new System.Drawing.Size(15, 14);
            this.mergeChordsCheckBox.TabIndex = 13;
            this.mergeChordsCheckBox.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label6.Location = new System.Drawing.Point(9, 323);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 24);
            this.label6.TabIndex = 14;
            this.label6.Text = "Weight Assigners";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 366);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Use weight assigner";
            // 
            // useWeightAssignerCheckBox
            // 
            this.useWeightAssignerCheckBox.AutoSize = true;
            this.useWeightAssignerCheckBox.Checked = true;
            this.useWeightAssignerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useWeightAssignerCheckBox.Location = new System.Drawing.Point(217, 366);
            this.useWeightAssignerCheckBox.Name = "useWeightAssignerCheckBox";
            this.useWeightAssignerCheckBox.Size = new System.Drawing.Size(15, 14);
            this.useWeightAssignerCheckBox.TabIndex = 16;
            this.useWeightAssignerCheckBox.UseVisualStyleBackColor = true;
            this.useWeightAssignerCheckBox.CheckedChanged += new System.EventHandler(this.useWeightAssignerCheckBox_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 398);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Size vs Weight";
            // 
            // sizeVsWeightAssigner
            // 
            this.sizeVsWeightAssigner.Location = new System.Drawing.Point(114, 398);
            this.sizeVsWeightAssigner.Maximum = 100;
            this.sizeVsWeightAssigner.Name = "sizeVsWeightAssigner";
            this.sizeVsWeightAssigner.Size = new System.Drawing.Size(118, 45);
            this.sizeVsWeightAssigner.TabIndex = 18;
            this.sizeVsWeightAssigner.Value = 50;
            this.sizeVsWeightAssigner.Scroll += new System.EventHandler(this.sizeVsWeightAssigner_Scroll);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 447);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Count Distrubution";
            // 
            // countDistAssigner
            // 
            this.countDistAssigner.Location = new System.Drawing.Point(114, 449);
            this.countDistAssigner.Maximum = 100;
            this.countDistAssigner.Name = "countDistAssigner";
            this.countDistAssigner.Size = new System.Drawing.Size(118, 45);
            this.countDistAssigner.TabIndex = 20;
            this.countDistAssigner.Value = 50;
            this.countDistAssigner.Scroll += new System.EventHandler(this.countDistAssigner_Scroll);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 593);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1168, 23);
            this.progressBar1.TabIndex = 21;
            // 
            // startMusicGeneration
            // 
            this.startMusicGeneration.Location = new System.Drawing.Point(8, 54);
            this.startMusicGeneration.Name = "startMusicGeneration";
            this.startMusicGeneration.Size = new System.Drawing.Size(224, 39);
            this.startMusicGeneration.TabIndex = 22;
            this.startMusicGeneration.Text = "Start Music Generation";
            this.startMusicGeneration.UseVisualStyleBackColor = true;
            this.startMusicGeneration.Click += new System.EventHandler(this.startMusicGeneration_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label10.Location = new System.Drawing.Point(4, 497);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(134, 24);
            this.label10.TabIndex = 23;
            this.label10.Text = "Graph Walking";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 533);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Walker depth";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 560);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Per each node";
            // 
            // walkerDepth
            // 
            this.walkerDepth.Location = new System.Drawing.Point(114, 533);
            this.walkerDepth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.walkerDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.walkerDepth.Name = "walkerDepth";
            this.walkerDepth.Size = new System.Drawing.Size(118, 20);
            this.walkerDepth.TabIndex = 26;
            this.walkerDepth.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // fromEachNode
            // 
            this.fromEachNode.Location = new System.Drawing.Point(114, 560);
            this.fromEachNode.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.fromEachNode.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.fromEachNode.Name = "fromEachNode";
            this.fromEachNode.Size = new System.Drawing.Size(118, 20);
            this.fromEachNode.TabIndex = 27;
            this.fromEachNode.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 294);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Mutate Cross";
            // 
            // mutateCrossCheckBox
            // 
            this.mutateCrossCheckBox.AutoSize = true;
            this.mutateCrossCheckBox.Checked = true;
            this.mutateCrossCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mutateCrossCheckBox.Location = new System.Drawing.Point(217, 293);
            this.mutateCrossCheckBox.Name = "mutateCrossCheckBox";
            this.mutateCrossCheckBox.Size = new System.Drawing.Size(15, 14);
            this.mutateCrossCheckBox.TabIndex = 29;
            this.mutateCrossCheckBox.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(1202, 580);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 13);
            this.label14.TabIndex = 30;
            this.label14.Text = "Genetic Fitness";
            // 
            // geneticErrorTextBox
            // 
            this.geneticErrorTextBox.Location = new System.Drawing.Point(1181, 596);
            this.geneticErrorTextBox.Name = "geneticErrorTextBox";
            this.geneticErrorTextBox.ReadOnly = true;
            this.geneticErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.geneticErrorTextBox.TabIndex = 31;
            // 
            // songListBox
            // 
            this.songListBox.FormattingEnabled = true;
            this.songListBox.Location = new System.Drawing.Point(1058, 64);
            this.songListBox.Name = "songListBox";
            this.songListBox.Size = new System.Drawing.Size(224, 511);
            this.songListBox.TabIndex = 32;
            this.songListBox.SelectedIndexChanged += new System.EventHandler(this.songListBox_SelectedIndexChanged);
            // 
            // graphDifferenceTextBox
            // 
            this.graphDifferenceTextBox.Location = new System.Drawing.Point(1058, 38);
            this.graphDifferenceTextBox.Name = "graphDifferenceTextBox";
            this.graphDifferenceTextBox.ReadOnly = true;
            this.graphDifferenceTextBox.Size = new System.Drawing.Size(111, 20);
            this.graphDifferenceTextBox.TabIndex = 33;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1055, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(115, 13);
            this.label15.TabIndex = 34;
            this.label15.Text = "Difference From Songs";
            // 
            // useSubGraphCheckBox
            // 
            this.useSubGraphCheckBox.AutoSize = true;
            this.useSubGraphCheckBox.Checked = true;
            this.useSubGraphCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useSubGraphCheckBox.Location = new System.Drawing.Point(1176, 18);
            this.useSubGraphCheckBox.Name = "useSubGraphCheckBox";
            this.useSubGraphCheckBox.Size = new System.Drawing.Size(96, 17);
            this.useSubGraphCheckBox.TabIndex = 35;
            this.useSubGraphCheckBox.Text = "Use SubGraph";
            this.useSubGraphCheckBox.UseVisualStyleBackColor = true;
            // 
            // stopMusic
            // 
            this.stopMusic.Location = new System.Drawing.Point(1181, 35);
            this.stopMusic.Name = "stopMusic";
            this.stopMusic.Size = new System.Drawing.Size(91, 23);
            this.stopMusic.TabIndex = 36;
            this.stopMusic.Text = "Stop Music";
            this.stopMusic.UseVisualStyleBackColor = true;
            this.stopMusic.Click += new System.EventHandler(this.stopMusic_Click);
            // 
            // FinalUIBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 628);
            this.Controls.Add(this.stopMusic);
            this.Controls.Add(this.useSubGraphCheckBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.graphDifferenceTextBox);
            this.Controls.Add(this.songListBox);
            this.Controls.Add(this.geneticErrorTextBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.mutateCrossCheckBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.fromEachNode);
            this.Controls.Add(this.walkerDepth);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.startMusicGeneration);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.countDistAssigner);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.sizeVsWeightAssigner);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.useWeightAssignerCheckBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.mergeChordsCheckBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.allowRandomChordSelection);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.geneticEpochs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.geneticInitialPopulation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.helper);
            this.Controls.Add(this.loadNeuralNetworkButton);
            this.Controls.Add(this.loadNeuralNetworkTextbox);
            this.Name = "FinalUIBase";
            this.Text = "FinalUI";
            this.helper.ResumeLayout(false);
            this.helper.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.geneticInitialPopulation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.geneticEpochs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeVsWeightAssigner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.countDistAssigner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.walkerDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromEachNode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.TextBox loadNeuralNetworkTextbox;
        private System.Windows.Forms.Button loadNeuralNetworkButton;
        private System.Windows.Forms.ToolStripButton createNeuralNet;
        private System.Windows.Forms.ToolStrip helper;
        private System.Windows.Forms.ToolStripButton createStatModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown geneticInitialPopulation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown geneticEpochs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox allowRandomChordSelection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox mergeChordsCheckBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox useWeightAssignerCheckBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar sizeVsWeightAssigner;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar countDistAssigner;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button startMusicGeneration;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown walkerDepth;
        private System.Windows.Forms.NumericUpDown fromEachNode;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox mutateCrossCheckBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox geneticErrorTextBox;
        private System.Windows.Forms.ListBox songListBox;
        private System.Windows.Forms.TextBox graphDifferenceTextBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox useSubGraphCheckBox;
        private System.Windows.Forms.Button stopMusic;

    }
}