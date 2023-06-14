using System;
using System.Globalization;

namespace CyberspawnsServer.Core
{
    public static class DateTimeExtentions
    {
        public static string ToString(this DateTime time) => time.ToString("yy/MM/dd - hh:mm:ss.fff");

        public static bool InRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck.Date >= startDate.Date && dateToCheck.Date <= endDate.Date;
        }

        public static bool InRange(this DateTime dateToCheck, DateTime? startDate, DateTime? endDate)
        {

            ValidateDateRange(startDate, endDate);

            bool matchedStartCondition = true;
            bool matchedEndCondition = true;
            if (startDate.HasValue)
            {
                matchedStartCondition = dateToCheck.Date >= startDate.Value.Date;
            }

            if (endDate.HasValue)
            {
                matchedEndCondition = dateToCheck.Date <= endDate.Value.Date;
            }


            return matchedStartCondition && matchedEndCondition;
        }


        public static bool WithinTime(this DateTime dateToCheck, DateTime? startDate, DateTime? endDate)
        {

            ValidateDateRange(startDate, endDate);

            bool matchedStartCondition = true;
            bool matchedEndCondition = true;
            if (startDate.HasValue)
            {
                matchedStartCondition = dateToCheck >= startDate.Value;
            }

            if (endDate.HasValue)
            {
                matchedEndCondition = dateToCheck <= endDate.Value;
            }


            return matchedStartCondition && matchedEndCondition;
        }


        public static bool DateEquals(this DateTime dateToCheck, DateTime? dateUnderConsideration)
        {
            if (dateUnderConsideration == null)
                return false;

            if (dateToCheck.Date != dateUnderConsideration.Value.Date)
                return false;

            return true;

        }

        public static double ToTimeStamp(this DateTime dateInstance)
        {
            DateTime epochDateTime = new DateTime(1970, 1, 1);
            return (dateInstance - epochDateTime).TotalMilliseconds;
        }
        public static double ToTimeStampSec(this DateTime dateInstance)
        {
            DateTime epochDateTime = new DateTime(1970, 1, 1);
            return (dateInstance - epochDateTime).TotalSeconds;
        }

        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            int difference = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (difference < 0)
                difference += 7;
            return dt.AddDays(-difference).Date;
        }

        public static DateTime LastDayOfWeek(this DateTime dt)
        {
            return dt.FirstDayOfWeek().AddDays(6);
        }

        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            return dt.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static DateTime FirstDayOfNextMonth(this DateTime dt)
        {
            return dt.FirstDayOfMonth().AddMonths(1);
        }

        public static void ValidateDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                    throw new InvalidOperationException("Invalid Date Arguments, Start Date Cannot Be Greater Than End Date");
            }

        }
    }

}
