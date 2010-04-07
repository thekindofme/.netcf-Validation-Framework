using ValidationFramework.Reflection;

namespace ValidationFramework.Extensions
{
    /// <summary>
    /// Extends <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets the value of a property through reflection.
        /// </summary>
        /// <param name="value">The <see cref="object"/> to get the value from.</param>
        /// <param name="propertyName">The name of the property to extract the value for.</param>
        /// <returns>The value of the property.</returns>
        public static object GetPropertyValue(this object value, string propertyName)
        {
            Guard.ArgumentNotNull(value, "value");
            var propertyInfo = value.GetType().GetProperty(propertyName, TypeDescriptor.PropertyFlags);
            return propertyInfo.GetValue(value, null);
        }
    }
}
