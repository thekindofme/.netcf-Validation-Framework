using System;
using System.Globalization;
using ValidationFramework.Reflection;
#if (WindowsCE)
using DateTimeParser = ValidationFramework.Utilities.DateTimeParser;
#else
using DateTimeParser = System.DateTime;
#endif

namespace ValidationFramework
{

    /// <summary>
    /// Performs a string to number conversion validation.
    /// </summary>
    /// <remarks>If the value being validated is null the rule will evaluate to true.</remarks>
    /// <seealso cref="DateTimeConversionRuleConfigReader"/>
	/// <seealso cref="DateTimeConversionRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class DateTimeConversionRule : ValueRule
    {

        #region Constructors

  
        /// <remarks>
        /// The following are defaulted
        /// <list type="bullet">
		/// <item>
		/// <see cref="Format"/> to null:
		/// </item>
		/// <item>
		/// <see cref="DateTimeFormatInfo"/> to <see cref="System.Globalization.DateTimeStyles.None"/>:
		/// </item>
        /// </list>
        /// </remarks>
		public DateTimeConversionRule()
			: this(null, null, DateTimeStyles.None)
        {
        }


		/// <param name="format">The format to use for the conversion.</param>
		/// <param name="dateTimeFormatInfo">The <see cref="System.Globalization.DateTimeFormatInfo"/> that this <see cref="DateTimeConversionRule"/> uses to convert with.</param>
		/// <param name="dateTimeStyles">The <see cref="System.Globalization.DateTimeStyles"/> that this <see cref="DateTimeConversionRule"/> uses to convert with.</param>
		public DateTimeConversionRule(string format, DateTimeFormatInfo dateTimeFormatInfo, DateTimeStyles dateTimeStyles)
            : base(TypePointers.StringType)
        {
        	
			Format = format;
			if (dateTimeFormatInfo == null)
        	{
        		DateTimeFormatInfo = DateTimeFormatInfo.CurrentInfo;
        	}
        	else
        	{
				DateTimeFormatInfo = dateTimeFormatInfo;
        	}

        	DateTimeStyles = dateTimeStyles;
        }

    	#endregion


        #region Properties


		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                return "The value must be convertible to DateTime.";
            }
        }

		/// <summary>
		/// Gets the format to use for the conversion.
		/// </summary>
		public string Format
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the <see cref="System.Globalization.DateTimeFormatInfo"/> that this <see cref="DateTimeConversionRule"/> uses to convert with.
		/// </summary>
		public DateTimeFormatInfo DateTimeFormatInfo
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the <see cref="System.Globalization.NumberStyles"/> that this <see cref="DateTimeConversionRule"/> uses to convert with.
		/// </summary>
		public DateTimeStyles DateTimeStyles
		{
			get;
			private set;
		}

    	#endregion


        #region Methods



		/// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (targetMemberValue != null)
            {
                var s = (string) targetMemberValue;
                if (s.Length == 0)
                {
                    return true;
                }
                else
                {
                    DateTime dateTime;
					if (Format == null)
					{
                        if (DateTimeParser.TryParse(s, DateTimeFormatInfo, DateTimeStyles, out dateTime))
						{
							return true;
						}
					}
					else
					{
                        if (DateTimeParser.TryParseExact(s, Format, DateTimeFormatInfo, DateTimeStyles, out dateTime))
						{
							return true;
						}
					}
                }
                return false;
            }
		    return true;
        }


		/// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
			return string.Format("The {0} '{1}' must be convertible to a date and/or time.", descriptorType, tokenizedMemberName);
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
			var dateTimeConversionRule = (DateTimeConversionRule)rule;
			return dateTimeConversionRule.Format == Format &&
				dateTimeConversionRule.DateTimeStyles == DateTimeStyles &&
				dateTimeConversionRule.DateTimeFormatInfo == DateTimeFormatInfo;
        }



        #endregion
    }
}