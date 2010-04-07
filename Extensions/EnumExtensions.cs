using System;
#if (WindowsCE || SILVERLIGHT)
using System.Collections.Generic;
using System.Reflection;
#endif

namespace ValidationFramework.Extensions
{
    /// <summary>
    /// Converts an <see cref="Enum"/> to a user friendly representation that can be displayed on a user interface.
    /// </summary>
    /// <seealso cref="EnumUserFriendlyStringAttribute"/>
	public static class EnumExtensions
    {
    
        /// <summary>
        /// Converts an <see cref="Enum"/> to a user friendly representation that can be displayed on a user interface. 
        /// </summary>
        /// <param name="enumItem">The <see cref="Enum"/> to convert.</param>
        /// <returns>A user friendly string representing the <paramref name="enumItem"/>.</returns>
        /// <remarks>
        /// Uses the <see cref="EnumUserFriendlyStringAttribute"/> on each item in the <see cref="Enum"/> to determine the string returned.
        /// If no <see cref="EnumUserFriendlyStringAttribute"/> is specified it will use <see cref="StringExtensions.ToCamelTokenized"/> for the return value.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="enumItem"/> is a null reference.</exception>
        public static string ToUserFriendlyString(this Enum enumItem)
        {
            Guard.ArgumentNotNull(enumItem, "enumItem");
            var type = enumItem.GetType();
            var enumItemValue = enumItem.ToString();
            var memInfo = type.GetMember(enumItemValue);
            var attributes = memInfo[0].GetCustomAttributes(typeof(EnumUserFriendlyStringAttribute), false);
            if (attributes.Length == 1)
            {
                var attribute = (EnumUserFriendlyStringAttribute)attributes[0];
                return attribute.UserFriendlyName;
            }
            else
            {
				return enumItemValue.ToCamelTokenized();
            }
		}

#if (WindowsCE || SILVERLIGHT)
		public static Array GetValues(Type enumType)
        {
            //TODO: should cache this
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Argument must be enum.", "enumType");
            }
            var infos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var list= new List<object>();
            foreach (var fi in infos)
            {
                var value = fi.GetValue(null);
                list.Add(value);
            }
            return list.ToArray();
        }
        public static string[] GetNames(Type enumType)
        {
            //TODO: should cache this
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Argument must be enum.", "enumType");
            }
            var infos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var list= new List<string>();
            foreach (var fi in infos)
            {
                list.Add(fi.Name);
            }
            return list.ToArray();
        }
#endif

    }
}