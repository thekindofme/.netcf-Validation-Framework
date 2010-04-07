using System;
using System.Reflection;

namespace ValidationFramework.Extensions
{
    internal static class CustomAttributeProviderExtensions
    {

        public static bool ContainsAttribute<TAttribute>(this ICustomAttributeProvider customAttributeProvider) where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute);
            var attributes = customAttributeProvider.GetCustomAttributes(attributeType, true);
            return attributes.Length == 1;
        }
    }
}
