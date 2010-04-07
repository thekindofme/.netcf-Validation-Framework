using System;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="RequiredRule{T}"/> from a <see cref="RuleData"/>.
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
    /// <code lang="xml" title="This example shows an xml configuration for RequiredEnumRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// initialValue="One" 
    /// typeName="RequiredEnumRule"/>
    /// </code>
    /// </example>
    /// <seealso cref="RequiredRule{T}"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class RequiredEnumRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, Type runtimeType)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");

            string initialValue = null;
            if (ruleData.XmlAttributes != null)
            {
                string initialValueString;
                if (ruleData.XmlAttributes.TryGetValue("initialValue", out initialValueString))
                {
                    initialValue = initialValueString;
                }
            }

		    if (initialValue == null)
		    {
                return RequiredEnumRuleCreator.ReadConfig(ruleData.ErrorMessage, ruleData.UseErrorMessageProvider, runtimeType);
		    }
		    else
		    {
                return RequiredEnumRuleCreator.ReadConfig(initialValue, ruleData.ErrorMessage, ruleData.UseErrorMessageProvider, runtimeType);
		    }
        }

        #endregion
    }
}