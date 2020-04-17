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
            this.LogoPathL = new System.Windows.Forms.Label();
            this.StoredIn = new System.Windows.Forms.TextBox();
            this.Grades = new System.Windows.Forms.TextBox();
            this.LogoPath = new System.Windows.Forms.TextBox();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSearch1 = new System.Windows.Forms.Button();
            this.btnSearch2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StoredInL
            // 
            this.StoredInL.AutoSize = true;
            this.StoredInL.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StoredInL.Location = new System.Drawing.Point(45, 55);
            this.StoredInL.Name = "StoredInL";
            this.StoredInL.Size = new System.Drawing.Size(137, 24);
            this.StoredInL.TabIndex = 0;
            this.StoredInL.Text = "Speichern Unter: ";
            // 
            // GradesL
            // 
            this.GradesL.AutoSize = true;
            this.GradesL.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GradesL.Location = new System.Drawing.Point(45, 111);
            this.GradesL.Name = "GradesL";
            this.GradesL.Size = new System.Drawing.Size(68, 24);
            this.GradesL.TabIndex = 1;
            this.GradesL.Text = "Stufen: ";
            // 
            // LogoPathL
            // 
            this.LogoPathL.AutoSize = true;
            this.LogoPathL.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogoPathL.Location = new System.Drawing.Point(45, 166);
            this.LogoPathL.Name = "LogoPathL";
            this.LogoPathL.Size = new System.Drawing.Size(171, 24);
            this.LogoPathL.TabIndex = 2;
            this.LogoPathL.Text = "Logo Path (Optional): ";
            // 
            // StoredIn
            // 
            this.StoredIn.Location = new System.Drawing.Point(257, 58);
            this.StoredIn.Name = "StoredIn";
            this.StoredIn.Size = new System.Drawing.Size(317, 25);
            this.StoredIn.TabIndex = 3;
            // 
            // Grades
            // 
            this.Grades.Location = new System.Drawing.Point(257, 110);
            this.Grades.Name = "Grades";
            this.Grades.Size = new System.Drawing.Size(317, 25);
            this.Grades.TabIndex = 4;
            // 
            // LogoPath
            // 
            this.LogoPath.Location = new System.Drawing.Point(257, 165);
            this.LogoPath.Name = "LogoPath";
            this.LogoPath.Size = new System.Drawing.Size(317, 25);
            this.LogoPath.TabIndex = 5;
            // 
            // btnAccept
            // 
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAccept.Location = new System.Drawing.Point(386, 239);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(200, 30);
            this.btnAccept.TabIndex = 6;
            this.btnAccept.Text = "Fertig und Generieren";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(622, 239);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Aufheben";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSearch1
            // 
            this.btnSearch1.Location = new System.Drawing.Point(598, 58);
            this.btnSearch1.Name = "btnSearch1";
            this.btnSearch1.Size = new System.Drawing.Size(120, 25);
            this.btnSearch1.TabIndex = 8;
            this.btnSearch1.Text = "Durchsuchen";
            this.btnSearch1.UseVisualStyleBackColor = true;
            this.btnSearch1.Click += new System.EventHandler(this.btnSearch1_Click);
            // 
            // btnSearch2
            // 
            this.btnSearch2.Location = new System.Drawing.Point(598, 165);
            this.btnSearch2.Name = "btnSearch2";
            this.btnSearch2.Size = new System.Drawing.Size(120, 25);
            this.btnSearch2.TabIndex = 9;
            this.btnSearch2.Text = "Durchsuchen";
            this.btnSearch2.UseVisualStyleBackColor = true;
            this.btnSearch2.Click += new System.EventHandler(this.btnSearch2_Click);
            // 
            // FormForGeneratingPlans
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(760, 296);
            this.Controls.Add(this.btnSearch2);
            this.Controls.Add(this.btnSearch1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.LogoPath);
            this.Controls.Add(this.Grades);
            this.Controls.Add(this.StoredIn);
            this.Controls.Add(this.LogoPathL);
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
        private System.Windows.Forms.Label LogoPathL;
        private System.Windows.Forms.TextBox StoredIn;
        private System.Windows.Forms.TextBox Grades;
        private System.Windows.Forms.TextBox LogoPath;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSearch1;
        private System.Windows.Forms.Button btnSearch2;
    }
}