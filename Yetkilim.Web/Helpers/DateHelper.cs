using System;

namespace Yetkilim.Web.Helpers
{
	public class DateHelper
	{
        public static long ToUnixTimestamp(DateTime target)
        {
            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            return Convert.ToInt64((target - d).TotalSeconds);
        }

        public static DateTime GetStartOfWeek(DateTime value)
        {
            value = value.Date;
            int dayOfWeek = (int)value.DayOfWeek;
            return value.AddDays((double)(-dayOfWeek));
        }
    }
}