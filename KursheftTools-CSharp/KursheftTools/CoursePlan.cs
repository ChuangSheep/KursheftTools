using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace KursheftTools
{
    class CoursePlan
    {
        /// <summary>
        /// A list of Daynotes objects contains all the notes for this course in this half year
        /// </summary>
        private readonly List<Daynote> _lines;

        private readonly string _courseName;
        private readonly string _className;
        private readonly string _teacher;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="courseName">The name of this course as string</param>
        /// <param name="className">The name of the class of the course as string</param>
        /// <param name="teacher">The name of the teacher as string</param>
        public CoursePlan(string courseName, string className, string teacher)
        {
            _lines = new List<Daynote>();
            this._courseName = courseName;
            this._className = className;
            this._teacher = teacher;
        }

        /// <summary>
        /// Add a daynote object to this course plan
        /// </summary>
        /// <param name="daynotes">A Daynotes object contains all the notes on one day</param>
        public void AddLine(Daynote daynotes)
        {
            _lines.Add(daynotes);
        }

        /// <summary>
        /// Get the title of this course with .pdf
        /// </summary>
        /// <returns>A string represents the title of this course. </returns>
        public string GetTitle()
        {
            return $"{this._courseName}-{this._teacher}-{this._className}.pdf";
        }
        /// <summary>
        /// Get the grade of this course
        /// </summary>
        /// <returns>The grade of this course as string</returns>
        public string GetGrade()
        {
            return _className.Length == 2 ? _className : _className.Substring(0, 2);
        }

        /// <summary>
        /// Export this course to the given path as pdf
        /// </summary>
        /// <param name="periods">An array of DateTime contains 3 items representing the start of the year, 
        ///                     the start of the second period and the end of the half year.</param>
        /// <param name="storedPath">The path where the exported pdf should be stored.
        ///                     This path does NOT contain the name of this pdf file.
        ///                     The name will be like this format:
        ///                     CourseName-Teacher-Class.pdf</param>
        /// <param name="logoFilePath">A optional parameter represents where the logo is stored. 
        ///                     If this is not given, then the logo will be replaced by the words "Sollstuden für Kurs"</param>
        /// <returns>A boolean value represents whether the export is successful or not.</returns>
        public bool ExportAsPDF(DateTime[] periods, string storedPath,  string logoFilePath = "default")
        {
            string title = $"{this._courseName}-{this._teacher}-{this._className}";
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics xGps = XGraphics.FromPdfPage(page);

            #region Preset Format
            double ROWS = 30;
            //double SMALLWEEKDAYRECTWIDTH = Formats.getPixel(5);
            double SMALLDATERECTWIDTH = Formats.GetPixel(30);
            double SMALLNOTERECTWIDTH = Formats.GetPixel(55);
            double SMALLRECTHEIGHT = Formats.GetPixel(7);
            double TOPHEIGHT = Formats.GetPixel(40);
            double LEFTBLANK = Formats.GetPixel(10);
            double CENTERBLANK = Formats.GetPixel(20);
            //double RIGHTBLANK = Formats.getPixel(10);
            double OFFSET = Formats.GetPixel(1.5);
            //IN PIXEL
            XPoint[] smallRectStartCo = new XPoint[4] { new XPoint(), new XPoint(), new XPoint(), new XPoint() };

            #endregion

            #region Preset Text Format 
            XFont boldFont = new XFont("Times New Roman", 10, XFontStyle.Bold);
            XFont regularFont = new XFont("Times New Roman", 10);
            XFont titleFont = new XFont("Times New Roman", 24, XFontStyle.Bold);
            XFont subtitleFont = new XFont("Times New Roman", 16, XFontStyle.Bold);
            XFont smallNoteFont = new XFont("Times New Roman", 8);
            XTextFormatter xtf = new XTextFormatter(xGps);
            XBrush[] brushes = new XBrush[2] { new XSolidBrush(XColor.FromArgb(230, 230, 230)), new XSolidBrush(XColor.FromArgb(210, 210, 210)) };
            #endregion

            #region Responsive Design & Error check

            if (this._lines.Count > ROWS * 2)
            {
                ROWS *= 1.3;
                SMALLRECTHEIGHT = Formats.GetPixel(6);
            }
            if (this._lines.Count > ROWS * 2)
            {
                ROWS *= 1.3;
                SMALLRECTHEIGHT = Formats.GetPixel(4.5);
                OFFSET = Formats.GetPixel(1);

                boldFont = new XFont("Times New Roman", 8, XFontStyle.Bold);
                regularFont = new XFont("Times New Roman", 8);
            }
            //Round it to a whole number
            ROWS = Math.Round(ROWS);
            //If the data is too long or too short
            if (this._lines.Count > ROWS * 2) throw new ArgumentException("the weeklyplan is too long: " + this._lines.Count + " : " + this._courseName + this._className + this._teacher);
            else if (_lines.Count < 3) throw new ArgumentException("the weeklyplan is too short: " + this._lines.Count + " course: " + this._courseName + this._className + this._teacher);
            #endregion

            #region Points

            smallRectStartCo[0].X = LEFTBLANK;
            smallRectStartCo[0].Y = TOPHEIGHT;
            smallRectStartCo[1].X = LEFTBLANK + SMALLDATERECTWIDTH;
            smallRectStartCo[1].Y = TOPHEIGHT;
            smallRectStartCo[2].X = LEFTBLANK + SMALLDATERECTWIDTH + SMALLNOTERECTWIDTH + CENTERBLANK;
            smallRectStartCo[2].Y = TOPHEIGHT;
            smallRectStartCo[3].X = LEFTBLANK + CENTERBLANK + 2 * SMALLDATERECTWIDTH + SMALLNOTERECTWIDTH;
            smallRectStartCo[3].Y = TOPHEIGHT;

            #endregion

            #region HEAD
            //Set the Logo
            if (logoFilePath == "default")
            {
                XRect rectSubtitle = new XRect(0, 0, Formats.GetPixel(46), Formats.GetPixel(30));
                xGps.DrawString("Sollstuden für Kurs", subtitleFont, XBrushes.Gray, rectSubtitle, XStringFormats.TopLeft);
            }
            else
            {
                XImage lgIcon = XImage.FromFile(logoFilePath);
                if (lgIcon == null) throw new ArgumentNullException("lgIcon", "the stream of the icon is null");
                xGps.DrawImage(lgIcon, 0, 0, Formats.GetPixel(46), Formats.GetPixel(30));

                XRect rectSubtitle = new XRect(Formats.GetPixel(55), Formats.GetPixel(2), Formats.GetPixel(100), Formats.GetPixel(10));
                xGps.DrawString("Sollstuden für Kurs", subtitleFont, XBrushes.Gray, rectSubtitle, XStringFormats.TopLeft);
            }

            //Set the title
            XRect rectTitle = new XRect(Formats.GetPixel(55), Formats.GetPixel(10), Formats.GetPixel(100), Formats.GetPixel(20));
            xGps.DrawString(title, titleFont, XBrushes.Black, rectTitle, XStringFormats.TopLeft);

            //Set the current date and the half year
            DateTime dtNow = DateTime.Now;
            XRect rectDate = new XRect(Formats.GetPixel(180), Formats.GetPixel(2), Formats.GetPixel(28), Formats.GetPixel(5));
            xGps.DrawString("Stand: " + dtNow.ToString("dd-MM-yyyy"), regularFont, XBrushes.Gray, rectDate, XStringFormats.TopRight);

            XRect rectYear = new XRect(Formats.GetPixel(180), Formats.GetPixel(7), Formats.GetPixel(28), Formats.GetPixel(5));
            xGps.DrawString(DateTimeCalcUtils.GetHalfYear(this._lines[0].GetDate()), boldFont, XBrushes.Gray, rectYear, XStringFormats.TopRight);
            #endregion

            #region Main Body

            XRect rectCurrentLine;
            //The first column
            for (int i = 0; i < this._lines.Count && i < ROWS; i++)
            {
                //The color
                if (i % 2 != 0) xGps.DrawRectangle(brushes[0], new XRect(new XPoint(smallRectStartCo[0].X, smallRectStartCo[0].Y - OFFSET), new XPoint(smallRectStartCo[1].X + SMALLNOTERECTWIDTH, smallRectStartCo[1].Y + SMALLRECTHEIGHT - OFFSET)));
                else xGps.DrawRectangle(brushes[1], new XRect(new XPoint(smallRectStartCo[0].X, smallRectStartCo[0].Y - OFFSET), new XPoint(smallRectStartCo[1].X + SMALLNOTERECTWIDTH, smallRectStartCo[1].Y + SMALLRECTHEIGHT - OFFSET)));

                rectCurrentLine = new XRect(smallRectStartCo[0], new XPoint(LEFTBLANK + SMALLDATERECTWIDTH, smallRectStartCo[0].Y + SMALLRECTHEIGHT));
                xtf.DrawString(_lines[i].GetWeekdayS() + "  " + _lines[i].GetDateS(), boldFont, XBrushes.Black, rectCurrentLine, XStringFormats.TopLeft);
                rectCurrentLine = new XRect(smallRectStartCo[1], new XPoint(smallRectStartCo[1].X + SMALLNOTERECTWIDTH, smallRectStartCo[1].Y + SMALLRECTHEIGHT));
                xtf.DrawString(_lines[i].GetNotes(), regularFont, XBrushes.Black, rectCurrentLine, XStringFormats.TopLeft);

                smallRectStartCo[0].Y += SMALLRECTHEIGHT;
                smallRectStartCo[1].Y += SMALLRECTHEIGHT;
            }
            //The second column if exists
            if (this._lines.Count > ROWS)
            {
                for (int i = 0; i < _lines.Count - ROWS && i < ROWS; i++)
                {
                    //The color
                    if (i % 2 != 0) xGps.DrawRectangle(brushes[0], new XRect(new XPoint(smallRectStartCo[2].X, smallRectStartCo[2].Y - OFFSET), new XPoint(smallRectStartCo[3].X + SMALLNOTERECTWIDTH, smallRectStartCo[3].Y + SMALLRECTHEIGHT - OFFSET)));
                    else xGps.DrawRectangle(brushes[1], new XRect(new XPoint(smallRectStartCo[2].X, smallRectStartCo[2].Y - OFFSET), new XPoint(smallRectStartCo[3].X + SMALLNOTERECTWIDTH, smallRectStartCo[3].Y + SMALLRECTHEIGHT - OFFSET)));

                    rectCurrentLine = new XRect(smallRectStartCo[2], new XPoint(smallRectStartCo[3].X, smallRectStartCo[2].Y + SMALLRECTHEIGHT));
                    xtf.DrawString(_lines[i + (int)ROWS].GetWeekdayS() + "  " + _lines[i + (int)ROWS].GetDateS(), boldFont, XBrushes.Black, rectCurrentLine, XStringFormats.TopLeft);
                    rectCurrentLine = new XRect(smallRectStartCo[3], new XPoint(smallRectStartCo[3].X + SMALLNOTERECTWIDTH, smallRectStartCo[3].Y + SMALLRECTHEIGHT));
                    xtf.DrawString(_lines[i + (int)ROWS].GetNotes(), regularFont, XBrushes.Black, rectCurrentLine, XStringFormats.TopLeft);

                    smallRectStartCo[2].Y += SMALLRECTHEIGHT;
                    smallRectStartCo[3].Y += SMALLRECTHEIGHT;
                }
            }
            #endregion

            #region Bottom

            XRect rect;
            const string INFO1 = "Alle Termine dieser Liste müssen in der Kursmappe eingetragen sein," +
                            "auch die unterrichtsfreien Tage. Alle Termine(außer den Ferien)" +
                            "müssen durch ihre Paraphe bestätigt werden. Tragen Sie bitte auch" +
                            "die Fehlstundenzahl sowie die Soll - Ist - Stunden(Kursheft S.5) ein. ";
            const string INFO2 = "Hinweis: Schüler, die aus schulischen Gründen den Unterricht " +
                                "versäumt haben(Klausur, schul.Veranstaltung usw.) müssen im " +
                                "Kursheft aufgeführt werden. Diese Stunden dürfen auf dem " +
                                "Zeugnis aber nicht als Fehlstunden vermerkt werden.";
            string OUTPUT = INFO1 + "\r\n\r\n" + INFO2;

            //If there is no lines at the second column, put the info direct at the second line. 
            if (this._lines.Count - ROWS > 0)
            {
                rect = new XRect(smallRectStartCo[2].X, smallRectStartCo[2].Y + SMALLRECTHEIGHT, SMALLDATERECTWIDTH + SMALLNOTERECTWIDTH, Formats.A4.pixelHeight - (smallRectStartCo[2].Y + SMALLRECTHEIGHT));
                xtf.DrawString(OUTPUT, smallNoteFont, XBrushes.Black, rect, XStringFormats.TopLeft);
            }
            //Otherwise, add spaces
            else
            {
                rect = new XRect(smallRectStartCo[2].X, smallRectStartCo[2].Y, SMALLDATERECTWIDTH + SMALLNOTERECTWIDTH, Formats.A4.pixelHeight - (smallRectStartCo[2].Y + SMALLRECTHEIGHT));
                xtf.DrawString(OUTPUT, smallNoteFont, XBrushes.Black, rect, XStringFormats.TopLeft);
            }

            rect = new XRect(smallRectStartCo[0].X, smallRectStartCo[0].Y + Formats.GetPixel(1), SMALLNOTERECTWIDTH + SMALLDATERECTWIDTH, Formats.A4.pixelHeight - (smallRectStartCo[0].Y + SMALLRECTHEIGHT / 2));
            xGps.DrawString($"1. Kursabschnitt: {periods[0]:dd-MM-yyyy} - {periods[1].AddDays(-17):dd-MM-yyyy}", regularFont, XBrushes.Black, rect, XStringFormats.TopLeft);
            smallRectStartCo[0].Y += SMALLRECTHEIGHT / 2 + Formats.GetPixel(2);
            rect = new XRect(smallRectStartCo[0].X, smallRectStartCo[0].Y + Formats.GetPixel(1), SMALLNOTERECTWIDTH + SMALLDATERECTWIDTH, Formats.A4.pixelHeight - (smallRectStartCo[0].Y + SMALLRECTHEIGHT / 2));
            xGps.DrawString($"2. Kursabschnitt: {periods[1]:dd-MM-yyyy} - {periods[2]:dd-MM-yyyy}", regularFont, XBrushes.Black, rect, XStringFormats.TopLeft);
            #endregion

            #region Clean Up

            //Test illegal character to avoid the error when saving the document
            foreach (char c in title.ToCharArray())
            {
                //Test if there is illegal character in the title
                if (!((c >= 97 && c <= 123) || (c >= 48 && c <= 57) || (c >= 65 && c <= 90) || 
                        c == 45 || c == 46 || c == 64 || c == 95 || c == 32 || c == 'ä' || c == 'ö' || c == 'ü' ||
                        c == 'ß' || c == 'Ä' || c == 'Ö' || c == 'Ü'))
                {
                    //Replace it with .
                    title = title.Replace(c, '.');
                }
            }

            document.Save($"{storedPath}\\{title}.pdf");
            document.Close();
            document.Dispose();
            GC.Collect();

            Debug.WriteLine($"{storedPath}\\{title}.pdf ist exportiert.");
            #endregion
            return true;
        }

        /// <summary>
        /// Read all the note from the given Worksheet object, create the Daynotes objects and add them to this CoursePlan object. 
        /// </summary>
        /// <param name="noteBoard">A excel worksheet object represents the note board. </param>
        /// <param name="dates">An array of DateTime objects contains all the start time of this course </param>
        /// <param name="isRegular">An array of string represents this course appears every week or every two weeks. 
        ///                 "" represents regular, "g" represents only on even weeks and "u" only on odd weeks. 
        ///                 The order of this array should be the same as the order of the array "dates". </param>
        public void ReadNoteBoard(Excel.Worksheet noteBoard, DateTime[] dates, string[] isRegular)
        {
            DateTimeCalcUtils.SortDate(ref dates, ref isRegular);
            //Initialize the counter
            int k = 0;
            //Traverse the note board
            for (int i = 3; i < noteBoard.UsedRange.Rows.Count - 1; i++)
            {
                //Jump the title lines
                if (((Excel.Range)noteBoard.Cells[i, 2]).Text == "Anfang d. 2. Abschnitts")
                {
                    //Jump the holiday
                    for (int n = 0; n < dates.Length; n++)
                    {
                        dates[n] = dates[n].AddDays(14);
                    }
                    //To the next row of the note board
                    continue;
                }
                //Jump the title lines
                else if (((Excel.Range)noteBoard.Cells[i, 2]).Text == "Ende des Schuljahres" || ((Excel.Range)noteBoard.Cells[i, 2]).Text == "") continue;

                DateTime lineDate = ((Excel.Range)noteBoard.Cells[i, 2]).Value;

                //If the date fits
                if (DateTime.Compare(lineDate, dates[k]) == 0)
                {
                    //If this weekday is regular, or it fits to the rule
                    if (isRegular[k] == "" ||
                        (DateTimeCalcUtils.IsEvenWeek(lineDate) && isRegular[k] == "g") ||
                        (!DateTimeCalcUtils.IsEvenWeek(lineDate) && isRegular[k] == "u"))
                    {
                        Daynote currentDaynotes = new Daynote(lineDate);

                        //Go through the three notes on the same row
                        for (int j = 3; j < 8; j += 2)
                        {
                            string currentNote = ((Excel.Range)noteBoard.Cells[i, j]).Value;
                            string currentLineGrade = ((Excel.Range)noteBoard.Cells[i, j + 1]).Value;

                            //When the note is not empty
                            if (currentNote != null)
                            {
                                //If the grade fits to the current course
                                if (currentLineGrade == this._className || currentLineGrade == this.GetGrade() || currentLineGrade == null)
                                {
                                    currentDaynotes.AddNote(currentNote);
                                }
                            }
                        }
                        //Add the daynote to the plan
                        this.AddLine(currentDaynotes);
                    }

                    dates[k] = dates[k].AddDays(7);

                    //Move the counter of the DatesA array
                    if (k < dates.Length - 1) k++;
                    else k = 0;
                }

            }
        }


        /// <summary>
        /// ONLY FOR DEBUG
        /// Print all the daynotes in this CoursePlan object to the console
        /// </summary>
        public void PrintString()
        {
            Debug.WriteLine($"Course: {this._courseName}, Class: {this._className}, Teacher: {this._teacher}");
            foreach (Daynote daynotes in _lines)
            {
                Debug.WriteLine($"{daynotes.GetWeekdayS()}  {daynotes.GetDateS()}: {daynotes.GetNotes()}");
            }
        }
    }
}
