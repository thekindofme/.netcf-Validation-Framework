using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace ValidationFramework.Extensions
{
    /// <summary>
    /// String helper methods
    /// </summary>
	public static class StringExtensions
	{

		private const string space = " ";


        internal static string ToUpperIgnoreNull(this string value)
        {
            if (value != null)
            {
                value = value.ToUpper(CultureInfo.InvariantCulture);
            }
            return value;
        }
        internal static XmlReader ToXmlReader(this string value)
        {
            var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = true, IgnoreComments = true };

            var xmlReader = XmlReader.Create(new StringReader(value), settings);
            xmlReader.Read();
            return xmlReader;
        }

		/// <summary>
		/// Splits string name into a readable string based on camel casing.
		/// </summary>
		/// <param name="value">The string to split.</param>
		/// <returns>A modified <see cref="string"/> with spaces inserted in front of every, excluding the first, upper-cased character.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is a null reference.</exception>
		public static string ToCamelTokenized(this string value)
		{
			Guard.ArgumentNotNull(value, "value");
			if (value.Length ==0)
			{
				return value;
			}
			var stringBuilder = new StringBuilder(value.Length);
			stringBuilder.Append(value[0]);
			for (var index = 1; index < value.Length; index++)
			{
				var c = value[index];
				if (Char.IsUpper(c))
				{
					stringBuilder.Append(space);
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		public static string GetLastAfter(this string value, string after)
		{
			Guard.ArgumentNotNull(value, "value");
			if (value.Contains(after))
			{
				var dotIndex = value.LastIndexOf(after);
				return value.Substring(dotIndex + 1, value.Length - dotIndex - 1);
			}
			return value;
		}

	}
}
