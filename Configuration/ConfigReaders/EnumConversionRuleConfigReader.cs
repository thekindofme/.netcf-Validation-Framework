using System;

namespace ValidationFramework.Configuration
{
    /// <summary>
	/// A <see cref="IRuleConfigReader"/> that creates a <see cref="EnumConversionRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
	///       <c>ignoreCase</c> (optional): Used to populate <see cref="EnumConversionRule.IgnoreCase"/>.   
    ///     </li>
    ///     <li>
	///       <c>enumType</c> (optional): Used to populate <see cref="EnumConversionRule.EnumType"/>.   
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
	/// <code lang="xml" title="This example shows an xml configuration for EnumConversionRuleConfigReader. The EnumConversionRule generated will validate that the value can be converted to the given enum Type.">
    /// <rule 
    /// errorMessage="hello" 
	/// typeName="EnumConversionRule" 
	/// enumTypeName="MyNamespace.MyEnum,MyAssembly"
	/// ignoreCase="true"/>
    /// </code>
    /// </example>
	/// <seealso cref="EnumConversionRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class EnumConversionRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
		public override Rule CreateInstance(RuleData ruleData, Type runtimeType)
        {
        	Guard.ArgumentNotNull(ruleData, "ruleData");


            var enumTypeName = ruleData.XmlAttributes["enumTypeName"];
        	var enumType = Type.GetType(enumTypeName);
            var ignoreCase = RuleData.TryGetValue(ruleData.XmlAttributes, "ignoreCase", false);
        	return new EnumConversionRule(enumType)
        	       	{
        	       		IgnoreCase = ignoreCase
        	       	};
        }

    	#endregion
    }
}