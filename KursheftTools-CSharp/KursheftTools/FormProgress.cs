using System;
using System.Windows.Forms;

namespace KursheftTools
{
    public partial class FormProgress : Form
    {

        public ProgressBar Progressbar { get { return this.ProgressBar; } }
        public TextBox InfoBoard { get { return this.detailProgress; } }
        private string _currentExport = "";
        readonly FormForGeneratingPlans mainForm = null;
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

        
        /// <summary>
        /// Check the info in the main form every 100ms
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ProgressBar.Maximum > mainForm.ExportedPDFs)
            {
                ProgressBar.Value = mainForm.ExportedPDFs;
                ProgressL.Text = $"{mainForm.ExportedPDFs} von {ProgressBar.Maximum}";
                if (mainForm.CurrentInfo != null && mainForm.CurrentInfo != _currentExport)
                {
                    detailProgress.AppendText("\r\n" + mainForm.CurrentInfo);
                    _currentExport = mainForm.CurrentInfo;
                }
            }
            else
            {
                this.Close();
            }
        }
    }
}
