using System;

namespace ValidationFramework.Extensions
{
    /// <summary>
    /// Specifies the user friendly name of an <see cref="Enum"/>s <see langword="field"/>.
    /// </summary>
    /// <seealso cref="EnumExtensions"/>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumUserFriendlyStringAttribute : Attribute
    {

        #region Constructors

  
        /// <param name="userFriendlyName">The user friendly name.</param>
        public EnumUserFriendlyStringAttribute(string userFriendlyName)
        {
            UserFriendlyName = userFriendlyName;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets a value indicating the user friendly name of the <see langword="field"/>.
        /// </summary>
        public string UserFriendlyName
        {
        	get;
        	private set;
        }

        #endregion
    }
}