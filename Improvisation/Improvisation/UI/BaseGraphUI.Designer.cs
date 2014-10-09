using System;
namespace Improvisation
{
    partial class BaseGraphUI<T>
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
        public virtual void InitializeComponent()
        {
            this.wpfContainer = new System.Windows.Forms.Integration.ElementHost();
            this.generateButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.serachTextBox = new System.Windows.Forms.TextBox();
            this.depthNumeric = new System.Windows.Forms.NumericUpDown();
            this.NGramListBox = new System.Windows.Forms.ListBox();
            this.sentenceTextBox = new System.Windows.Forms.RichTextBox();

            ((System.ComponentModel.ISupportInitialize)(this.depthNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // wpfContainer
            // 
            this.wpfContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpfContainer.Location = new System.Drawing.Point(0, 0);
            this.wpfContainer.Name = "wpfContainer";
            this.wpfContainer.Size = new System.Drawing.Size(1084, 588);
            this.wpfContainer.TabIndex = 0;
            this.wpfContainer.Text = "elementHost1";
            this.wpfContainer.Child = null;
            // 
            // generateButton
            // 
            this.generateButton.AutoSize = true;
            this.generateButton.Location = new System.Drawing.Point(191, 1);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(111, 41);
            this.generateButton.TabIndex = 1;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.GenerateButtonClick);
            // 
            // reloadButton
            // 
            this.reloadButton.Location = new System.Drawing.Point(325, 1);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(111, 41);
            this.reloadButton.TabIndex = 2;
            this.reloadButton.Text = "Reload";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.ReloadButtonClick);
            // 
            // serachTextBox
            // 
            this.serachTextBox.Location = new System.Drawing.Point(12, 12);
            this.serachTextBox.Name = "serachTextBox";
            this.serachTextBox.Size = new System.Drawing.Size(159, 20);
            this.serachTextBox.TabIndex = 3;
            this.serachTextBox.TextChanged += new System.EventHandler(this.SearchTextChanged);
            // 
            // depthNumeric
            // 
            this.depthNumeric.Location = new System.Drawing.Point(12, 38);
            this.depthNumeric.Name = "depthNumeric";
            this.depthNumeric.Size = new System.Drawing.Size(159, 20);
            this.depthNumeric.TabIndex = 5;
            // 
            // NGramListBox
            // 
            this.NGramListBox.FormattingEnabled = true;
            this.NGramListBox.Location = new System.Drawing.Point(12, 64);
            this.NGramListBox.Name = "NGramListBox";
            this.NGramListBox.Size = new System.Drawing.Size(159, 290);
            this.NGramListBox.TabIndex = 6;
            this.NGramListBox.SelectedIndexChanged += new System.EventHandler(this.NGramListBoxSelectedIndexChanged);
            // 
            // richTextBox1
            // 
            this.sentenceTextBox.Location = new System.Drawing.Point(191, 64);
            this.sentenceTextBox.Name = "richTextBox1";
            this.sentenceTextBox.ReadOnly = true;
            this.sentenceTextBox.Size = new System.Drawing.Size(245, 96);
            this.sentenceTextBox.TabIndex = 7;
            this.sentenceTextBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 588);

            this.Controls.Add(this.sentenceTextBox);
            this.Controls.Add(this.NGramListBox);
            this.Controls.Add(this.depthNumeric);
            this.Controls.Add(this.serachTextBox);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.wpfContainer);
            
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.depthNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Integration.ElementHost wpfContainer { get; private set; }
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button reloadButton;
        public System.Windows.Forms.TextBox serachTextBox { get; private set; }
        private System.Windows.Forms.NumericUpDown depthNumeric;
        private System.Windows.Forms.ListBox NGramListBox;
        public System.Windows.Forms.RichTextBox sentenceTextBox;

    }
}