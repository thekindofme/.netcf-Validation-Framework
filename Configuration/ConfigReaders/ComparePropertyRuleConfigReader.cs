using System;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="ComparePropertyRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    /// <b>Note:</b> this <see cref="ValidationFramework.Configuration.IRuleConfigReader"/> can only be applied to properties. If it is applied to a member an <see cref="System.InvalidOperationException"/> will be thrown.
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>propertyToCompare</c> (required): Used to populate <see cref="ComparePropertyRule.PropertyToCompare"/>.  
    ///     </li>
    ///     <li>
    ///       <c>compareOperator</c> (required): Used to populate <see cref="ComparePropertyRule.CompareOperator"/>.   
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for ComparePropertyRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// typeName="ComparePropertyRule" 
    /// propertyToCompare="MyProperty" 
    /// compareOperator="Equal"/>
    /// </code>
    /// </example>
    /// <seealso cref="ComparePropertyRule"/>
    /// <seealso cref="ComparePropertyRuleAttribute"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class ComparePropertyRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, RuntimeTypeHandle runtimeTypeHandle)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");
            var propertyToCompare = ruleData.XmlAttributes["propertyToCompare"];
            var compareOperator = (CompareOperator)Enum.Parse(typeof(CompareOperator), ruleData.XmlAttributes["compareOperator"], true);

            return new ComparePropertyRule(propertyToCompare, compareOperator);
        }

        #endregion
    }
}