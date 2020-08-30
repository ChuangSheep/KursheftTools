using System;
using System.Collections.Generic;

namespace KursheftTools
{
    class Daynote
    {
        private readonly DateTime _date;
        private readonly List<string> _notes;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dt">A DateTime object represents the date</param>
        public Daynote(DateTime dt)
        {
            _date = dt;
            _notes = new List<string>();
        }

        /// <summary>
        /// Add a note to this object
        /// </summary>
        /// <param name="note">A string represents the note that needs to be added</param>
        public void AddNote(string note)
        {
            if (!string.IsNullOrEmpty(note)) _notes.Add(note);
        }

        /// <summary>
        /// Get the weekday of this Daynote object as string
        /// </summary>
        /// <returns>A string represents on which weekday this Daynote object is</returns>
        public string GetWeekdayS()
        {
            return DateTimeCalcUtils.GetWeekday(_date);
        }

        /// <summary>
        /// Get the date of this Daynote object as string
        /// </summary>
        /// <returns>A string in "dd-MM-yyyy" format</returns>
        public string GetDateS()
        {
            return _date.ToString("dd.MM.yyyy", new System.Globalization.CultureInfo("de-DE"));
        }

        /// <summary>
        /// Get the date of this Daynote object as DateTime
        /// </summary>
        /// <returns>A DateTime object</returns>
        public DateTime GetDate()
        {
            return this._date;
        }

        /// <summary>
        /// Get all the notes of this Daynote object seperated by ';'
        /// </summary>
        /// <returns>A string of all the notes of this Daynote object</returns>
        public string GetNotes()
        {
            string cache = "";
            foreach (string s in _notes)
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
