using System;
using System.Globalization;

namespace ValidationFramework.Reflection
{
    internal static class TypeConverterEx
    {
        public static object ChangeType(string value, Type conversionType)
        {
            if (conversionType.Equals(TypePointers.DateTimeType))
            {
                return DateTimeConverter.Parse(value);
            }
            else
            {
                return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
            }
        }
    }
}