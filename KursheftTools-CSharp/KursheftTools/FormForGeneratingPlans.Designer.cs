namespace KursheftTools
{
    partial class FormForGeneratingPlans
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
            this.StoredInL = new System.Windows.Forms.Label();
            this.GradesL = new System.Windows.Forms.Label();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSearch1 = new System.Windows.Forms.Button();
            this.DoMerge = new System.Windows.Forms.CheckBox();
            this.Grades = new KursheftTools.WatermarkedTextBox();
            this.StoredIn = new KursheftTools.WatermarkedTextBox();
            this.SuspendLayout();
            // 
            // StoredInL
            // 
            this.StoredInL.AutoSize = true;
            this.StoredInL.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StoredInL.Location = new System.Drawing.Point(45, 59);
            this.StoredInL.Name = "StoredInL";
            this.StoredInL.Size = new System.Drawing.Size(137, 24);
            this.StoredInL.TabIndex = 0;
            this.StoredInL.Text = "Speichern Unter: ";
            // 
            // GradesL
            // 
            this.GradesL.AutoSize = true;
            this.GradesL.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GradesL.Location = new System.Drawing.Point(45, 118);
            this.GradesL.Name = "GradesL";
            this.GradesL.Size = new System.Drawing.Size(68, 24);
            this.GradesL.TabIndex = 1;
            this.GradesL.Text = "Stufen: ";
            // 
            // btnAccept
            // 
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAccept.Location = new System.Drawing.Point(386, 205);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(200, 32);
            this.btnAccept.TabIndex = 4;
            this.btnAccept.Text = "&Fertig und Generieren";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(622, 205);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 32);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Aufheben";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSearch1
            // 
            this.btnSearch1.Location = new System.Drawing.Point(598, 56);
            this.btnSearch1.Name = "btnSearch1";
            this.btnSearch1.Size = new System.Drawing.Size(120, 27);
            this.btnSearch1.TabIndex = 1;
            this.btnSearch1.Text = "&Durchsuchen";
            this.btnSearch1.UseVisualStyleBackColor = true;
            this.btnSearch1.Click += new System.EventHandler(this.btnSearch1_Click);
            // 
            // DoMerge
            // 
            this.DoMerge.AutoSize = true;
            this.DoMerge.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoMerge.Location = new System.Drawing.Point(49, 205);
            this.DoMerge.Name = "DoMerge";
            this.DoMerge.Size = new System.Drawing.Size(209, 28);
            this.DoMerge.TabIndex = 3;
            this.DoMerge.Text = "Merge nach dem Export";
            this.DoMerge.UseVisualStyleBackColor = true;
            // 
            // Grades
            // 
            this.Grades.Cue = "EF;Q1";
            this.Grades.Location = new System.Drawing.Point(257, 121);
            this.Grades.Name = "Grades";
            this.Grades.Size = new System.Drawing.Size(317, 22);
            this.Grades.TabIndex = 2;
            // 
            // StoredIn
            // 
            this.StoredIn.Cue = "F:\\Path\\To\\Save";
            this.StoredIn.Location = new System.Drawing.Point(257, 61);
            this.StoredIn.Name = "StoredIn";
            this.StoredIn.Size = new System.Drawing.Size(317, 22);
            this.StoredIn.TabIndex = 0;
            // 
            // FormForGeneratingPlans
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(760, 279);
            this.Controls.Add(this.StoredIn);
            this.Controls.Add(this.Grades);
            this.Controls.Add(this.DoMerge);
            this.Controls.Add(this.btnSearch1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.GradesL);
            this.Controls.Add(this.StoredInL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormForGeneratingPlans";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kurshefte Generieren";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label StoredInL;
        private System.Windows.Forms.Label GradesL;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSearch1;
        private System.Windows.Forms.CheckBox DoMerge;
        private WatermarkedTextBox Grades;
        private WatermarkedTextBox StoredIn;
    }
}