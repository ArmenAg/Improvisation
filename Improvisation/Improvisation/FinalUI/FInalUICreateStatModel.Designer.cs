namespace Improvisation.FinalUI
{
    partial class FInalUICreateStatModel
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
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.midiFileListView = new System.Windows.Forms.ListView();
            this.loadMidiFilesButton = new System.Windows.Forms.Button();
            this.leftRangeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.rightRangeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.beginGenerationButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.leftRangeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightRangeNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // midiFileListView
            // 
            this.midiFileListView.Location = new System.Drawing.Point(12, 67);
            this.midiFileListView.Name = "midiFileListView";
            this.midiFileListView.Size = new System.Drawing.Size(126, 174);
            this.midiFileListView.TabIndex = 4;
            this.midiFileListView.UseCompatibleStateImageBehavior = false;
            // 
            // loadMidiFilesButton
            // 
            this.loadMidiFilesButton.Location = new System.Drawing.Point(12, 12);
            this.loadMidiFilesButton.Name = "loadMidiFilesButton";
            this.loadMidiFilesButton.Size = new System.Drawing.Size(126, 49);
            this.loadMidiFilesButton.TabIndex = 5;
            this.loadMidiFilesButton.Text = "Load Good Midi Files";
            this.loadMidiFilesButton.UseVisualStyleBackColor = true;
            this.loadMidiFilesButton.Click += new System.EventHandler(this.loadMidiFilesButton_Click);
            // 
            // leftRangeNumericUpDown
            // 
            this.leftRangeNumericUpDown.Location = new System.Drawing.Point(12, 247);
            this.leftRangeNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.leftRangeNumericUpDown.Name = "leftRangeNumericUpDown";
            this.leftRangeNumericUpDown.Size = new System.Drawing.Size(56, 20);
            this.leftRangeNumericUpDown.TabIndex = 6;
            this.leftRangeNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.leftRangeNumericUpDown.ValueChanged += new System.EventHandler(this.leftRangeNumericUpDown_ValueChanged);
            // 
            // rightRangeNumericUpDown
            // 
            this.rightRangeNumericUpDown.Location = new System.Drawing.Point(82, 247);
            this.rightRangeNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rightRangeNumericUpDown.Name = "rightRangeNumericUpDown";
            this.rightRangeNumericUpDown.Size = new System.Drawing.Size(56, 20);
            this.rightRangeNumericUpDown.TabIndex = 7;
            this.rightRangeNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rightRangeNumericUpDown.ValueChanged += new System.EventHandler(this.rightRangeNumericUpDown_ValueChanged);
            // 
            // beginGenerationButton
            // 
            this.beginGenerationButton.Location = new System.Drawing.Point(12, 301);
            this.beginGenerationButton.Name = "beginGenerationButton";
            this.beginGenerationButton.Size = new System.Drawing.Size(126, 49);
            this.beginGenerationButton.TabIndex = 8;
            this.beginGenerationButton.Text = "Begin Generation";
            this.beginGenerationButton.UseVisualStyleBackColor = true;
            this.beginGenerationButton.Click += new System.EventHandler(this.beginGenerationButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 278);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(89, 17);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Note by Note";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // FInalUICreateStatModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 362);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.beginGenerationButton);
            this.Controls.Add(this.rightRangeNumericUpDown);
            this.Controls.Add(this.leftRangeNumericUpDown);
            this.Controls.Add(this.loadMidiFilesButton);
            this.Controls.Add(this.midiFileListView);
            this.Name = "FInalUICreateStatModel";
            this.Text = "FInalUICreateStatModel";
            ((System.ComponentModel.ISupportInitialize)(this.leftRangeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightRangeNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView midiFileListView;
        private System.Windows.Forms.Button loadMidiFilesButton;
        private System.Windows.Forms.NumericUpDown leftRangeNumericUpDown;
        private System.Windows.Forms.NumericUpDown rightRangeNumericUpDown;
        private System.Windows.Forms.Button beginGenerationButton;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}