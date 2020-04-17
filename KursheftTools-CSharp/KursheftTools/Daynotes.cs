using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursheftTools
{
    class Daynotes
    {
        private DateTime Date { get; }
        private List<string> Notes { get; }

        public Daynotes(DateTime dt)
        {
            Date = dt;
            Notes = new List<string>();
        }


        public void AddNote(string note)
        {
            if (note != "") Notes.Add(note);
        }

        public string GetWeekdayS()
        {
            return DateTimeCalcUtils.GetWeekday(Date);
        }
        public string GetDateS()
        {
            return Date.ToString("dd-MM-yyyy");
        }
        public DateTime GetDate()
        {
            return this.Date;
        }
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
