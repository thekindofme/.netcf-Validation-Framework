using System;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="LengthStringRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>minimum</c> (optional): Used to populate <see cref="ValidationFramework.LengthRule.Maximum"/>.   
    ///     </li>
    ///     <li>
    ///       <c>maximum</c> (required): Used to populate <see cref="ValidationFramework.LengthRule.Minimum"/>.   
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for LengthStringRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// typeName="LengthStringRule" 
    /// minimum="1" 
    /// maximum="5"/>";
    /// </code>
    /// </example>
    /// <seealso cref="LengthStringRule"/>
    /// <seealso cref="LengthStringRuleAttribute"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class LengthStringRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, Type runtimeType)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");

            var minimum = RuleData.TryGetValue(ruleData.XmlAttributes, "minimum", 0);
            var maximum = int.Parse(ruleData.XmlAttributes["maximum"]);
            var trimWhiteSpace = RuleData.TryGetValue(ruleData.XmlAttributes, "trimWhiteSpace", true);
            return new LengthStringRule(minimum, maximum)
                   	{
                   		TrimWhiteSpace = trimWhiteSpace
                   	};
        }

        #endregion
    }
}