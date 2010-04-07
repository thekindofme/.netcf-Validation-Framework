using System;
using System.Globalization;
using SystemDateTime = System.DateTime;

namespace ValidationFramework.Utilities
{
    internal static class DateTimeParser
    {
        public static bool TryParse(string s, DateTimeFormatInfo dateTimeFormatInfo, DateTimeStyles dateTimeStyles, out SystemDateTime dateTime)
        {
            try
            {
                dateTime = SystemDateTime.Parse(s, dateTimeFormatInfo, dateTimeStyles);
                return true;
            }
            catch (FormatException)
            {
                dateTime = SystemDateTime.MinValue;
                return false;
            }
        }
        public static bool TryParseExact(string s, string format,DateTimeFormatInfo dateTimeFormatInfo, DateTimeStyles dateTimeStyles, out SystemDateTime dateTime)
        {
            try
            {
                dateTime = SystemDateTime.ParseExact(s, format, dateTimeFormatInfo, dateTimeStyles);
                return true;
            }
            catch (FormatException)
            {
                dateTime = SystemDateTime.MinValue;
                return false;
            }
        }
    }
}
