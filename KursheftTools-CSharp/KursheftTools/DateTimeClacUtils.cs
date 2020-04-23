using System;
using System.Globalization;

namespace KursheftTools
{
    public static class DateTimeCalcUtils
    {

        /// <summary>
        /// get the week number in a year based on a date
        /// </summary>
        /// <param name="date">the date in this week</param>
        /// <returns></returns>
        public static int GetWeekOfYear(this DateTime date)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }


        /// <summary>
        /// Calculates the next weekday after the start date
        /// </summary>
        /// <param name="start">The start date</param>
        /// <param name="weekday">the weekday in short form: ex. Mo, Di etc. </param>
        /// <returns></returns>
        public static DateTime GetNearestWeekdayS(DateTime start, string weekday)
        {
            var culture = new CultureInfo("de-DE");

            if (culture.DateTimeFormat.GetDayName(start.DayOfWeek).Substring(0, 2) == weekday) return start;

            for (int i = 0; i < 7; i++)
            {
                start = start.AddDays(1);

                if (culture.DateTimeFormat.GetDayName(start.DayOfWeek).Substring(0, 2) == weekday)
                {
                    return start;
                }
            }

            //For every start date there should be a returned value
            //The following exception is not excepted to be thrown
            throw new ArithmeticException("A not expected error occured at dateTimeCalc.GetNearestWeekday");
        }


        /// <summary>
        /// get the short form of a weekday from an integer
        /// The format is De-DE
        /// </summary>
        /// <param name="weekday">An integer representing a weekday
        /// Monday is represented as 1, Sunday as 7</param>
        /// <returns>A short form of the weekday in string(ex. Mo, Di)</returns>
        /// <exception cref="ArgumentException">If the inputed string is not between 1 and 7, raise the argument exception</exception>
        public static string GetWeekdayFromNumber(int weekday)
        {
            switch (weekday)
            {
                case 1: return "Mo";
                case 2: return "Di";
                case 3: return "Mi";
                case 4: return "Do";
                case 5: return "Fr";
                case 6: return "Sa";
                case 7: return "So";
                default: throw new ArgumentException("The inputed argument weekday is not an Integer between 1-7", "weekday");
            }
        }

        /// <summary>
        /// Get the weekday in short form of the given DateTime object of culture de-DE
        /// </summary>
        /// <param name="dt">The date</param>
        /// <returns>A string represents the short form of the given date. </returns>
        public static string GetWeekday(DateTime dt)
        {
            return dt.ToString("ddd", new CultureInfo("de-DE")).Substring(0, 2);
        }

        /// <summary>
        /// Indicates whether the given date is in even week or not. 
        /// </summary>
        /// <param name="dt">The date</param>
        /// <returns>A boolean value represents whether the given date is in even week or not</returns>
        public static bool IsEvenWeek(DateTime dt)
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Calendar calendar = culture.Calendar;
            CalendarWeekRule calendarWeekRule = culture.DateTimeFormat.CalendarWeekRule;
            DayOfWeek FirstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

            return calendar.GetWeekOfYear(dt, calendarWeekRule, FirstDayOfWeek) % 2 == 0;
        }

        /// <summary>
        /// Sort the given date array from early to late
        /// This function WILL CHANGE THE ORIGIN GIVEN PARAMETER as it is ref
        /// </summary>
        /// <typeparam name="T">A general type, such as string</typeparam>
        /// <param name="dateArr">The date array that needs to be sorted</param>
        /// <param name="associatedArr">Another array. The order of items in this array will be changed if the dateArr is changed.</param>
        public static void SortDate <T> (ref DateTime[] dateArr, ref T[] associatedArr)
        {
            if (dateArr.Length != associatedArr.Length) throw new ArgumentException("The given two arrays dont have the same numbers of items", "associatedArr");
            for (int i = 0; i < dateArr.Length - 1; i++)
            {
                for (int j = 0; j < dateArr.Length - 1; j++)
                {
                    if (DateTime.Compare(dateArr[j], dateArr[j + 1]) > 0)
                    {
                        ArrSwap(ref dateArr, j, j + 1);
                        ArrSwap(ref associatedArr, j, j + 1);
                    }
                }
            }
        }

        /// <summary>
        /// Sort the given array by ref mode
        /// The given array ITSELF WILL BE CHANGED
        /// </summary>
        /// <typeparam name="T">Any type that can be in an array.</typeparam>
        /// <param name="Arr">The array</param>
        /// <param name="index1">The first item</param>
        /// <param name="index2">The second item</param>
        private static void ArrSwap<T>(ref T[] Arr, int index1, int index2)
        {
            if (index1 > Arr.Length || index2 > Arr.Length) throw new ArgumentException($"The given index is out of range: {index1}, {index2}", "index1 or index 2");
            T cache = Arr[index1];
            Arr[index1] = Arr[index2];
            Arr[index2] = cache;
        }

        /// <summary>
        /// Indicates whether the given date is in the first half year or the second 
        /// </summary>
        /// <param name="dt">The date</param>
        /// <returns>A string contains the information whether the given date is in the first half year or the second</returns>
        public static string GetHalfYear(DateTime dt)
        {
            if (dt.GetWeekOfYear() < 30) return $"2. Halbjahr {dt.AddDays(-365):yyyy}/{dt:yy}";
            else return $"1. Halbjahr {dt:yyyy}/{dt.AddDays(365):yy}";
        }

        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <returns>Number of business days during the 'span'</returns>
        /// <exception cref="ArgumentException"></exception>
        // See: https://stackoverflow.com/questions/1617049/calculate-the-number-of-business-days-between-two-dates
        public static int BusinessDaysUntil(DateTime firstDay, DateTime lastDay)
        {
            // Get date-only portion of date, without its time.
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;

            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;
                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            return businessDays;
        }

    }
}
