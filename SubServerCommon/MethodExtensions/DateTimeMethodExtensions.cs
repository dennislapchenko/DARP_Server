using System;

namespace ComplexServerCommon
{
    public static class DateTimeMethodExtensions
    {

        public static DateTime UpToSeconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
    }
}
