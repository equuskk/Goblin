using System;
using System.Globalization;

namespace Goblin.WebApp.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetWeekNumber(this DateTime date)
        {
            var ciCurr = CultureInfo.CurrentCulture;
            var weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek,
                                                        CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
            return weekNum;
        }
    }
}