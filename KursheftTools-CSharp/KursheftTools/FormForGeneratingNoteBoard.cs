using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace KursheftTools
{
    public partial class FormForGeneratingNoteBoard : Form
    {
        public FormForGeneratingNoteBoard()
        {
            InitializeComponent();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            Color RED = Color.FromArgb(255, 192, 192);
            DateTime[] periods = new DateTime[3];
            //Check the validity of the given dates
            //Store the text boxes into an array called textboxs
            TextBox[] textboxs = new TextBox[3] { StartPeriod1, StartPeriod2, EndHY };

            bool allRight = true;
            foreach (TextBox tB in textboxs)
            {
                tB.BackColor = Color.White;
                try
                {
                    periods[Array.IndexOf(textboxs, tB)] = DateTime.ParseExact(tB.Text, "dd-MM-yyyy", new CultureInfo("de-DE"));
                    if (DateTimeCalcUtils.GetWeekday(periods[Array.IndexOf(textboxs, tB)]) == "Sa" ||
                        DateTimeCalcUtils.GetWeekday(periods[Array.IndexOf(textboxs, tB)]) == "So") throw new FormatException("The dates could not be on the weekends");
                }
                catch (FormatException)
                {
                    allRight = false;
                    tB.BackColor = RED;
                }

                catch (Exception err)
                {
                    Console.WriteLine("Error in \'FormForGeneratingSheet2.cs\'");
                    Console.WriteLine($"Generic Exception Handler: {err}");
                }
            }

            if (allRight)
            {
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        //Test if the dates do not fit or the periods are too short
                        DateTimeCalcUtils.BusinessDaysUntil(periods[i], periods[i + 1].AddDays(-17));
                    }
                    catch (ArgumentException)
                    {
                        allRight = false;
                        textboxs[i + 1].BackColor = RED;
                    }

                }
            }

            if (allRight)
            {
                this.DialogResult = DialogResult.Yes;
                this.Hide();
                if (CreateBoard(periods)) MessageBox.Show("Ein leerer Bemerkungsbogen wurde erfolgreich generiert.\r\nAbschnitts: " +
                    $"{periods[0].ToString("dd-MM-yyyy")} ~ {periods[1].ToString("dd-MM-yyyy")} ~ {periods[2].ToString("dd-MM-yyyy")}", 
                    "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ForForGeneratingNoteBoard_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            MessageBox.Show("Die Datum soll in dieser Form \'dd-mm-yyyy\' angegeben werden", 
                "Hilfe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Boolean CreateBoard(DateTime[] dates)
        {
            if (dates.Length != 3) throw new ArgumentException("The augument \"dates\" (array of DateTime) must have 3 items", "dates");

            //start a new sheet 
            Excel.Worksheet sheet1;
            sheet1 = (Excel.Worksheet)Globals.ThisAddIn.Application.Worksheets.Add();

            Excel.Range tRange;

            //Entire Sheet Setting:
            Excel.Range er = sheet1.get_Range("A:A", System.Type.Missing);
            er.EntireColumn.EntireRow.ColumnWidth = 30;
            er.EntireColumn.EntireRow.RowHeight = 30;

            for (int i = 1; i < 4; i++)
            {
                sheet1.Cells[1, (i + 1) * 2].EntireColumn.ColumnWidth = 10;
                sheet1.Cells[1, (i + 1) * 2].EntireColumn.WrapText = true;
                sheet1.Cells[1, (i + 1) * 2].EntireColumn.NumberFormat = "@";
                sheet1.Cells[1, (i + 1) * 2 - 1] = "Besonderheit " + i;
                sheet1.Cells[1, (i + 1) * 2] = "Stufe";
            }


            //Titles:
            sheet1.Cells[1, 1] = "Wochentage";
            sheet1.Cells[1, 2] = "Datum";
            sheet1.Cells[1, 9] = dates[0].ToString("dd-MM-yyyy") + "~" + dates[1].ToString("dd-MM-yyyy") + "~" + dates[2].ToString("dd-MM-yyyy");
            sheet1.Cells[1, 1].EntireRow.Font.Bold = true;
            sheet1.Cells[1, 1].EntireRow.Font.Size = 14;

            sheet1.Application.ActiveWindow.SplitRow = 1;
            sheet1.Application.ActiveWindow.FreezePanes = true;


            #region 1. + 2. column: Weekdays and Dates
            //Jump the holidays
            int[] dateLength = new int[3] { 0, DateTimeCalcUtils.BusinessDaysUntil(dates[0], dates[1].AddDays(-17)), DateTimeCalcUtils.BusinessDaysUntil(dates[0], dates[1].AddDays(-17)) + DateTimeCalcUtils.BusinessDaysUntil(dates[1], dates[2]) - 1 };
            string[] titlesStart = new string[3] { "Anfang d. 1. Abschnitts", "Anfang d. 2. Abschnitts", "Ende des Schulhalbjahres" };
            sheet1.get_Range("B:B", System.Type.Missing).NumberFormat = "dd-MM-yyyy";  //It MAY cause a problem

            for (int i = 0; i < dates.Length; i++)
            {
                tRange = sheet1.Range[sheet1.Cells[dateLength[i] + 3 + i, 1], sheet1.Cells[dateLength[i] + 3 + i, 1]];
                tRange.Value = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dates[i].DayOfWeek);


                tRange = sheet1.Range[sheet1.Cells[dateLength[i] + 3 + i, 2], sheet1.Cells[dateLength[i] + 3 + i, 2]];
                tRange.Value = dates[i].Date;

                if (i < 2)
                {
                    //IMPORTANT: We assume that all the three entered dates are WEEKDAYS
                    sheet1.Cells[dateLength[i] + 2 + i, 2] = titlesStart[i];
                    sheet1.Cells[dateLength[i] + 2 + i, 2].EntireRow.Interior.Color = Color.FromArgb(146, 208, 80);
                    ((Excel.Range)sheet1.Range[sheet1.Cells[dateLength[i] + 2 + i, 2], sheet1.Cells[dateLength[i] + 2 + i, 2]]).Font.Size = 14;
                    tRange.AutoFill(sheet1.Range[sheet1.Cells[dateLength[i] + 3 + i, 2], sheet1.Cells[dateLength[i + 1] + i + 2, 2]], Excel.XlAutoFillType.xlFillWeekdays);

                    tRange = sheet1.Range[sheet1.Cells[dateLength[i] + 3 + i, 1], sheet1.Cells[dateLength[i] + 3 + i, 1]];
                    tRange.AutoFill(sheet1.Range[sheet1.Cells[dateLength[i] + 3 + i, 1], sheet1.Cells[dateLength[i + 1] + i + 2, 1]], Excel.XlAutoFillType.xlFillWeekdays);
                }
            }
            sheet1.Cells[dateLength[2] + 3 + 1, 1] = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dates[2].DayOfWeek);
            sheet1.Cells[dateLength[2] + 3 + 2, 1] = "";
            sheet1.Cells[dateLength[2] + 3 + 1, 2] = dates[2].Date;
            sheet1.Cells[dateLength[2] + 3 + 2, 2] = titlesStart[2];
            sheet1.Cells[dateLength[2] + 3 + 2, 2].EntireRow.Interior.Color = Color.FromArgb(146, 208, 80);
            ((Excel.Range)sheet1.Range[sheet1.Cells[dateLength[2] + 3 + 2, 2], sheet1.Cells[dateLength[2] + 3 + 2, 2]]).Font.Size = 14;
            #endregion

            sheet1.Cells[1, 1].EntireColumn.AutoFit();
            sheet1.Activate();

            return true;
        }
    }
}
