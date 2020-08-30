namespace KursheftTools
{
    partial class FormForGeneratingNoteBoard
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
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.StartPeriod1L = new System.Windows.Forms.Label();
            this.StartPeriod2L = new System.Windows.Forms.Label();
            this.EndHYL = new System.Windows.Forms.Label();
            this.EndPeriod1L = new System.Windows.Forms.Label();
            this.Holiday1L = new System.Windows.Forms.Label();
            this.Holiday2L = new System.Windows.Forms.Label();
            this.Holiday2 = new KursheftTools.WatermarkedTextBox();
            this.Holiday1 = new KursheftTools.WatermarkedTextBox();
            this.EndPeriod1 = new KursheftTools.WatermarkedTextBox();
            this.EndHY = new KursheftTools.WatermarkedTextBox();
            this.StartPeriod2 = new KursheftTools.WatermarkedTextBox();
            this.StartPeriod1 = new KursheftTools.WatermarkedTextBox();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(267, 367);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(96, 30);
            this.btnAccept.TabIndex = 6;
            this.btnAccept.Text = "&Fertig";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(397, 367);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Aufheben";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // StartPeriod1L
            // 
            this.StartPeriod1L.AutoSize = true;
            this.StartPeriod1L.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartPeriod1L.Location = new System.Drawing.Point(33, 50);
            this.StartPeriod1L.Name = "StartPeriod1L";
            this.StartPeriod1L.Size = new System.Drawing.Size(180, 24);
            this.StartPeriod1L.TabIndex = 2;
            this.StartPeriod1L.Text = "Anfang d. 1. Abschnitts";
            // 
            // StartPeriod2L
            // 
            this.StartPeriod2L.AutoSize = true;
            this.StartPeriod2L.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartPeriod2L.Location = new System.Drawing.Point(33, 151);
            this.StartPeriod2L.Name = "StartPeriod2L";
            this.StartPeriod2L.Size = new System.Drawing.Size(185, 24);
            this.StartPeriod2L.TabIndex = 3;
            this.StartPeriod2L.Text = "Anfang d. 2. Abschnitts ";
            // 
            // EndHYL
            // 
            this.EndHYL.AutoSize = true;
            this.EndHYL.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndHYL.Location = new System.Drawing.Point(33, 201);
            this.EndHYL.Name = "EndHYL";
            this.EndHYL.Size = new System.Drawing.Size(165, 24);
            this.EndHYL.TabIndex = 4;
            this.EndHYL.Text = "Ende des Halbjahres ";
            // 
            // EndPeriod1L
            // 
            this.EndPeriod1L.AutoSize = true;
            this.EndPeriod1L.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndPeriod1L.Location = new System.Drawing.Point(33, 101);
            this.EndPeriod1L.Name = "EndPeriod1L";
            this.EndPeriod1L.Size = new System.Drawing.Size(166, 24);
            this.EndPeriod1L.TabIndex = 11;
            this.EndPeriod1L.Text = "Ende d. 1. Abschnitts";
            // 
            // Holiday1L
            // 
            this.Holiday1L.AutoSize = true;
            this.Holiday1L.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Holiday1L.Location = new System.Drawing.Point(33, 251);
            this.Holiday1L.Name = "Holiday1L";
            this.Holiday1L.Size = new System.Drawing.Size(113, 24);
            this.Holiday1L.TabIndex = 14;
            this.Holiday1L.Text = "Ferienphase 1";
            // 
            // Holiday2L
            // 
            this.Holiday2L.AutoSize = true;
            this.Holiday2L.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Holiday2L.Location = new System.Drawing.Point(33, 301);
            this.Holiday2L.Name = "Holiday2L";
            this.Holiday2L.Size = new System.Drawing.Size(113, 24);
            this.Holiday2L.TabIndex = 15;
            this.Holiday2L.Text = "Ferienphase 2";
            // 
            // Holiday2
            // 
            this.Holiday2.Cue = "dd.mm.yyyy-dd.mm.yyyy";
            this.Holiday2.Location = new System.Drawing.Point(293, 299);
            this.Holiday2.Name = "Holiday2";
            this.Holiday2.Size = new System.Drawing.Size(200, 25);
            this.Holiday2.TabIndex = 5;
            // 
            // Holiday1
            // 
            this.Holiday1.Cue = "dd.mm.yyyy-dd.mm.yyyy";
            this.Holiday1.Location = new System.Drawing.Point(293, 249);
            this.Holiday1.Name = "Holiday1";
            this.Holiday1.Size = new System.Drawing.Size(200, 25);
            this.Holiday1.TabIndex = 4;
            // 
            // EndPeriod1
            // 
            this.EndPeriod1.Cue = "dd.mm.yyyy";
            this.EndPeriod1.Location = new System.Drawing.Point(293, 99);
            this.EndPeriod1.Name = "EndPeriod1";
            this.EndPeriod1.Size = new System.Drawing.Size(200, 25);
            this.EndPeriod1.TabIndex = 1;
            // 
            // EndHY
            // 
            this.EndHY.Cue = "dd.mm.yyyy";
            this.EndHY.Location = new System.Drawing.Point(293, 199);
            this.EndHY.Name = "EndHY";
            this.EndHY.Size = new System.Drawing.Size(200, 25);
            this.EndHY.TabIndex = 3;
            // 
            // StartPeriod2
            // 
            this.StartPeriod2.Cue = "dd.mm.yyyy";
            this.StartPeriod2.Location = new System.Drawing.Point(293, 149);
            this.StartPeriod2.Name = "StartPeriod2";
            this.StartPeriod2.Size = new System.Drawing.Size(200, 25);
            this.StartPeriod2.TabIndex = 2;
            // 
            // StartPeriod1
            // 
            this.StartPeriod1.Cue = "dd.mm.yyyy";
            this.StartPeriod1.Location = new System.Drawing.Point(293, 49);
            this.StartPeriod1.Name = "StartPeriod1";
            this.StartPeriod1.Size = new System.Drawing.Size(200, 25);
            this.StartPeriod1.TabIndex = 0;
            // 
            // FormForGeneratingNoteBoard
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(539, 418);
            this.Controls.Add(this.Holiday2L);
            this.Controls.Add(this.Holiday1L);
            this.Controls.Add(this.Holiday2);
            this.Controls.Add(this.Holiday1);
            this.Controls.Add(this.EndPeriod1);
            this.Controls.Add(this.EndPeriod1L);
            this.Controls.Add(this.EndHY);
            this.Controls.Add(this.StartPeriod2);
            this.Controls.Add(this.StartPeriod1);
            this.Controls.Add(this.EndHYL);
            this.Controls.Add(this.StartPeriod2L);
            this.Controls.Add(this.StartPeriod1L);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormForGeneratingNoteBoard";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bemerkungsbogen Erstellen";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.ForForGeneratingNoteBoard_HelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label StartPeriod1L;
        private System.Windows.Forms.Label StartPeriod2L;
        private System.Windows.Forms.Label EndHYL;
        private WatermarkedTextBox StartPeriod1;
        private WatermarkedTextBox StartPeriod2;
        private WatermarkedTextBox EndHY;
        private System.Windows.Forms.Label EndPeriod1L;
        private WatermarkedTextBox EndPeriod1;
        private WatermarkedTextBox Holiday1;
        private WatermarkedTextBox Holiday2;
        private System.Windows.Forms.Label Holiday1L;
        private System.Windows.Forms.Label Holiday2L;
    }
}