using System;


namespace AtrendUsa.Plugin.Misc.BuildYourBox.Extensions
{
    public static class DateTimeExtensions
    {
        public static int ToTimestamp(this DateTime dt)
        {
            return (int)(dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
