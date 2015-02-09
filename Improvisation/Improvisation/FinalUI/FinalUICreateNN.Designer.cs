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
            this.chkNoteByNote = new System.Windows.Forms.CheckBox();
            this.errotextBox = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.chkRealtime = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.hiddelLayerSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.okayWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadMidiFilesButton
            // 
            this.loadMidiFilesButton.Location = new System.Drawing.Point(17, 19);
            this.loadMidiFilesButton.Name = "loadMidiFilesButton";
            this.loadMidiFilesButton.Size = new System.Drawing.Size(126, 49);
            this.loadMidiFilesButton.TabIndex = 0;
            this.loadMidiFilesButton.Text = "Load Good Midi Files";
            this.loadMidiFilesButton.UseVisualStyleBackColor = true;
            this.loadMidiFilesButton.Click += new System.EventHandler(this.loadMidiFilesButton_Click);
            // 
            // midiFileListView
            // 
            this.midiFileListView.Location = new System.Drawing.Point(17, 74);
            this.midiFileListView.Name = "midiFileListView";
            this.midiFileListView.Size = new System.Drawing.Size(126, 174);
            this.midiFileListView.TabIndex = 3;
            this.midiFileListView.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 275);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Size of Hidden Layer";
            // 
            // hiddelLayerSize
            // 
            this.hiddelLayerSize.Location = new System.Drawing.Point(23, 291);
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
            this.loadOkayButton.Location = new System.Drawing.Point(160, 19);
            this.loadOkayButton.Name = "loadOkayButton";
            this.loadOkayButton.Size = new System.Drawing.Size(126, 49);
            this.loadOkayButton.TabIndex = 4;
            this.loadOkayButton.Text = "Load Okay Midi Files";
            this.loadOkayButton.UseVisualStyleBackColor = true;
            this.loadOkayButton.Click += new System.EventHandler(this.loadOkayButton_Click);
            // 
            // okayMidiFilesListView
            // 
            this.okayMidiFilesListView.Location = new System.Drawing.Point(160, 74);
            this.okayMidiFilesListView.Name = "okayMidiFilesListView";
            this.okayMidiFilesListView.Size = new System.Drawing.Size(126, 174);
            this.okayMidiFilesListView.TabIndex = 5;
            this.okayMidiFilesListView.UseCompatibleStateImageBehavior = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(157, 275);
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
            this.okayWeight.Location = new System.Drawing.Point(160, 291);
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
            this.label3.Location = new System.Drawing.Point(20, 338);
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
            this.numericUpDown1.Location = new System.Drawing.Point(23, 354);
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
            this.trainingButton.Location = new System.Drawing.Point(15, 19);
            this.trainingButton.Name = "trainingButton";
            this.trainingButton.Size = new System.Drawing.Size(126, 49);
            this.trainingButton.TabIndex = 9;
            this.trainingButton.Text = "Begin Training";
            this.trainingButton.UseVisualStyleBackColor = true;
            this.trainingButton.Click += new System.EventHandler(this.trainingButton_Click);
            // 
            // chkNoteByNote
            // 
            this.chkNoteByNote.AutoSize = true;
            this.chkNoteByNote.Location = new System.Drawing.Point(161, 355);
            this.chkNoteByNote.Name = "chkNoteByNote";
            this.chkNoteByNote.Size = new System.Drawing.Size(89, 17);
            this.chkNoteByNote.TabIndex = 10;
            this.chkNoteByNote.Text = "Note by Note";
            this.chkNoteByNote.UseVisualStyleBackColor = true;
            // 
            // errotextBox
            // 
            this.errotextBox.Location = new System.Drawing.Point(15, 74);
            this.errotextBox.Name = "errotextBox";
            this.errotextBox.Size = new System.Drawing.Size(126, 20);
            this.errotextBox.TabIndex = 11;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(17, 19);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(439, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // chkRealtime
            // 
            this.chkRealtime.AutoSize = true;
            this.chkRealtime.Location = new System.Drawing.Point(161, 334);
            this.chkRealtime.Name = "chkRealtime";
            this.chkRealtime.Size = new System.Drawing.Size(140, 17);
            this.chkRealtime.TabIndex = 13;
            this.chkRealtime.Text = "Run with realtime priority";
            this.chkRealtime.UseVisualStyleBackColor = true;
            this.chkRealtime.CheckedChanged += new System.EventHandler(this.realtimeWarning);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkRealtime);
            this.groupBox1.Controls.Add(this.loadMidiFilesButton);
            this.groupBox1.Controls.Add(this.midiFileListView);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkNoteByNote);
            this.groupBox1.Controls.Add(this.hiddelLayerSize);
            this.groupBox1.Controls.Add(this.loadOkayButton);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.okayMidiFilesListView);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.okayWeight);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 389);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Location = new System.Drawing.Point(12, 408);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(470, 58);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Progress";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.trainingButton);
            this.groupBox3.Controls.Add(this.errotextBox);
            this.groupBox3.Location = new System.Drawing.Point(327, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(155, 107);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            // 
            // FinalUICreateNN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 474);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Name = "FinalUICreateNN";
            this.Text = "FinalUICreateNN";
            ((System.ComponentModel.ISupportInitialize)(this.hiddelLayerSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.okayWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.CheckBox chkNoteByNote;
        private System.Windows.Forms.TextBox errotextBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox chkRealtime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}