namespace tapsandbox
{
    partial class Form1
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
            this.ReadBox = new System.Windows.Forms.TextBox();
            this.TapWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // ReadBox
            // 
            this.ReadBox.Location = new System.Drawing.Point(11, 12);
            this.ReadBox.Multiline = true;
            this.ReadBox.Name = "ReadBox";
            this.ReadBox.Size = new System.Drawing.Size(839, 496);
            this.ReadBox.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 521);
            this.Controls.Add(this.ReadBox);
            this.Name = "Form1";
            this.Text = "TAP FUCKER";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ReadBox;
        private System.ComponentModel.BackgroundWorker TapWorker;
    }
}

