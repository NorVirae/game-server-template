using System;
using System.Collections.Generic;
using System.Text;

namespace CyberspawnsServer.Core
{
    public class Timer
    {
        public static DateTime serverStartTime;

        

        private static double delta;
        private static double seconds;
        private static double time;

        public static float DeltaTime
        {
            get { return (float)delta; }
        }

        public static DateTime Now
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public static double TotalsecondsSinceStart
        {
            get
            {
                return Now.Subtract(serverStartTime).TotalSeconds;
            }
        }

        public static string GetTimeFormat()
        {
            DateTime now = Now;
            
            return string.Format("{{0}}");
        }

        public static void Init()
        {
            serverStartTime = DateTime.UtcNow;
        }

        public static bool CheckTick()
        {
            double totalSec = Now.GetTotalSeconds();
            delta =  totalSec - time;
            time = totalSec;
            seconds += delta;
            if(seconds >= 0.5f)
            {
                seconds = 0;
                return true;
            }
            return false;
        }
    }

    internal static class DateTimeEx
    {
        public static double GetTotalSeconds(this DateTime time)
        {
            return (time.ToUniversalTime().Subtract(new DateTime(2020, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// Gets the Unix timestamp of the DateTime object
        /// </summary>
        public static ulong ToUnixTimestamp(this DateTime time)
        {
            return (ulong)Math.Truncate(time.GetTotalSeconds());
        }
    }

    public struct TimeMode
    {
        //public int 
    }
}
