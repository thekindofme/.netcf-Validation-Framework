using System;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="RequiredStringRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>initialValue</c> (optional): Used to populate <see cref="RequiredRule{T}.InitialValue"/>.   
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for RequiredStringRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// typeName="RequiredStringRule" 
    /// initialValue="hello2"
    /// trimWhiteSpace="false"/>
    /// </code>
    /// </example>
    /// <seealso cref="RequiredStringRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class RequiredStringRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

        /// <summary>
        /// Create a <see cref="Rule"/> from a <see cref="RuleData"/>.
        /// </summary>
        /// <param name="ruleData">The <see cref="RuleData"/> that represent the xml to create the <see cref="Rule"/> for.</param>
        /// <param name="runtimeTypeHandle">The <see cref="System.RuntimeTypeHandle"/> for the <see cref="Type"/> to create the <see cref="Rule"/> for.</param>
        /// <returns>A <see cref="Rule"/> that <paramref name="ruleData"/> represented</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ruleData"/> is null.</exception>
        public override Rule CreateInstance(RuleData ruleData, RuntimeTypeHandle runtimeTypeHandle)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");
            var trimWhiteSpace = true;
            bool? ignoreCase = null;
            string initialValue = null;
            if (ruleData.XmlAttributes != null)
            {
                trimWhiteSpace = RuleData.TryGetValue(ruleData.XmlAttributes, "trimWhiteSpace", true);
                string ignoreCaseString;
                if (ruleData.XmlAttributes.TryGetValue("ignoreCase", out ignoreCaseString))
                {
                    ignoreCase = bool.Parse(ignoreCaseString);
                }

                ruleData.XmlAttributes.TryGetValue("initialValue", out initialValue);
            }
			if (initialValue == null)
			{
				if (ignoreCase != null)
				{
					throw new InvalidOperationException("ignoreCase cannot be set if there is no initialValue.");
				}
				return new RequiredStringRule
				       	{
				       		TrimWhiteSpace = trimWhiteSpace
				       	};
			}
			else
			{
				if (ignoreCase == null)
				{
					ignoreCase = true;
				}
				return new RequiredStringRule
				{
					InitialValue = initialValue,
					IgnoreCase = ignoreCase.Value,
					TrimWhiteSpace = trimWhiteSpace
				};
			}
        }

        #endregion
    }
}