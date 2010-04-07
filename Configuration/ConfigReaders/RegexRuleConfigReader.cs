using System;
using System.Text.RegularExpressions;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="RegexRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>validationExpression</c> (required): Used to populate <see cref="ValidationFramework.RegexRule.ValidationExpression"/>.   
    ///     </li>
    ///     <li>
    ///       <c>regexOptions</c> (optional): Used to populate <see cref="ValidationFramework.RegexRule.RegexOptions"/>.   
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for RegexRuleConfigReader. The RegexRule generated will validate an email address.">
    /// <rule 
    /// validationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
    /// errorMessage="hello" 
    /// typeName="RegexRule" 
    /// regexOptions="RightToLeft"/>
    /// </code>
    /// </example>
    /// <seealso cref="RegexRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class RegexRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, Type runtimeType)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");
            var regexOptions = RegexOptions.None;

            string regexOptionsString;
            if (ruleData.XmlAttributes.TryGetValue("regexOptions", out regexOptionsString))
            {
                regexOptions = (RegexOptions) Enum.Parse(typeof (RegexOptions), regexOptionsString, true);
            }

			return new RegexRule(ruleData.XmlAttributes["validationExpression"])
			{
				RegexOptions = regexOptions
			};
        }

        #endregion
    }
}