using System;
using System.Globalization;

namespace ValidationFramework.Reflection
{
    internal static class TypeConverterEx
    {
        public static object ChangeType(string value, Type conversionType)
        {
            if (conversionType.TypeHandle.Equals(TypePointers.DateTimeTypeHandle))
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