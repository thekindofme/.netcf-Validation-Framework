using System;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="RequiredBoolRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="RuleData"/> for default attributes.
    ///   No extra elements or attributes.
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for RequiredBoolRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// typeName="RequiredBoolRule"/>
    /// </code>
    /// </example>
    /// <seealso cref="RequiredBoolRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class RequiredBoolRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, Type runtimeType)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");
            return new RequiredBoolRule();
        }

        #endregion
    }
}