using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace KursheftTools
{
    public partial class FormForGeneratingNoteBoard : Form
    {
        private static readonly CultureInfo DEUTSCHCULT = new CultureInfo("de-DE");
        public FormForGeneratingNoteBoard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// If the accept is clicked, check the given values
        /// If these are valid, start to create the note board
        /// Otherwise, mark the wrong field as red
        /// </summary>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Color RED = Color.FromArgb(255, 192, 192);
            Color WHITE = Color.White;
            DateTime[] periods = new DateTime[4];
            DateTime[,] holidays = new DateTime[2, 2];
            TextBox[] textboxes = new TextBox[4] { StartPeriod1, EndPeriod1, StartPeriod2, EndHY };
            TextBox[] textBoxHolidays = new TextBox[2] { Holiday1, Holiday2 };

            foreach (TextBox tbx in textboxes)
            {

                // Reset the color
                tbx.BackColor = WHITE;

                // Weekends are not allowed as start or end of a Kursabschnitt
                try
                {
                    periods[Array.IndexOf(textboxes, tbx)] = DateTime.ParseExact(tbx.Text, "dd.MM.yyyy", DEUTSCHCULT);
                    if (DateTimeCalcUtils.GetWeekday(periods[Array.IndexOf(textboxes, tbx)]) == "Sa" ||
                        DateTimeCalcUtils.GetWeekday(periods[Array.IndexOf(textboxes, tbx)]) == "So")
                    {
                        DialogResult = DialogResult.None;
                        tbx.BackColor = RED;
                    }
                }
                catch (FormatException)
                {
                    DialogResult = DialogResult.None;
                    periods[Array.IndexOf(textboxes, tbx)] = new DateTime();
                    tbx.BackColor = RED;
                }
            }

            for (byte i = 0; i < textBoxHolidays.Length; i++)
            {
                string[] prds = textBoxHolidays[i].Text.Split('-');
                try
                {
                    holidays[i, 0] = DateTime.ParseExact(prds[0], "dd.MM.yyyy", DEUTSCHCULT);
                    holidays[i, 1] = DateTime.ParseExact(prds[1], "dd.MM.yyyy", DEUTSCHCULT);
                }
                catch (FormatException)
                {
                    DialogResult = DialogResult.None;
                    holidays[i, 0] = new DateTime();
                    holidays[i, 1] = new DateTime();
                    textBoxHolidays[i].BackColor = RED;
                }

            }

            for (byte i = 0; i < textboxes.Length - 1; i++)
            {
                if (periods[i + 1] < periods[i])
                {
                    DialogResult = DialogResult.None;
                }
            }

            if (DialogResult == DialogResult.Yes)
            {
                this.Hide();
                if (CreateBoard(periods, holidays)) MessageBox.Show("Ein leerer Bemerkungsbogen wurde erfolgreich generiert.\r\nAbschnitts: " +
                    $"{periods[0]:dd.MM.yyyy} ~ {periods[1]:dd.MM.yyyy} | {periods[2]:dd.MM.yyyy} ~ {periods[3]:dd.MM.yyyy}",
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
            MessageBox.Show("Die Datum soll in dieser Form \'dd.mm.yyyy\' angegeben werden",
                "Hilfe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Create a note board based on the given date periods
        /// </summary>
        /// <param name="dates">An array of DateTime objects represents the start of year, of the second period and the end of the half year. </param>
        /// <returns>A boolean value represents whether this operation finished successfully or not</returns>
        private bool CreateBoard(DateTime[] dates, DateTime[,] holidays)
        {
            if (dates.Length != 4) throw new ArgumentException("The augument \"dates\" (array of DateTime) must have 4 items", nameof(dates));
            if (holidays.Length != 4) throw new ArgumentException("The augument \"holidays\" (array of DateTime) must have 4 items", nameof(holidays));

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
            sheet1.Cells[1, 9] = dates[0].ToString("dd.MM.yyyy", DEUTSCHCULT) + "~" + dates[1].ToString("dd.MM.yyyy", DEUTSCHCULT) + "~" + dates[2].ToString("dd.MM.yyyy", DEUTSCHCULT) + "~" + dates[3].ToString("dd.MM.yyyy", DEUTSCHCULT);
            sheet1.Cells[2, 9] = $"{holidays[0, 0]:dd.MM.yyyy}~{holidays[0, 1]:dd.MM.yyyy}";
            sheet1.Cells[3, 9] = $"{holidays[1, 0]:dd.MM.yyyy}~{holidays[1, 1]:dd.MM.yyyy}";
            sheet1.Cells[1, 1].EntireRow.Font.Bold = true;
            sheet1.Cells[1, 1].EntireRow.Font.Size = 14;

            sheet1.Application.ActiveWindow.SplitRow = 1;
            sheet1.Application.ActiveWindow.FreezePanes = true;


            #region 1. + 2. column: Weekdays and Dates
            int[] dateLength = new int[3] { 0, DateTimeCalcUtils.BusinessDaysUntil(dates[0], dates[1]), DateTimeCalcUtils.BusinessDaysUntil(dates[0], dates[1]) + DateTimeCalcUtils.BusinessDaysUntil(dates[2], dates[3]) - 1 };

            DateTime[] startDates = new DateTime[3] { dates[0], dates[2], dates[3] };
            string[] titlesStart = new string[3] { "Anfang d. 1. Abschnitts", "Anfang d. 2. Abschnitts", "Ende des Schulhalbjahres" };
            sheet1.get_Range("B:B", System.Type.Missing).NumberFormat = "dd.MM.yyyy";  //It MAY cause a problem

            for (int i = 0; i < dateLength.Length; i++)
            {
                tRange = sheet1.Range[sheet1.Cells[dateLength[i] + 3 + i, 1], sheet1.Cells[dateLength[i] + 3 + i, 1]];
                tRange.Value = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(startDates[i].DayOfWeek);


                tRange = sheet1.Range[sheet1.Cells[dateLength[i] + 3 + i, 2], sheet1.Cells[dateLength[i] + 3 + i, 2]];
                tRange.Value = startDates[i].Date;

                if (i < 2)
                {
                    sheet1.Cells[dateLength[i] + 2 + i, 2] = titlesStart[i];
                    sheet1.Cells[dateLength[i] + 2 + i, 2].EntireRow.Interior.Color = Color.FromArgb(146, 208, 80);
                    ((Excel.Range)sheet1.Range[sheet1.Cells[dateLength[i] + 2 + i, 2], sheet1.Cells[dateLength[i] + 2 + i, 2]]).Font.Size = 14;
                    tRange.AutoFill(sheet1.Range[sheet1.Cells[dateLength[i] + 3 + i, 2], sheet1.Cells[dateLength[i + 1] + i + 2, 2]], Excel.XlAutoFillType.xlFillWeekdays);

                    tRange = sheet1.Range[sheet1.Cells[dateLength[i] + 3 + i, 1], sheet1.Cells[dateLength[i] + 3 + i, 1]];
                    tRange.AutoFill(sheet1.Range[sheet1.Cells[dateLength[i] + 3 + i, 1], sheet1.Cells[dateLength[i + 1] + i + 2, 1]], Excel.XlAutoFillType.xlFillWeekdays);
                }
            }
            sheet1.Cells[dateLength[2] + 3 + 1, 1] = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dates[3].DayOfWeek);
            sheet1.Cells[dateLength[2] + 3 + 2, 1] = "";
            sheet1.Cells[dateLength[2] + 3 + 1, 2] = dates[3].Date;
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
