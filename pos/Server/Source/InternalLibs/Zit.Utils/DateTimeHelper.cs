using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zit.Utils
{
    public static class DateTimeHelper
    {
        public static DateTime GetMaxOfDate(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, 23, 59, 59, 999, datetime.Kind);
        }
        public static DateTime GetMinOfDate(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, 0, datetime.Kind);
        }

        public static DateTime GetStartOfTheWeek(this DateTime datetime)
        {
            DateTime index = datetime.Date;
            while (index.DayOfWeek != DayOfWeek.Monday)
            {
                index = index.AddDays(-1);
            }

            return index;
        }

        public static int GetQuarter(this DateTime datetime)
        {
            return datetime.Month / 3+1;
        }

        public static int GetWeek(this DateTime datetime)
        {
            return (int)(Math.Round(datetime.DayOfYear / 7.0, 0) + 1);
        }
    }
}
