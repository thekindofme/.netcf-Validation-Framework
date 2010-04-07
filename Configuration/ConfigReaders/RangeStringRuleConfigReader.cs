using System;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="RangeStringRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>minimum</c> (required): Used to populate <see cref="RangeRule{T}.Maximum"/>.   
    ///     </li>
    ///     <li>
    ///       <c>maximum</c> (required): Used to populate <see cref="RangeRule{T}.Minimum"/>.   
    ///     </li>
    ///     <li>
    ///       <c>equalsMinimumIsValid</c> (optional): Used to populate <see cref="RangeRule{T}.EqualsMinimumIsValid"/>.   
    ///     </li>
    ///     <li>
    ///       <c>equalsMaximumIsValid</c> (optional): Used to populate <see cref="RangeRule{T}.EqualsMaximumIsValid"/>.   
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for RangeStringRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// typeName="RangeStringRule" 
    /// minimum="a" 
    /// maximum="c"
    /// equalsMinimumIsValid="true" 
    /// equalsMaximumIsValid="true"/>
    /// </code>
    /// </example>
    /// <seealso cref="RangeStringRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class RangeStringRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods
		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, RuntimeTypeHandle runtimeTypeHandle)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");

            var equalsMinimumIsValid = RuleData.TryGetValue(ruleData.XmlAttributes, "equalsMinimumIsValid", true);
            var equalsMaximumIsValid = RuleData.TryGetValue(ruleData.XmlAttributes, "equalsMaximumIsValid", true);
			return new RangeStringRule(ruleData.XmlAttributes["minimum"], ruleData.XmlAttributes["maximum"])
			{
				EqualsMaximumIsValid = equalsMaximumIsValid,
				EqualsMinimumIsValid = equalsMinimumIsValid
			};
        }

        #endregion
    }
}