using System;
using System.Windows.Forms;

namespace KursheftTools
{
    public partial class FormProgress : Form
    {

        public ProgressBar Progressbar { get { return this.ProgressBar; } }
        public TextBox InfoBoard { get { return this.detailProgress; } }
        private string currentExport = "";

        FormForGeneratingPlans mainForm = null;
        public FormProgress(FormForGeneratingPlans mForm, int maximalLength)
        {
            InitializeComponent();
            this.ProgressBar.Maximum = maximalLength;
            mainForm = mForm;
        }

        private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            //If the user want to close this form, prevent it
            e.Cancel = true;
        }

        

        private void FormProgress_Load(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ProgressBar.Maximum > mainForm.ExportedPDFs)
            {
                ProgressBar.Value = mainForm.ExportedPDFs;
                ProgressL.Text = $"{mainForm.ExportedPDFs} von {ProgressBar.Maximum}";
                if (mainForm.currentInfo != null && mainForm.currentInfo != currentExport)
                {
                    detailProgress.AppendText("\r\n" + mainForm.currentInfo);
                    currentExport = mainForm.currentInfo;
                }
            }
            else
            {
                this.Close();
            }
        }
    }
}
