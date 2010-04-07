using System;
using System.Globalization;
using ValidationFramework.Extensions;

namespace ValidationFramework.Configuration
{
    /// <summary>
	/// A <see cref="IRuleConfigReader"/> that creates a <see cref="NumberConversionRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>typeCode</c> (required): Used to populate <see cref="NumberConversionRule.TypeCode"/>.   
    ///     </li>
    ///     <li>
	///       <c>numberStyles</c> (optional): Used to populate <see cref="NumberConversionRule.NumberStyles"/>.   
    ///     </li>
    ///     <li>
	///       <c>numberFormatInfoTypeName</c> (optional): Used to populate <see cref="NumberConversionRule.NumberFormatInfo"/>.   
    ///     </li>
    ///     <li>
	///       <c>numberFormatInfoPropertyName</c> (optional): Used to populate <see cref="NumberConversionRule.NumberFormatInfo"/>.   
    ///     </li>
    ///   </ul>
	/// Both numberFormatInfoTypeName and numberFormatInfoPropertyName have to be included or excluded.
    /// </remarks>
    /// <example>
	/// <code lang="xml" title="This example shows an xml configuration for NumberConversionRuleConfigReader. The NumberConversionRule generated will validate that a string in convertible to a decimal.">
    /// <rule 
	/// typeCode="Decimal" 
    /// errorMessage="hello" 
	/// typeName="NumberConversionRule" 
	/// numberFormatInfoTypeName="System.Globalization.NumberFormatInfo"
	/// numberFormatInfoPropertyName="CurrentInfo"
	/// numberStyles="AllowDecimalPoint"/>
    /// </code>
    /// </example>
	/// <seealso cref="NumberConversionRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class NumberConversionRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
		public override Rule CreateInstance(RuleData ruleData, RuntimeTypeHandle runtimeTypeHandle)
        {
        	Guard.ArgumentNotNull(ruleData, "ruleData");

        	var numberStyles = NumberStyles.None;

        	string numberStylesString;
            if (ruleData.XmlAttributes.TryGetValue("numberStyles", out numberStylesString))
        	{
        		numberStyles = (NumberStyles) Enum.Parse(typeof (NumberStyles), numberStylesString, true);
        	}

            var typeCodeString = ruleData.XmlAttributes["typeCode"];
        	var typeCode = (TypeCode) Enum.Parse(typeof (TypeCode), typeCodeString, true);

        	string numberFormatInfoTypeName;
            ruleData.XmlAttributes.TryGetValue("numberFormatInfoTypeName", out numberFormatInfoTypeName);

        	string numberFormatInfoPropertyName;
            ruleData.XmlAttributes.TryGetValue("numberFormatInfoPropertyName", out numberFormatInfoPropertyName);

            var removeWhiteSpace = RuleData.TryGetValue(ruleData.XmlAttributes, "removeWhiteSpace", false);

			var numberFormatInfo = (NumberFormatInfo) TypeExtensions.GetStaticProperty(numberFormatInfoTypeName, numberFormatInfoPropertyName);
			return new NumberConversionRule(typeCode)
			       	{
			       		RemoveWhiteSpace = removeWhiteSpace,
						NumberStyles = numberStyles,
						NumberFormatInfo = numberFormatInfo
			       	};
        }

    	#endregion
    }
}