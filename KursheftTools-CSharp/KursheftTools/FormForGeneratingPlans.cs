using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
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
        private static readonly System.Globalization.CultureInfo DEUTSCHCULT = new System.Globalization.CultureInfo("de-DE");

        private readonly Excel.Worksheet _noteBoard;
        //Stores how many Pdfs are exported yet
        //It is a class member mainly because of the progress window
        //Only so can the data transport between threads possible
        public int ExportedPDFs = 0;
        //The information to show on the progress window
        public string CurrentInfo;
        private string _PDFStorePath;
        private string[] _PDFClasses;
        private string[] _grades;
        private bool _mergeAfterExport;
        private readonly DateTime[] _periods;
        private readonly DataTable _coursePlan;
        private readonly DateTime[,] _holidays;
        //Change if there is more grades
        //ex. "10" for the Realschule und Hauptschule
        private static readonly string[] VALIDGRADES = new string[8] { "05", "06", "07", "08", "09", "EF", "Q1", "Q2" };

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="sheet">A excel worksheet object represents the note board. </param>
        /// <param name="prds">An array of 4 DateTime objects represents the start of first period, end of the first period
        ///             the start of the second period and the end of the half year. </param>
        /// <param name="coursePlan">A datatable object contains the full course list. </param>
        public FormForGeneratingPlans(Excel.Worksheet sheet, DateTime[] prds, DataTable coursePlan, DateTime[,] holidays)
        {
            //Initialize the class menbers
            if (prds.Length == 4) _periods = prds;
            else throw new ArgumentException("The augument \"prds\" does not have 4 items", nameof(prds));

            _noteBoard = sheet;
            _coursePlan = coursePlan;
            _holidays = holidays;
            
            InitializeComponent();
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Wählen Sie einen Ordner aus, um die PDF-Datei darunter zu speichern."
            })
            {

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = folderBrowserDialog.SelectedPath;

                    StoredIn.Text = path;
                }
            }
        }


        /// <summary>
        /// Check the input data
        /// If these are valid, start to export the plans
        /// Otherwise, mark the wrong field as red
        /// </summary>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            //Set the dialog result to none to avoid automatic close of the form 
            DialogResult = DialogResult.None;
            Color RED = Color.FromArgb(255, 192, 192);
            //Reset the colors
            Grades.BackColor = Color.White;
            StoredIn.BackColor = Color.White;

            bool allRight = true;

            //if the directory or file user inputed does not exist
            //mark it red, and dont accept
            if (!Directory.Exists(StoredIn.Text))
            {
                StoredIn.BackColor = RED;
                allRight = false;
            }

            // Save the checkbox status
            this._mergeAfterExport = DoMerge.Checked;

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
                _PDFStorePath = StoredIn.Text;
                //Get all the classes that need to be exported
                //If the user inputed 05, then the array will contain 05a, 05b, 05c, 05d, 05e
                //Change the following if there is more or less classes
                this._grades = gradesToExport;
                List<string> grds = new List<string>();
                string[] Classes = new string[5] { "a", "b", "c", "d", "e"};
                foreach (string s in gradesToExport)
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
                        if (s == "EF")
                        {
                            grds.Add(s + "f");
                        }
                    }
                }

                _PDFClasses = grds.ToArray();


                //Call the export function
                //This will process the datatable, start the export and show a progress window
                ExportPlans();

                // If should merge, then merge
                if (this._mergeAfterExport)
                {
                    MergePDFs();
                }
                GC.Collect();

                MessageBox.Show($"{ExportedPDFs} PDF-Datei wurde erfolgreich exportiert unter\r\n {_PDFStorePath}", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            List<CoursePlan> plans = new List<CoursePlan>();
            List<DateTime[]> dates = new List<DateTime[]>();
            List<string[]> isRegular = new List<string[]>();

            foreach (string CurrentValidClass in _PDFClasses)
            {
                //Get the datatable that contains all the information for the current class
                try
                {
                    DataTable currentClassDT = _coursePlan.Select($"Class = '{CurrentValidClass}'").CopyToDataTable();

                    //Get a List containing all unique subjects
                    var subjectList = currentClassDT.AsEnumerable().Select(s => new
                    {
                        Subject = s.Field<string>("Subject")
                    })
                    .Distinct().ToList();

                    //Traverse the list of subjects
                    foreach (var currentSubject in subjectList)
                    {
                        //Check the subject and remove the illegal chars
                        //As the subjectList contains the strings like {Subject = GE} etc. 
                        string sbj = currentSubject.ToString().Substring(12, currentSubject.ToString().Length - 2 - 12);

                        //Get the information of a specific course
                        DataRow[] currentCourse = currentClassDT.Select($"Subject = '{sbj}'");

                        if (currentCourse.Length != 0 && !string.IsNullOrEmpty((string)currentCourse[0].ItemArray[1]))
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
                                if (string.IsNullOrEmpty((string)row.ItemArray[0]) || string.IsNullOrEmpty((string)row.ItemArray[1])) continue;
                                //If this is the first line or the dates are not the same
                                if (indexOfRow == 0 ||
                                    DateTime.Compare(DateTimeCalcUtils.GetNearestWeekdayS(_periods[0], DateTimeCalcUtils.GetWeekdayFromNumber(int.Parse(currentCourse[indexOfRow].ItemArray[5].ToString(), DEUTSCHCULT))), currentDT.Last()) != 0)
                                {
                                    currentDT.Add(DateTimeCalcUtils.GetNearestWeekdayS(_periods[0], DateTimeCalcUtils.GetWeekdayFromNumber(int.Parse(currentCourse[indexOfRow].ItemArray[5].ToString(), DEUTSCHCULT))));
                                    
                                    //If the course is on the 8th hour, then it will only be on even weeks
                                    if (row.ItemArray[6].ToString() == "8")
                                    {
                                        currentIsRegular.Add("g");
                                    }
                                    //On the 9th hour, then only on odd weeks
                                    else if (row.ItemArray[6].ToString() == "9")
                                    {
                                        currentIsRegular.Add("u");
                                    }
                                    //Otherwise every week
                                    else
                                    {
                                        currentIsRegular.Add("");
                                    }
                                }
                                //If the isRegular value, however, are not the same, then if there's a course on this day on every week, set isRegular to regular("")
                                else if (!string.IsNullOrEmpty(currentIsRegular.Last()) && 
                                            //If this course will be held on 8th AND on 9th hour
                                            //Then change the IsRegular to every week
                                            (
                                            (row.ItemArray[6].ToString() != "8" && row.ItemArray[6].ToString() != "9") ||
                                            (row.ItemArray[6].ToString() == "8" && currentIsRegular.Last() == "u") ||
                                            (row.ItemArray[6].ToString() == "9" && currentIsRegular.Last() == "g"))
                                            )
                                {
                                    currentIsRegular[currentIsRegular.Count - 1] = "";
                                }
                            }

                            //Add them to the list
                            plans.Add(currentPlan);
                            dates.Add(currentDT.ToArray());
                            isRegular.Add(currentIsRegular.ToArray());
                        }
                    }
                }
                //If there is no course for this class, the there would be an exception thrown by .CopyToDataTable()
                //Therefore, catch this exception and just continue to the next class
                catch (InvalidOperationException)
                {
                    continue;
                }
            }

            //Start a new thread showing the progress
            Thread progressWindowThread = new Thread(delegate ()
            {
                FormProgress formProgress = new FormProgress(this, plans.Count);
                formProgress.ShowDialog();
            });
           progressWindowThread.Start();

            //Export them
            if (plans.Count == dates.Count && dates.Count == isRegular.Count && isRegular.Count != 0)
            {
                for (int i = 0; i < plans.Count; i++)
                {
                    CoursePlan currentCoursePlan = plans[i];
                    currentCoursePlan.ReadNoteBoard(_noteBoard, dates[i], isRegular[i], _holidays);
                    //After all the note board processed
                    //Export the current course plan
                    _ = currentCoursePlan.ExportAsPDF(_periods, _PDFStorePath);
                    CurrentInfo = $"{currentCoursePlan.GetTitle()} wurde exportiert. ";
                    ExportedPDFs++;
                }

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("The Plans is empty: no plan stored");
                ExportedPDFs =  -1;
            }

            //Close the progress window
            progressWindowThread.Abort();
            System.Diagnostics.Debug.WriteLine($"{ExportedPDFs} wurde exportiert unter: {_PDFStorePath}");
        }

        private void MergePDFs()
        {
            string[] pdfs = Directory.GetFiles(_PDFStorePath);
            PdfDocument document = new PdfDocument();
            foreach (string pdfFile in pdfs)
            {
                if (pdfFile.Contains("merged")) continue;
                PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);
                document.Version = inputPDFDocument.Version;
                foreach (PdfPage page in inputPDFDocument.Pages)
                {
                    document.AddPage(page);
                }
                // Delete the origin if needed
                // System.IO.File.Delete(pdfFile);
            }

            string gradesStr = "";
            foreach (string s in this._grades)
            {
                gradesStr += $"-{s}";
            }

            document.Options.CompressContentStreams = true;
            document.Options.NoCompression = false;

            document.Save($"{this._PDFStorePath}/merged{gradesStr}.pdf");
            document.Close();
            document.Dispose();
        }
    }
}
