using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using Microsoft.CSharp.RuntimeBinder;
using System.Data;
using System.Globalization;

namespace KursheftTools
{
    [ComVisible(true)]
    public class RibbonUI : Office.IRibbonExtensibility
    {
        private static readonly CultureInfo DEUTSCHCULT = new CultureInfo("de-DE");
        private Office.IRibbonUI ribbon;

        public RibbonUI()
        {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("KursheftTools.RibbonUI.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        //A datatable stores the course plan form the csv file
        //If it was not imported at first, the FormForGeneratingPlans will not be shown
        private DataTable _coursePlan;


        /// <summary>
        /// When the button "Bemerkungsbogen Generieren" is clicked, start the window to generate the second Sheet where we could add note to the days
        /// </summary>
        public void btnCreateNoteboard_Click(Office.IRibbonControl ctrl)
        {
            //Show a window where it asks you to input the dates for this half year
            using (FormForGeneratingNoteBoard ffgs2 = new FormForGeneratingNoteBoard())
                ffgs2.ShowDialog();
        }


        /// <summary>
        /// Import the course plan and store it at the public member of RibbonUI called coursePlan as a datatable
        /// </summary>
        public void btnImportCourse_Click(Office.IRibbonControl ctrl)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                RestoreDirectory = true,
                Filter = "Kursliste (*.csv, *.txt)|*.csv;*.txt"
            })
            {

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string coursePlanPath = openFileDialog.FileName;
                    //set the names of the columns in the datatable CoursePlan CHANGE IF THE FORMAT IS CHANGED
                    string[] columnNames = new string[9] { "CourseNumber", "Class", "Teacher", "Subject", "Room", "Weekday", "Hour", "U/G", "" };
                    _coursePlan = CSVUtils.ImportCSVasDT(coursePlanPath, "Course Plan", false, columnNames);
                    MessageBox.Show("Der Kursplan wurde importiert.", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        /// <summary>
        /// With the click of the "Kursplan Generieren" button, this function will be called
        /// It will call a window, where you could give the stored place of the pdfs and the grades you want to export
        /// </summary>
        public void btnCreatePlan_Click(Office.IRibbonControl ctrl)
        {
            Excel.Worksheet noteBoard = Globals.ThisAddIn.Application.ActiveSheet;
            //check if the course plan is imported
            bool allRight = true;
            //If the course plan is not imported
            if (_coursePlan == null)
            {
                allRight = false;
                MessageBox.Show("Der Kursplan existiert nicht.\r\nImportieren Sie zuerst den Kursplan und versuchen nochmal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string[] datesStrings = new string[4];
            //Test if the note board is not right
            try
            {
                var datesCell = noteBoard.get_Range("I1", Type.Missing).Value2;
                datesStrings = datesCell.Split('~');

                //If the length of the datesString does not fit
                if (datesStrings.Length != 4)
                {
                    allRight = false;
                    MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existert nicht\r\nSind Sie vielleicht auf falscher Seite?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException Fe)
            {
                System.Diagnostics.Debug.WriteLine(Fe);
                allRight = false;
                MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existert nicht\r\nSind Sie vielleicht auf falscher Seite?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (RuntimeBinderException Re)
            {
                System.Diagnostics.Debug.WriteLine(Re);
                MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existert nicht\r\nSind Sie vielleicht auf falscher Seite?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allRight = false;
            }


            DateTime[,] holidays = new DateTime[2, 2];
            try
            {
                var holiday1 = noteBoard.get_Range("I2", Type.Missing).Value2;
                var holiday2 = noteBoard.get_Range("I3", Type.Missing).Value2;

                holidays[0, 0] = DateTime.ParseExact(holiday1.Split('~')[0], "dd.MM.yyyy", new CultureInfo("de-DE"));
                holidays[0, 1] = DateTime.ParseExact(holiday1.Split('~')[1], "dd.MM.yyyy", new CultureInfo("de-DE"));
                holidays[1, 0] = DateTime.ParseExact(holiday2.Split('~')[0], "dd.MM.yyyy", new CultureInfo("de-DE"));
                holidays[1, 1] = DateTime.ParseExact(holiday2.Split('~')[1], "dd.MM.yyyy", new CultureInfo("de-DE"));
            }
            catch (FormatException)
            {
                allRight = false;
                MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existert nicht\r\nSind Sie vielleicht auf falscher Seite?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (RuntimeBinderException)
            {
                MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existert nicht\r\nSind Sie vielleicht auf falscher Seite?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allRight = false;
            }

            if (allRight)
            {
                DateTime[] periods = new DateTime[4];
                
                foreach (string s in datesStrings)
                {
                    periods[Array.IndexOf(datesStrings, s)] = DateTime.ParseExact(s, "dd.MM.yyyy", new CultureInfo("de-DE"));
                }

                using (FormForGeneratingPlans form = new FormForGeneratingPlans(noteBoard, periods, _coursePlan, holidays))
                    form.ShowDialog();
            }


        }


        public void btnExportNoteboard_Click(Office.IRibbonControl ctrl)
        {
            //The following line could cause an error
            //SEE: https://stackoverflow.com/questions/5246288/errormessage-in-excel
            //No solution found
            //Normally it should not cause the error, if it does,
            //try to disable the following line
            //if it's disabled, disable also the other one which set it again to true
            Globals.ThisAddIn.Application.DisplayAlerts = false;

            //Initialize the dialog window for saving pdfs: 
            using (SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Bemerkungsbogen (*.xlsx)|*.xlsx",
                DefaultExt = "Bemerkungsbogen.xlsx"
            })
            {

                //Store the active workbook and worksheet
                bool isRightSheet = true;
                Excel.Worksheet sheet = Globals.ThisAddIn.Application.ActiveSheet;
                Excel.Workbook book = Globals.ThisAddIn.Application.ActiveWorkbook;

                //try to get the dates of this half year from the notes table
                //If can't find, then ask the user to recreate a table or change to the right page
                try
                {
                    var datesCell = sheet.get_Range("I1", Type.Missing).Value2;
                    string[] datesStrings = datesCell.Split('~');
                }
                catch (FormatException Fe)
                {
                    System.Diagnostics.Debug.WriteLine(Fe);
                    isRightSheet = false;
                    MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existert nicht\r\nSind Sie vielleicht auf falscher Seite?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (RuntimeBinderException Re)
                {
                    System.Diagnostics.Debug.WriteLine(Re);
                    MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existert nicht\r\nSind Sie vielleicht auf falscher Seite?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isRightSheet = false;
                }

                //Let the user to choose the place to save the pdfs
                //If the user doesn't, then stop
                if (isRightSheet && saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string storePath = saveFileDialog.FileName;

                    Globals.ThisAddIn.Application.ScreenUpdating = false;
                    var newWorkbook = Globals.ThisAddIn.Application.Workbooks.Add();
                    sheet.Copy(newWorkbook.Sheets[1]);
                    ((Excel.Worksheet)newWorkbook.Worksheets[2]).Delete();
                    ((Excel.Worksheet)newWorkbook.Worksheets[1]).Name = "Bemerkungsbogen";

                    try
                    {
                        newWorkbook.SaveAs(storePath);
                    }
                    catch (COMException e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                        book.Save();
                    }

                    newWorkbook.Close();
                }
            }
            Globals.ThisAddIn.Application.ScreenUpdating = true;
            Globals.ThisAddIn.Application.DisplayAlerts = true;
        }

        public void btnImportNoteboard_Click(Office.IRibbonControl ctrl)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Bemerkungsbogen (*.xlsx)|*.xlsx",
                RestoreDirectory = true,
                Multiselect = false
            })
            {

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string dataPath = openFileDialog.FileName;

                    try
                    {
                        Excel.Workbook noteWB = Globals.ThisAddIn.Application.Workbooks.Open(dataPath);
                        Excel.Worksheet noteSheet = noteWB.Worksheets[1];

                        var datesCell = noteSheet.get_Range("I1", System.Type.Missing).Value2;
                        //Try to split the dates
                        string[] datesStrings = datesCell.Split('~');
                        //Maybe we should handle this situation with another way
                        if (datesStrings.Length != 4) throw new ArgumentException("The length of the date string array is not three", nameof(datesStrings));
                        DateTime[] dts = new DateTime[4] { DateTime.Parse(datesStrings[0], DEUTSCHCULT), DateTime.Parse(datesStrings[1], DEUTSCHCULT), DateTime.Parse(datesStrings[2], DEUTSCHCULT), DateTime.Parse(datesStrings[3], DEUTSCHCULT) };
                    }
                    catch (ArgumentNullException)
                    {
                        System.Diagnostics.Debug.WriteLine("Argument null!");
                        MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existiert nicht.\r\nImportieren oder Erstellen Sie einen Neuen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (ArgumentException)
                    {
                        System.Diagnostics.Debug.WriteLine("length of the array datesStrings");
                        MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existiert nicht.\r\nImportieren oder Erstellen Sie einen Neuen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (RuntimeBinderException)
                    {
                        System.Diagnostics.Debug.WriteLine("RBE: did not find the dates");
                        MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existiert nicht.\r\nImportieren oder Erstellen Sie einen Neuen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (FormatException)
                    {
                        System.Diagnostics.Debug.WriteLine("Format: dates are not correct");
                        MessageBox.Show("Die Bemerkungsbogen ist beschädigt oder existiert nicht.\r\nImportieren oder Erstellen Sie einen Neuen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Generic Error at RibbonUI: ");
                        System.Diagnostics.Debug.WriteLine(e);
                        throw;
                    }
                }
            }

        }


        public void btnInformation_Click(Office.IRibbonControl ctrl)
        {
            using (FormAbout aboutForm = new FormAbout())
                aboutForm.ShowDialog();
        }

        public void btnLicenses_Click(Office.IRibbonControl ctrl)
        {
            using (FormLicense fl = new FormLicense())
                fl.ShowDialog();
        }
        #endregion


        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
