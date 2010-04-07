using System;
using System.Globalization;
using ValidationFramework.Extensions;

namespace ValidationFramework.Configuration
{
    /// <summary>
	/// A <see cref="IRuleConfigReader"/> that creates a <see cref="DateTimeConversionRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
	///       <c>dateTimeStyles</c> (optional): Used to populate <see cref="DateTimeConversionRule.DateTimeStyles"/>.   
    ///     </li>
    ///     <li>
	///       <c>dateTimeFormatInfoTypeName</c> (optional): Used to populate <see cref="DateTimeConversionRule.DateTimeFormatInfo"/>.   
    ///     </li>
    ///     <li>
	///       <c>dateTimeFormatInfoPropertyName</c> (optional): Used to populate <see cref="DateTimeConversionRule.DateTimeFormatInfo"/>.   
    ///     </li>
    ///   </ul>
	/// Both dateTimeFormatInfoTypeName and dateTimeFormatInfoPropertyName have to be included or excluded.
    /// </remarks>
    /// <example>
	/// <code lang="xml" title="This example shows an xml configuration for DateTimeConversionRuleConfigReader. The DateTimeConversionRule generated will validate that a string in convertible to a DateTime.">
    /// <rule 
    /// errorMessage="hello" 
	/// typeName="DateTimeConversionRule" 
	/// dateTimeFormatInfoTypeName="System.Globalization.DateTimeFormatInfo"
	/// dateTimeFormatInfoPropertyName="CurrentInfo"
	/// dateTimeStyles="AllowWhiteSpaces"/>
    /// </code>
    /// </example>
	/// <seealso cref="DateTimeConversionRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class DateTimeConversionRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
		public override Rule CreateInstance(RuleData ruleData, RuntimeTypeHandle runtimeTypeHandle)
        {
        	Guard.ArgumentNotNull(ruleData, "ruleData");
			var dateTimeStyles = DateTimeStyles.None;

        	string dateTimeStylesString;
            if (ruleData.XmlAttributes.TryGetValue("dateTimeStyles", out dateTimeStylesString))
        	{
				dateTimeStyles = (DateTimeStyles)Enum.Parse(typeof(DateTimeStyles), dateTimeStylesString, true);
        	}

			string format;
            ruleData.XmlAttributes.TryGetValue("format", out format);

        	string dateTimeFormatInfoTypeName;
            ruleData.XmlAttributes.TryGetValue("dateTimeFormatInfoTypeName", out dateTimeFormatInfoTypeName);

        	string dateTimeFormatInfoPropertyName;
            ruleData.XmlAttributes.TryGetValue("dateTimeFormatInfoPropertyName", out dateTimeFormatInfoPropertyName);

        	var dateTimeFormatInfo = (DateTimeFormatInfo) TypeExtensions.GetStaticProperty(dateTimeFormatInfoTypeName, dateTimeFormatInfoPropertyName);
			return new DateTimeConversionRule(format, dateTimeFormatInfo, dateTimeStyles);
        }

    	#endregion
    }
}