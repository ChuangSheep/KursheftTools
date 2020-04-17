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
            this.StartPeriod1 = new System.Windows.Forms.TextBox();
            this.StartPeriod2 = new System.Windows.Forms.TextBox();
            this.EndHY = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(196, 235);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(96, 30);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "&Fertig";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(322, 235);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 30);
            this.btnCancel.TabIndex = 1;
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
            this.StartPeriod1L.Size = new System.Drawing.Size(190, 24);
            this.StartPeriod1L.TabIndex = 2;
            this.StartPeriod1L.Text = "Anfang d. 1. Abschnitts: ";
            // 
            // StartPeriod2L
            // 
            this.StartPeriod2L.AutoSize = true;
            this.StartPeriod2L.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartPeriod2L.Location = new System.Drawing.Point(33, 111);
            this.StartPeriod2L.Name = "StartPeriod2L";
            this.StartPeriod2L.Size = new System.Drawing.Size(190, 24);
            this.StartPeriod2L.TabIndex = 3;
            this.StartPeriod2L.Text = "Anfang d. 2. Abschnitts: ";
            // 
            // EndHYL
            // 
            this.EndHYL.AutoSize = true;
            this.EndHYL.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndHYL.Location = new System.Drawing.Point(33, 170);
            this.EndHYL.Name = "EndHYL";
            this.EndHYL.Size = new System.Drawing.Size(170, 24);
            this.EndHYL.TabIndex = 4;
            this.EndHYL.Text = "Ende des Halbjahres: ";
            // 
            // StartPeriod1
            // 
            this.StartPeriod1.Location = new System.Drawing.Point(267, 49);
            this.StartPeriod1.Name = "StartPeriod1";
            this.StartPeriod1.Size = new System.Drawing.Size(151, 25);
            this.StartPeriod1.TabIndex = 5;
            // 
            // StartPeriod2
            // 
            this.StartPeriod2.Location = new System.Drawing.Point(267, 110);
            this.StartPeriod2.Name = "StartPeriod2";
            this.StartPeriod2.Size = new System.Drawing.Size(151, 25);
            this.StartPeriod2.TabIndex = 6;
            // 
            // EndHY
            // 
            this.EndHY.Location = new System.Drawing.Point(267, 169);
            this.EndHY.Name = "EndHY";
            this.EndHY.Size = new System.Drawing.Size(151, 25);
            this.EndHY.TabIndex = 7;
            // 
            // FormForGeneratingNoteBoard
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(465, 283);
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
        private System.Windows.Forms.TextBox StartPeriod1;
        private System.Windows.Forms.TextBox StartPeriod2;
        private System.Windows.Forms.TextBox EndHY;
    }
}