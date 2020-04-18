using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursheftTools
{
    class Daynote
    {
        private DateTime Date { get; }
        private List<string> Notes { get; }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dt">A DateTime object represents the date</param>
        public Daynote(DateTime dt)
        {
            Date = dt;
            Notes = new List<string>();
        }

        /// <summary>
        /// Add a note to this object
        /// </summary>
        /// <param name="note">A string represents the note that needs to be added</param>
        public void AddNote(string note)
        {
            if (note != "") Notes.Add(note);
        }

        /// <summary>
        /// Get the weekday of this Daynote object as string
        /// </summary>
        /// <returns>A string represents on which weekday this Daynote object is</returns>
        public string GetWeekdayS()
        {
            return DateTimeCalcUtils.GetWeekday(Date);
        }

        /// <summary>
        /// Get the date of this Daynote object as string
        /// </summary>
        /// <returns>A string in "dd-MM-yyyy" format</returns>
        public string GetDateS()
        {
            return Date.ToString("dd-MM-yyyy");
        }

        /// <summary>
        /// Get the date of this Daynote object as DateTime
        /// </summary>
        /// <returns>A DateTime object</returns>
        public DateTime GetDate()
        {
            return this.Date;
        }

        /// <summary>
        /// Get all the notes of this Daynote object seperated by ';'
        /// </summary>
        /// <returns>A string of all the notes of this Daynote object</returns>
        public string GetNotes()
        {
            string cache = "";
            foreach (string s in Notes)
            {
                cache += s + "; ";
            }

            return cache;
        }

        /// <summary>
        /// ONLY FOR DEBUG USAGE
        /// Get the string form of this daynotes object
        /// </summary>
        public override string ToString()
        {
            return $"{GetWeekdayS()}  {GetDateS()}: {GetNotes()}";
        }

    }
}
