namespace KursheftTools
{
    partial class FormProgress
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
            this.components = new System.ComponentModel.Container();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.detailProgress = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ProgressL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(27, 230);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(487, 23);
            this.ProgressBar.TabIndex = 0;
            // 
            // detailProgress
            // 
            this.detailProgress.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.detailProgress.Location = new System.Drawing.Point(27, 26);
            this.detailProgress.Multiline = true;
            this.detailProgress.Name = "detailProgress";
            this.detailProgress.ReadOnly = true;
            this.detailProgress.Size = new System.Drawing.Size(487, 182);
            this.detailProgress.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ProgressL
            // 
            this.ProgressL.AutoSize = true;
            this.ProgressL.Location = new System.Drawing.Point(27, 266);
            this.ProgressL.Name = "ProgressL";
            this.ProgressL.Size = new System.Drawing.Size(0, 15);
            this.ProgressL.TabIndex = 2;
            // 
            // FormProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 293);
            this.Controls.Add(this.ProgressL);
            this.Controls.Add(this.detailProgress);
            this.Controls.Add(this.ProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FormProgress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Progress";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProgress_FormClosing);
            this.Load += new System.EventHandler(this.FormProgress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.TextBox detailProgress;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label ProgressL;
    }
}