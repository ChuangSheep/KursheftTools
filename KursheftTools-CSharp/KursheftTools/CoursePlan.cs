using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Text;

namespace KursheftTools
{
    class CoursePlan
    {
        private List<Daynotes> Lines;
        private string CourseName { get; }
        private string ClassName { get; }
        private string Teacher { get; }
        public CoursePlan(string CourseName, string ClassName, string Teacher)
        {
            Lines = new List<Daynotes>();
            this.CourseName = CourseName;
            this.ClassName = ClassName;
            this.Teacher = Teacher;
        }

        public void AddLine(Daynotes daynotes)
        {
            Lines.Add(daynotes);
        }

        public bool ExportAsPDF(DateTime[] Periods, string StoredPath,  string LogoFilePath = "default")
        {
            string title = $"{this.CourseName}-{this.Teacher}-{this.ClassName}";
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics xGps = XGraphics.FromPdfPage(page);

            #region Preset Format
            double ROWS = 30;
            //double SMALLWEEKDAYRECTWIDTH = Formats.getPixel(5);
            double SMALLDATERECTWIDTH = Formats.getPixel(30);
            double SMALLNOTERECTWIDTH = Formats.getPixel(55);
            double SMALLRECTHEIGHT = Formats.getPixel(7);
            double TOPHEIGHT = Formats.getPixel(40);
            double LEFTBLANK = Formats.getPixel(10);
            double CENTERBLANK = Formats.getPixel(20);
            double RIGHTBLANK = Formats.getPixel(10);
            double OFFSET = Formats.getPixel(1.5);
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

            if (this.Lines.Count > ROWS * 2)
            {
                ROWS *= 1.3;
                SMALLRECTHEIGHT = Formats.getPixel(6);
            }
            if (this.Lines.Count > ROWS * 2)
            {
                ROWS *= 1.3;
                SMALLRECTHEIGHT = Formats.getPixel(4.5);
                OFFSET = Formats.getPixel(1);

                boldFont = new XFont("Times New Roman", 8, XFontStyle.Bold);
                regularFont = new XFont("Times New Roman", 8);
            }
            //Round it to a whole number
            ROWS = Math.Round(ROWS);
            //If the data is too long or too short
            if (this.Lines.Count > ROWS * 2) throw new ArgumentException("the weeklyplan is too long: " + this.Lines.Count + " : " + this.CourseName + this.ClassName + this.Teacher);
            else if (Lines.Count < 3) throw new ArgumentException("the weeklyplan is too short: " + this.Lines.Count + " course: " + this.CourseName + this.ClassName + this.Teacher);
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
            if (LogoFilePath == "default")
            {
                XRect sTitle = new XRect(0, 0, Formats.getPixel(46), Formats.getPixel(30));
                xGps.DrawString("Sollstuden für Kurs", subtitleFont, XBrushes.Gray, sTitle, XStringFormats.TopLeft);
            }
            else
            {
                XImage lgIcon = XImage.FromFile(LogoFilePath);
                if (lgIcon == null) throw new ArgumentNullException("lgIcon", "the stream of the icon is null");
                xGps.DrawImage(lgIcon, 0, 0, Formats.getPixel(46), Formats.getPixel(30));

                XRect sTitle = new XRect(Formats.getPixel(55), Formats.getPixel(2), Formats.getPixel(100), Formats.getPixel(10));
                xGps.DrawString("Sollstuden für Kurs", subtitleFont, XBrushes.Gray, sTitle, XStringFormats.TopLeft);
            }

            //Set the title
            XRect rectTitle = new XRect(Formats.getPixel(55), Formats.getPixel(10), Formats.getPixel(100), Formats.getPixel(20));
            xGps.DrawString(title, titleFont, XBrushes.Black, rectTitle, XStringFormats.TopLeft);

            //Set the current date and the half year
            DateTime dtNow = DateTime.Now;
            XRect rectDate = new XRect(Formats.getPixel(180), Formats.getPixel(2), Formats.getPixel(28), Formats.getPixel(5));
            xGps.DrawString("Stand: " + dtNow.ToString("dd-MM-yyyy"), regularFont, XBrushes.Gray, rectDate, XStringFormats.TopRight);

            XRect rectYear = new XRect(Formats.getPixel(180), Formats.getPixel(7), Formats.getPixel(28), Formats.getPixel(5));
            xGps.DrawString(DateTimeCalcUtils.GetHalfYear(this.Lines[0].GetDate()), boldFont, XBrushes.Gray, rectYear, XStringFormats.TopRight);
            #endregion

            #region Main Body
            XRect currentLine;
            //The first column
            for (int i = 0; i < this.Lines.Count && i < ROWS; i++)
            {
                //The color
                if (i % 2 != 0) xGps.DrawRectangle(brushes[0], new XRect(new XPoint(smallRectStartCo[0].X, smallRectStartCo[0].Y - OFFSET), new XPoint(smallRectStartCo[1].X + SMALLNOTERECTWIDTH, smallRectStartCo[1].Y + SMALLRECTHEIGHT - OFFSET)));
                else xGps.DrawRectangle(brushes[1], new XRect(new XPoint(smallRectStartCo[0].X, smallRectStartCo[0].Y - OFFSET), new XPoint(smallRectStartCo[1].X + SMALLNOTERECTWIDTH, smallRectStartCo[1].Y + SMALLRECTHEIGHT - OFFSET)));

                currentLine = new XRect(smallRectStartCo[0], new XPoint(LEFTBLANK + SMALLDATERECTWIDTH, smallRectStartCo[0].Y + SMALLRECTHEIGHT));
                xtf.DrawString(Lines[i].GetWeekdayS() + "  " + Lines[i].GetDateS(), boldFont, XBrushes.Black, currentLine, XStringFormats.TopLeft);
                currentLine = new XRect(smallRectStartCo[1], new XPoint(smallRectStartCo[1].X + SMALLNOTERECTWIDTH, smallRectStartCo[1].Y + SMALLRECTHEIGHT));
                xtf.DrawString(Lines[i].GetNotes(), regularFont, XBrushes.Black, currentLine, XStringFormats.TopLeft);

                smallRectStartCo[0].Y += SMALLRECTHEIGHT;
                smallRectStartCo[1].Y += SMALLRECTHEIGHT;
            }
            //The second column
            if (this.Lines.Count > ROWS)
            {
                for (int i = 0; i < Lines.Count - ROWS && i < ROWS; i++)
                {
                    //The color
                    if (i % 2 != 0) xGps.DrawRectangle(brushes[0], new XRect(new XPoint(smallRectStartCo[2].X, smallRectStartCo[2].Y - OFFSET), new XPoint(smallRectStartCo[3].X + SMALLNOTERECTWIDTH, smallRectStartCo[3].Y + SMALLRECTHEIGHT - OFFSET)));
                    else xGps.DrawRectangle(brushes[1], new XRect(new XPoint(smallRectStartCo[2].X, smallRectStartCo[2].Y - OFFSET), new XPoint(smallRectStartCo[3].X + SMALLNOTERECTWIDTH, smallRectStartCo[3].Y + SMALLRECTHEIGHT - OFFSET)));

                    currentLine = new XRect(smallRectStartCo[2], new XPoint(smallRectStartCo[3].X, smallRectStartCo[2].Y + SMALLRECTHEIGHT));
                    xtf.DrawString(Lines[i + (int)ROWS].GetWeekdayS() + "  " + Lines[i].GetDateS(), boldFont, XBrushes.Black, currentLine, XStringFormats.TopLeft);
                    currentLine = new XRect(smallRectStartCo[3], new XPoint(smallRectStartCo[3].X + SMALLNOTERECTWIDTH, smallRectStartCo[3].Y + SMALLRECTHEIGHT));
                    xtf.DrawString(Lines[i + (int)ROWS].GetNotes(), regularFont, XBrushes.Black, currentLine, XStringFormats.TopLeft);

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
            if (this.Lines.Count - ROWS > 0)
            {
                rect = new XRect(smallRectStartCo[2].X, smallRectStartCo[2].Y + SMALLRECTHEIGHT, SMALLDATERECTWIDTH + SMALLNOTERECTWIDTH, Formats.A4.pixelHeight - (smallRectStartCo[2].Y + SMALLRECTHEIGHT));
                xtf.DrawString(OUTPUT, smallNoteFont, XBrushes.Black, rect, XStringFormats.TopLeft);
            }
            else
            {
                rect = new XRect(smallRectStartCo[2].X, smallRectStartCo[2].Y, SMALLDATERECTWIDTH + SMALLNOTERECTWIDTH, Formats.A4.pixelHeight - (smallRectStartCo[2].Y + SMALLRECTHEIGHT));
                xtf.DrawString(OUTPUT, smallNoteFont, XBrushes.Black, rect, XStringFormats.TopLeft);
            }

            rect = new XRect(smallRectStartCo[0].X, smallRectStartCo[0].Y + Formats.getPixel(1), SMALLNOTERECTWIDTH + SMALLDATERECTWIDTH, Formats.A4.pixelHeight - (smallRectStartCo[0].Y + SMALLRECTHEIGHT / 2));
            xGps.DrawString($"1. Kursabschnitt: {Periods[0].ToString("dd-MM-yyyy")} - {Periods[1].AddDays(-17).ToString("dd-MM-yyyy")}", regularFont, XBrushes.Black, rect, XStringFormats.TopLeft);
            smallRectStartCo[0].Y += SMALLRECTHEIGHT / 2 + Formats.getPixel(2);
            rect = new XRect(smallRectStartCo[0].X, smallRectStartCo[0].Y + Formats.getPixel(1), SMALLNOTERECTWIDTH + SMALLDATERECTWIDTH, Formats.A4.pixelHeight - (smallRectStartCo[0].Y + SMALLRECTHEIGHT / 2));
            xGps.DrawString($"2. Kursabschnitt: {Periods[1].ToString("dd-MM-yyyy")} - {Periods[2].ToString("dd-MM-yyyy")}", regularFont, XBrushes.Black, rect, XStringFormats.TopLeft);
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

            document.Save($"{StoredPath}\\{title}.pdf");
            document.Close();
            document.Dispose();
            GC.Collect();

            System.Diagnostics.Debug.WriteLine($"{StoredPath}\\{title}.pdf ist exportiert.");
            #endregion
            return true;
        }

        /// <summary>
        /// ONLY FOR DEBUG USAGE
        /// Print all the daynotes in this CoursePlan object to the console
        /// </summary>
        public void PrintString()
        {
            Debug.WriteLine($"Course: {this.CourseName}, Class: {this.ClassName}, Teacher: {this.Teacher}");
            foreach (Daynotes daynotes in Lines)
            {
                Debug.WriteLine($"{daynotes.GetWeekdayS()}  {daynotes.GetDateS()}: {daynotes.GetNotes()}");
            }
        }
    }
}
