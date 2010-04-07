using System;
using System.Globalization; 
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
	/// Specifies that a <see cref="DateTimeConversionRule"/> should be applied to the program element.
    /// </summary>
	/// <seealso cref="DateTimeConversionRule"/>
    /// <example>
	/// <code source="Examples\ExampleLibraryCSharp\Validators\DateTimeConversionRuleAttributeExample.cs" lang="cs"/>
	/// <code source="Examples\ExampleLibraryVB\Validators\DateTimeConversionRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class DateTimeConversionRuleAttribute : RuleAttribute
    {

    	#region Fields

		private readonly string format;

    	#endregion


        #region Constructors

        /// <inheritdoc />
		public DateTimeConversionRuleAttribute()
		{
			DateTimeStyles = DateTimeStyles.None;
        }

    
		/// <param name="format">The format used for conversion.</param>
		public DateTimeConversionRuleAttribute(string format)
        {
			this.format = format;
        	DateTimeStyles = DateTimeStyles.None;
        }

   
		/// <param name="format">The format used for conversion.</param>
        /// <param name="dateTimeFormatInfoType">The <see cref="Type"/> to get <see cref="System.Globalization.DateTimeFormatInfo"/> from.</param>
        /// <param name="dateTimeFormatInfoPropertyName">The name of the static property to get <see cref="System.Globalization.DateTimeFormatInfo"/> from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dateTimeFormatInfoType"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dateTimeFormatInfoPropertyName"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="dateTimeFormatInfoPropertyName"/> is <see cref="string.Empty"/>.</exception>
        public DateTimeConversionRuleAttribute(string format, Type dateTimeFormatInfoType, string dateTimeFormatInfoPropertyName)
        {
            Guard.ArgumentNotNullOrEmptyString(dateTimeFormatInfoPropertyName, "dateTimeFormatInfoPropertyName");
            Guard.ArgumentNotNull(dateTimeFormatInfoType, "dateTimeFormatInfoType");
			this.format = format;
            DateTimeFormatInfoType = dateTimeFormatInfoType;
            DateTimeFormatInfoPropertyName = dateTimeFormatInfoPropertyName;
        	DateTimeStyles = DateTimeStyles.None;
        }

		/// <param name="format">The format used for conversion.</param>
        /// <param name="dateTimeFormatInfoTypeName">The name of the <see cref="Type"/> to get <see cref="System.Globalization.DateTimeFormatInfo"/> from.</param>
        /// <param name="dateTimeFormatInfoPropertyName">The name of the static property to get <see cref="System.Globalization.DateTimeFormatInfo"/> from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dateTimeFormatInfoTypeName"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="dateTimeFormatInfoTypeName"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dateTimeFormatInfoPropertyName"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="dateTimeFormatInfoPropertyName"/> is <see cref="string.Empty"/>.</exception>
        public DateTimeConversionRuleAttribute(string format, string dateTimeFormatInfoTypeName, string dateTimeFormatInfoPropertyName)
        {
            Guard.ArgumentNotNullOrEmptyString(dateTimeFormatInfoPropertyName, "dateTimeFormatInfoPropertyName");
            Guard.ArgumentNotNullOrEmptyString(dateTimeFormatInfoTypeName, "dateTimeFormatInfoTypeName");
			this.format = format;
            DateTimeFormatInfoType = Type.GetType(dateTimeFormatInfoTypeName);
            DateTimeFormatInfoPropertyName = dateTimeFormatInfoPropertyName;
        	DateTimeStyles = DateTimeStyles.None;
        }
    	#endregion


        #region Properties


		/// <summary>
		/// Gets the format used for conversion.
		/// </summary>
		/// <seealso cref="DateTimeConversionRule.Format"/>
		public string Format
    	{
    		get
    		{
				return format;
    		}
    	}


		/// <summary>
		/// Gets the <see cref="Type"/> to get <see cref="System.Globalization.DateTimeFormatInfo"/> from.
		/// </summary>
		/// <seealso cref="DateTimeConversionRule.DateTimeFormatInfo"/>
		public Type DateTimeFormatInfoType
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the name of the static property to get <see cref="System.Globalization.DateTimeFormatInfo"/> from. 
		/// </summary>
		/// <seealso cref="DateTimeConversionRule.DateTimeFormatInfo"/>
		public string DateTimeFormatInfoPropertyName
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the <see cref="System.Globalization.DateTimeStyles"/> that this <see cref="DateTimeConversionRule"/> uses to convert with.
		/// </summary>
		/// <seealso cref="DateTimeConversionRule.DateTimeStyles"/>
		public DateTimeStyles DateTimeStyles
		{
			get;
			set;
		}

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
		{
            if (DateTimeFormatInfoType == null)
            {
                return new DateTimeConversionRule(format, null, DateTimeStyles);
            }
            else
            {
                var dateTimeFormatInfo = (DateTimeFormatInfo) TypeExtensions.GetStaticProperty(DateTimeFormatInfoType, DateTimeFormatInfoPropertyName);
                return new DateTimeConversionRule(format, dateTimeFormatInfo, DateTimeStyles);
            }
		}

    	#endregion
    }
}