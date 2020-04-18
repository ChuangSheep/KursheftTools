using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace KursheftTools
{
    public partial class FormForGeneratingPlans : Form
    {
        private Excel.Worksheet noteBoard;
        public int ExportedPDFs = 0;
        public string currentInfo;
        private string PDFStorePath;
        private string LogoStorePath;
        private string[] PDFClasses;
        private DateTime[] Periods;
        private DataTable coursePlan;
        private static readonly string[] VALIDGRADES = new string[8] { "05", "06", "07", "08", "09", "EF", "Q1", "Q2" };

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="sheet">A excel worksheet object represents the note board. </param>
        /// <param name="prds">An array of 3 DateTime objects represents the start of year, 
        ///             the start of the second period and the end of the half year. </param>
        /// <param name="coursePlan">A datatable object contains the full course list. </param>
        public FormForGeneratingPlans(Excel.Worksheet sheet, DateTime[] prds, DataTable coursePlan)
        {
            //Initialize the class menbers
            if (prds.Length == 3) Periods = prds;
            else throw new ArgumentException("The augument \"prds\" does not have 3 items", "prds");

            noteBoard = sheet;
            this.coursePlan = coursePlan;
            
            InitializeComponent();
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Wählen Sie einen Ordner aus, um die PDF-Datei darunter zu speichern."
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;

                StoredIn.Text = path;
            }
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png, *.jpg, *.jpeg) | *.png; *.jpg; *.jpeg",
                RestoreDirectory = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;

                LogoPath.Text = path;
            }
        }

        /// <summary>
        /// Check the input data
        /// If these are valid, start to export the plans
        /// Otherwise, mark the wrong field as red
        /// </summary>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            Color RED = Color.FromArgb(255, 192, 192);
            //Reset the colors
            Grades.BackColor = Color.White;
            StoredIn.BackColor = Color.White;
            LogoPath.BackColor = Color.White;

            bool allRight = true;

            //if the directory or file user inputed does not exist
            //mark it red, and dont accept
            if (!Directory.Exists(StoredIn.Text))
            {
                StoredIn.BackColor = RED;
                allRight = false;
            }
            //If there is a wrong value for the LogoPath
            if (LogoPath.Text != "" && !File.Exists(LogoPath.Text))
            {
                LogoPath.BackColor = RED;
                allRight = false;
            }

            //If the grades are not valid
            string[] gradesToExport = Grades.Text.Split(';');
            foreach (string s in gradesToExport)
            {
                if (!Array.Exists(VALIDGRADES, element => element == s))
                {
                    Grades.BackColor = RED;
                    allRight = false;
                }
            }

            //If everything is right
            if (allRight)
            {
                //Hide the current form
                this.Hide();

                //Write the text into the variables
                PDFStorePath = StoredIn.Text;
                LogoStorePath = LogoPath.Text;
                string[] GradesFromTB = Grades.Text.Split(';');
                List<string> grds = new List<string>();
                string[] Classes = new string[5] { "a", "b", "c", "d", "e" };
                foreach (string s in GradesFromTB)
                {
                    if (s == "Q1" || s == "Q2")
                    {
                        grds.Add(s);
                    }
                    else
                    {
                        foreach (string cls in Classes)
                        {
                            grds.Add(s + cls);
                        }
                    }
                }

                PDFClasses = grds.ToArray();

                //Call the export function
                ExportPlans();

                MessageBox.Show($"{ExportedPDFs} PDF-Datei wurde erfolgreich exportiert unter\r\n {PDFStorePath}", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.Yes;
                this.Close();

            }


        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }


        /// <summary>
        /// Export the plans based on the given coursePlan and the note board
        /// </summary>
        /// <returns>An Integer represents how many pdf files were exported</returns>
        private void ExportPlans()
        {

            //Initialize the lists for the loop
            List<CoursePlan> Plans = new List<CoursePlan>();
            List<DateTime[]> Dates = new List<DateTime[]>();
            List<string[]> IsRegular = new List<string[]>();

            foreach (string CurrentValidClass in PDFClasses)
            {
                //Get the datatable that contains all the information for the current class
                try
                {
                    DataTable currentClassDT = coursePlan.Select($"Class = '{CurrentValidClass}'").CopyToDataTable();

                    //Get a List containing all unique subjects
                    var subjectList = currentClassDT.AsEnumerable().Select(s => new
                    {
                        Subject = s.Field<string>("Subject")
                    })
                    .Distinct().ToList();

                    //Traverse the list of subjects
                    foreach (var currentSubject in subjectList)
                    {
                        string a = currentSubject.ToString().Remove(0, 12);
                        char[] cA = a.ToCharArray();
                        a = "";
                        for (int i = 0; i < cA.Length - 2; i++)
                        {
                            a += cA[i];
                        }


                        //Get the information of a specific course
                        DataRow[] currentCourse = currentClassDT.Select($"Subject = '{a}'");

                        if (currentCourse.Length != 0 && (string)currentCourse[0].ItemArray[1] != "")
                        {
                            //Create a new plan
                            CoursePlan currentPlan = new CoursePlan(currentCourse[0].ItemArray[3].ToString(), currentCourse[0].ItemArray[1].ToString(),
                                                                currentCourse[0].ItemArray[2].ToString());
                            List<DateTime> currentDT = new List<DateTime>();
                            List<string> currentIsRegular = new List<string>();
                            foreach (DataRow row in currentCourse)
                            {
                                //Assuming that all the rows are not same
                                int indexOfRow = Array.IndexOf(currentCourse, row);

                                //If this row does not contain any number or any class
                                if ((string)row.ItemArray[0] == "" || (string)row.ItemArray[1] == "") continue;
                                //If this is the first line or the dates are not the same
                                if (indexOfRow == 0 ||
                                    DateTime.Compare(DateTimeCalcUtils.GetNearestWeekdayS(Periods[0], DateTimeCalcUtils.GetWeekdayFromNumber(int.Parse(currentCourse[indexOfRow].ItemArray[5].ToString()))), currentDT.Last()) != 0)
                                {
                                    currentDT.Add(DateTimeCalcUtils.GetNearestWeekdayS(Periods[0], DateTimeCalcUtils.GetWeekdayFromNumber(int.Parse(currentCourse[indexOfRow].ItemArray[5].ToString()))));
                                    currentIsRegular.Add(currentCourse[indexOfRow].ItemArray[7].ToString());
                                }
                                //If the isRegular value, however, are not the same, then if there's a course on this day on every week, set isRegular to regular("")
                                else if ("" != currentIsRegular.Last() && row.ItemArray[7].ToString() != currentIsRegular.Last()) currentIsRegular[currentIsRegular.Count - 1] = "";

                            }

                            //Add them to the list
                            Plans.Add(currentPlan);
                            Dates.Add(currentDT.ToArray());
                            IsRegular.Add(currentIsRegular.ToArray());
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    continue;
                }
            }

            //Start a new thread showing the progress
            Thread progressT = new Thread(delegate ()
            {
                FormProgress formProgress = new FormProgress(this, Plans.Count);
                formProgress.ShowDialog();
            });
           progressT.Start();

            //Export them
            if (Plans.Count == Dates.Count && Dates.Count == IsRegular.Count && IsRegular.Count != 0)
            {
                for (int i = 0; i < Plans.Count; i++)
                {
                    CoursePlan currentCoursePlan = Plans[i];
                    currentCoursePlan.ReadNoteBoard(noteBoard, Dates[i], IsRegular[i]);
                    //After all the note board processed
                    //Export the current course plan
                    currentCoursePlan.ExportAsPDF(Periods, PDFStorePath, LogoStorePath != "" ? LogoStorePath : "default");
                    currentInfo = $"{currentCoursePlan.GetTitle()} wurde exportiert unter: {PDFStorePath}";
                    ExportedPDFs++;
                }

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("The Plans is empty: no plan stored");
                ExportedPDFs =  -1;
            }

            //progressT.Abort();
            System.Diagnostics.Debug.WriteLine($"{ExportedPDFs} wurde exportiert unter: {PDFStorePath}");
        }
    }
}
