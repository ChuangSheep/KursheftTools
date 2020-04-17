using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace KursheftTools
{
    public partial class FormForGeneratingPlans : Form
    {
        private Excel.Worksheet noteBoard;
        private string PDFStorePath;
        private string LogoStorePath;
        private string[] PDFGrades;
        private DateTime[] Periods;
        private DataTable coursePlan;
        private static readonly string[] VALIDGRADES = new string[8] { "05", "06", "07", "08", "09", "EF", "Q1", "Q2" };
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
                PDFGrades = Grades.Text.Split(';');

                //Call the export function
                int NumExported = ExportPlans();

                MessageBox.Show($"{NumExported} PDF-Datei wurde erfolgreich exportiert unter\r\n {PDFStorePath}", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.Yes;
                this.Close();

            }


        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private int ExportPlans()
        {
            int ExportedPDFs = 0;
            //Get all rows of the course list
            //Assuming all the order are not changed
            //And the courses and the dates are ordered properly
            DataRow[] rows = coursePlan.Select();

            //Initialize the variables for the usage in the loop
            string CurrentCourseNum = "", CurrentClass = "", Teacher = "", CurrentSubject = "";
            List<DateTime> dates = new List<DateTime>();
            List<string> isRegular = new List<string>();

            foreach (DataRow row in rows)
            {
                //Assuming that all the rows are not same
                int indexOfRow = Array.IndexOf(rows, row);
                //Initialize the variables for the loop
                string CurrentCoursegrade = "";
                try
                {
                     CurrentCoursegrade = row.ItemArray[1].ToString().Length == 2 ? row.ItemArray[1].ToString() : row.ItemArray[1].ToString().Substring(0, 2);
                }
                catch (ArgumentOutOfRangeException)
                {
                    //System.Diagnostics.Debug.WriteLine(AOORE);
                    continue;
                }
                //If we should export this grade
                if (Array.Exists(PDFGrades, element => element == CurrentCoursegrade))
                {

                    //Jump the rows without a course number or a class name
                    if (row.ItemArray[0].ToString() == "" || row.ItemArray[1].ToString() == "") continue;

                        //If the current course is still for this row
                    if (CurrentCourseNum == row.ItemArray[0].ToString() && CurrentClass == row.ItemArray[1].ToString() && CurrentSubject == row.ItemArray[3].ToString())
                    {
                        //If this date is not added, add it
                        if (dates.Last() != DateTimeCalcUtils.GetNearestWeekday(Periods[0], DateTimeCalcUtils.GetWeekdayFromNumber(int.Parse(row.ItemArray[5].ToString()))))
                        {
                            dates.Add(DateTimeCalcUtils.GetNearestWeekday(Periods[0], DateTimeCalcUtils.GetWeekdayFromNumber(int.Parse(row.ItemArray[5].ToString()))));
                            isRegular.Add(row.ItemArray[7].ToString());
                        }
                        //If the isRegular value, however, are not the same, then if there's a course on this day on every week, set isRegular to regular("")
                        else if ("" != isRegular.Last() && row.ItemArray[7].ToString() != isRegular.Last()) isRegular[isRegular.Count - 1] = "";
                    }
                        //If we are going to a new course
                    else
                    {
                        CurrentCourseNum = row.ItemArray[0].ToString();
                        CurrentClass = row.ItemArray[1].ToString();
                        Teacher = row.ItemArray[2].ToString();
                        CurrentSubject = row.ItemArray[3].ToString();

                        isRegular.Clear();
                        dates.Clear();
                        //Add the date to the array
                        isRegular.Add(row.ItemArray[7].ToString());
                        dates.Add(DateTimeCalcUtils.GetNearestWeekday(Periods[0], DateTimeCalcUtils.GetWeekdayFromNumber(int.Parse(row.ItemArray[5].ToString()))));
       
                    }


                    //Test if the next row is for another course

                    //If this is already the last line
                    //Or, if the course number or the class or the subject is different in the next row
                    if (indexOfRow == rows.Length - 1 || rows[indexOfRow + 1].ItemArray[0].ToString() != CurrentCourseNum 
                        || rows[indexOfRow + 1].ItemArray[1].ToString() != CurrentClass || rows[indexOfRow + 1].ItemArray[3].ToString() != CurrentSubject)
                    {
                        //Initialize the course plan
                        CoursePlan currentCoursePlan = new CoursePlan(CurrentSubject, CurrentClass, Teacher);

                        //Convert the lists to the array
                        DateTime[] DatesA = dates.ToArray();
                        string[] IsRegularA = isRegular.ToArray();
                        //Clear the lists
                        dates.Clear();
                        isRegular.Clear();
                        //Sort the date
                        //The order of the array "IsRegularA" should be changed at the same time
                        DateTimeCalcUtils.SortDate(ref DatesA, ref IsRegularA);

                        //Go through each line of the note board
                        int k = 0;
                        for (int i = 3; i < noteBoard.UsedRange.Rows.Count - 1; i++)
                        {
                            var lineDate = ((Excel.Range)noteBoard.Cells[i, 2]).Value;
                            DateTime DlineDate = new DateTime();

                            try
                            {
                                //Try to convert the lineDate to DateTime obj
                                 DlineDate = lineDate;
                            }
                            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                            {
                                //Jump the title lines
                                //System.Diagnostics.Debug.WriteLine(Rbe);

                                //Jump the holiday
                                for (int n = 0; n < DatesA.Length; n++)
                                {
                                    DatesA[n] = DatesA[n].AddDays(14);
                                }
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine($"Generic Exception: {e}");
                            }

                            if (DateTime.Compare(DlineDate, new DateTime()) != 0)
                            {
                                //If the date fits
                                if (DateTime.Compare(DlineDate, DatesA[k]) == 0)
                                {
                                    //If this weekday is regular, or it fits to the rule
                                    if (IsRegularA[k] == "" ||
                                        (DateTimeCalcUtils.IsEvenWeek(DlineDate) && IsRegularA[k] == "g") ||
                                        (!DateTimeCalcUtils.IsEvenWeek(DlineDate) && IsRegularA[k] == "u"))
                                    {
                                        Daynotes currentDaynotes = new Daynotes(DlineDate);

                                        //Go through the three notes on the same row
                                        for (int j = 3; j < 8; j += 2)
                                        {
                                            string currentNote = ((Excel.Range)noteBoard.Cells[i, j]).Value;
                                            string currentLineGrade = ((Excel.Range)noteBoard.Cells[i, j + 1]).Value;

                                            //When the note is not empty
                                            if (currentNote != null)
                                            {
                                                //If the grade fits to the current course
                                                if (currentLineGrade == CurrentCoursegrade || currentLineGrade == null)
                                                {
                                                    currentDaynotes.AddNote(currentNote);
                                                }
                                            }
                                        }
                                        //Add the daynote to the plan
                                        currentCoursePlan.AddLine(currentDaynotes);
                                    }

                                    DatesA[k] = DatesA[k].AddDays(7);

                                    //Move the counter of the DatesA array
                                    if (k < DatesA.Length - 1) k++;
                                    else k = 0;
                                }
                            }
                        }

                        //After all the note board processed
                        //Export the current course plan
                        currentCoursePlan.ExportAsPDF(Periods, PDFStorePath, LogoStorePath != "" ? LogoStorePath : "default");
                        ExportedPDFs++;
                    }
                }

            }

            System.Diagnostics.Debug.WriteLine($"{ExportedPDFs} sind exportiert unter: {PDFStorePath}");
            return ExportedPDFs;
        }
    }
}
