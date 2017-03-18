using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datelib
{
    public static class CustomDates
    {
        public static void calcuateDayMonthsYears(DateTime starttime, DateTime endtime, out double days, out double months, out double years)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            days = Math.Abs((starttime - endtime).TotalDays);
            months = Math.Abs((starttime.Month - endtime.Month) + 12 * (starttime.Year - endtime.Year));
            TimeSpan span = endtime - starttime;
            // Because we start at year 1 for the Gregorian
            // calendar, we must subtract a year here.
            years = (zeroTime + span).Year - 1;
        }
    }
}
