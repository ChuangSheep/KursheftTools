namespace KursheftTools
{
    partial class FormAbout
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
            this.CopyrightNameL = new System.Windows.Forms.Label();
            this.VersionL = new System.Windows.Forms.Label();
            this.LicenseRTB = new System.Windows.Forms.RichTextBox();
            this.ProgrammNameL = new System.Windows.Forms.Label();
            this.DateL = new System.Windows.Forms.Label();
            this.ContactLL = new System.Windows.Forms.LinkLabel();
            this.GithubL = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CopyrightNameL
            // 
            this.CopyrightNameL.AutoSize = true;
            this.CopyrightNameL.Font = new System.Drawing.Font("SimSun", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CopyrightNameL.Location = new System.Drawing.Point(32, 61);
            this.CopyrightNameL.Name = "CopyrightNameL";
            this.CopyrightNameL.Size = new System.Drawing.Size(269, 17);
            this.CopyrightNameL.TabIndex = 0;
            this.CopyrightNameL.Text = "Copyright (c) 2020 Chuyang W.";
            // 
            // VersionL
            // 
            this.VersionL.AutoSize = true;
            this.VersionL.Font = new System.Drawing.Font("SimSun", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VersionL.Location = new System.Drawing.Point(468, 29);
            this.VersionL.Name = "VersionL";
            this.VersionL.Size = new System.Drawing.Size(143, 17);
            this.VersionL.TabIndex = 1;
            this.VersionL.Text = "Version 1.0.3.2";
            // 
            // LicenseRTB
            // 
            this.LicenseRTB.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.LicenseRTB.Location = new System.Drawing.Point(35, 144);
            this.LicenseRTB.Name = "LicenseRTB";
            this.LicenseRTB.ReadOnly = true;
            this.LicenseRTB.Size = new System.Drawing.Size(576, 210);
            this.LicenseRTB.TabIndex = 2;
            this.LicenseRTB.Text = "";
            this.LicenseRTB.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.LicenseRTB_LinkClicked);
            // 
            // ProgrammNameL
            // 
            this.ProgrammNameL.AutoSize = true;
            this.ProgrammNameL.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ProgrammNameL.Location = new System.Drawing.Point(31, 27);
            this.ProgrammNameL.Name = "ProgrammNameL";
            this.ProgrammNameL.Size = new System.Drawing.Size(163, 20);
            this.ProgrammNameL.TabIndex = 4;
            this.ProgrammNameL.Text = "Kursheft Tools";
            // 
            // DateL
            // 
            this.DateL.AutoSize = true;
            this.DateL.Font = new System.Drawing.Font("SimSun", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DateL.Location = new System.Drawing.Point(522, 48);
            this.DateL.Name = "DateL";
            this.DateL.Size = new System.Drawing.Size(89, 17);
            this.DateL.TabIndex = 5;
            this.DateL.Text = "Feb. 2021";
            // 
            // ContactLL
            // 
            this.ContactLL.AutoSize = true;
            this.ContactLL.Location = new System.Drawing.Point(35, 362);
            this.ContactLL.Name = "ContactLL";
            this.ContactLL.Size = new System.Drawing.Size(0, 17);
            this.ContactLL.TabIndex = 6;
            this.ContactLL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ContactLL_LinkClicked);
            // 
            // GithubL
            // 
            this.GithubL.AutoSize = true;
            this.GithubL.Location = new System.Drawing.Point(35, 387);
            this.GithubL.Name = "GithubL";
            this.GithubL.Size = new System.Drawing.Size(0, 17);
            this.GithubL.TabIndex = 7;
            this.GithubL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GithubL_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(32, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Some Rights Reserved.";
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 416);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GithubL);
            this.Controls.Add(this.ContactLL);
            this.Controls.Add(this.DateL);
            this.Controls.Add(this.ProgrammNameL);
            this.Controls.Add(this.LicenseRTB);
            this.Controls.Add(this.VersionL);
            this.Controls.Add(this.CopyrightNameL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CopyrightNameL;
        private System.Windows.Forms.Label VersionL;
        private System.Windows.Forms.RichTextBox LicenseRTB;
        private System.Windows.Forms.Label ProgrammNameL;
        private System.Windows.Forms.Label DateL;
        private System.Windows.Forms.LinkLabel ContactLL;
        private System.Windows.Forms.LinkLabel GithubL;
        private System.Windows.Forms.Label label1;
    }
}