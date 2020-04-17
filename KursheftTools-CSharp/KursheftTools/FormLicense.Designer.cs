namespace KursheftTools
{
    partial class FormLicense
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
            this.licenseTB = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // licenseTB
            // 
            this.licenseTB.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.licenseTB.Location = new System.Drawing.Point(12, 12);
            this.licenseTB.Name = "licenseTB";
            this.licenseTB.ReadOnly = true;
            this.licenseTB.Size = new System.Drawing.Size(637, 341);
            this.licenseTB.TabIndex = 0;
            this.licenseTB.Text = "";
            this.licenseTB.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.licenseTB_LinkClicked);
            // 
            // FormLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 365);
            this.Controls.Add(this.licenseTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLicense";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "License";
            this.Load += new System.EventHandler(this.FormLicenses_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox licenseTB;
    }
}