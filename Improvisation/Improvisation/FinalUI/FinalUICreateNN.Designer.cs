namespace Improvisation.FinalUI
{
    partial class FinalUICreateNN
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
            if (null != this.threadTrain && this.threadTrain.ThreadState != System.Threading.ThreadState.Aborted)
            {
                this.threadTrain.Abort();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadMidiFilesButton = new System.Windows.Forms.Button();
            this.midiFileListView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.hiddelLayerSize = new System.Windows.Forms.NumericUpDown();
            this.loadOkayButton = new System.Windows.Forms.Button();
            this.okayMidiFilesListView = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.okayWeight = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.trainingButton = new System.Windows.Forms.Button();
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.errotextBox = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.hiddelLayerSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.okayWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // loadMidiFilesButton
            // 
            this.loadMidiFilesButton.Location = new System.Drawing.Point(12, 12);
            this.loadMidiFilesButton.Name = "loadMidiFilesButton";
            this.loadMidiFilesButton.Size = new System.Drawing.Size(126, 49);
            this.loadMidiFilesButton.TabIndex = 0;
            this.loadMidiFilesButton.Text = "Load Good Midi Files";
            this.loadMidiFilesButton.UseVisualStyleBackColor = true;
            this.loadMidiFilesButton.Click += new System.EventHandler(this.loadMidiFilesButton_Click);
            // 
            // midiFileListView
            // 
            this.midiFileListView.Location = new System.Drawing.Point(12, 67);
            this.midiFileListView.Name = "midiFileListView";
            this.midiFileListView.Size = new System.Drawing.Size(126, 174);
            this.midiFileListView.TabIndex = 3;
            this.midiFileListView.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Size of Hidden Layer";
            // 
            // hiddelLayerSize
            // 
            this.hiddelLayerSize.Location = new System.Drawing.Point(18, 284);
            this.hiddelLayerSize.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.hiddelLayerSize.Name = "hiddelLayerSize";
            this.hiddelLayerSize.Size = new System.Drawing.Size(120, 20);
            this.hiddelLayerSize.TabIndex = 0;
            // 
            // loadOkayButton
            // 
            this.loadOkayButton.Location = new System.Drawing.Point(155, 12);
            this.loadOkayButton.Name = "loadOkayButton";
            this.loadOkayButton.Size = new System.Drawing.Size(126, 49);
            this.loadOkayButton.TabIndex = 4;
            this.loadOkayButton.Text = "Load Okay Midi Files";
            this.loadOkayButton.UseVisualStyleBackColor = true;
            this.loadOkayButton.Click += new System.EventHandler(this.loadOkayButton_Click);
            // 
            // okayMidiFilesListView
            // 
            this.okayMidiFilesListView.Location = new System.Drawing.Point(155, 67);
            this.okayMidiFilesListView.Name = "okayMidiFilesListView";
            this.okayMidiFilesListView.Size = new System.Drawing.Size(126, 174);
            this.okayMidiFilesListView.TabIndex = 5;
            this.okayMidiFilesListView.UseCompatibleStateImageBehavior = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(152, 268);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Weight of Okay";
            // 
            // okayWeight
            // 
            this.okayWeight.DecimalPlaces = 2;
            this.okayWeight.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.okayWeight.Location = new System.Drawing.Point(155, 284);
            this.okayWeight.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.okayWeight.Name = "okayWeight";
            this.okayWeight.Size = new System.Drawing.Size(120, 20);
            this.okayWeight.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 331);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Train Until";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 1;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(18, 347);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 8;
            // 
            // trainingButton
            // 
            this.trainingButton.Location = new System.Drawing.Point(505, 12);
            this.trainingButton.Name = "trainingButton";
            this.trainingButton.Size = new System.Drawing.Size(126, 49);
            this.trainingButton.TabIndex = 9;
            this.trainingButton.Text = "Begin Training";
            this.trainingButton.UseVisualStyleBackColor = true;
            this.trainingButton.Click += new System.EventHandler(this.trainingButton_Click);
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.Location = new System.Drawing.Point(156, 348);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(89, 17);
            this.checkBox.TabIndex = 10;
            this.checkBox.Text = "Note by Note";
            this.checkBox.UseVisualStyleBackColor = true;
            // 
            // errotextBox
            // 
            this.errotextBox.Location = new System.Drawing.Point(505, 67);
            this.errotextBox.Name = "errotextBox";
            this.errotextBox.Size = new System.Drawing.Size(126, 20);
            this.errotextBox.TabIndex = 11;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 390);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(619, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // FinalUICreateNN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 438);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.errotextBox);
            this.Controls.Add(this.checkBox);
            this.Controls.Add(this.trainingButton);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.okayWeight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.okayMidiFilesListView);
            this.Controls.Add(this.loadOkayButton);
            this.Controls.Add(this.hiddelLayerSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.midiFileListView);
            this.Controls.Add(this.loadMidiFilesButton);
            this.Name = "FinalUICreateNN";
            this.Text = "FinalUICreateNN";
            ((System.ComponentModel.ISupportInitialize)(this.hiddelLayerSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.okayWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadMidiFilesButton;
        private System.Windows.Forms.ListView midiFileListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown hiddelLayerSize;
        private System.Windows.Forms.Button loadOkayButton;
        private System.Windows.Forms.ListView okayMidiFilesListView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown okayWeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button trainingButton;
        private System.Windows.Forms.CheckBox checkBox;
        private System.Windows.Forms.TextBox errotextBox;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}