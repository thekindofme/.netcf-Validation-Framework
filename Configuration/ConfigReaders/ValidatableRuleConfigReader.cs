using System;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="ValidatableRule"/> from a <see cref="RuleData"/>.
    /// </summary>
	/// <example>
    /// <code lang="xml" title="This example shows an xml configuration for ValidatableRuleConfigReader.">
    /// <rule 
    /// errorMessage="hello" 
    /// typeName="ValidatableRule"/>
    /// </code>
    /// </example>
    /// <seealso cref="ValidatableRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class ValidatableRuleConfigReader : BaseRuleConfigReader
    {


        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, RuntimeTypeHandle runtimeTypeHandle)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");

            var useMemberErrorMessages = false;
            if (ruleData.XmlAttributes != null)
            {
                useMemberErrorMessages = RuleData.TryGetValue(ruleData.XmlAttributes, "useMemberErrorMessages", false);
            }
            //TODO: refactor with ValidatableRuleAttribute
                	var instance = new ValidatableRule();
            if (useMemberErrorMessages)
            {
                if (ruleData.ErrorMessage == null)
                {
                	instance.UseMemberErrorMessages = true;
                }
                else
                {
                    throw new InvalidOperationException("Cannot use 'useMemberErrorMessages'");
                }
            }
			return instance;
        }

        #endregion
    }
}