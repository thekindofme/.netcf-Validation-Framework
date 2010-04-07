using System;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// Defines properties and methods that must be implemented to allow a class to convert xml to a <see cref="Rule"/>.
    /// </summary>
    /// <example>
    /// <b>Extending the validation framework</b><br/>
    /// <code source="Examples\ExampleLibraryCSharp\NewRule\RequiredCharacterRule.cs" title="Implementing a custom rule by inheriting from Rule" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\NewRule\RequiredCharacterRule.vb" title="Implementing a custom rule by inheriting from Rule" lang="vbnet"/>
    /// <code source="Examples\ExampleLibraryCSharp\NewRule\RequiredCharacterRuleAttribute.cs" title="Creating a IRuleAttribute for that Rule" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\NewRule\RequiredCharacterRuleAttribute.vb" title="Creating a IRuleAttribute for that Rule" lang="vbnet"/>
    /// <code source="Examples\ExampleLibraryCSharp\NewRule\RequiredCharacterRuleConfigReader.cs" title="Creating IRuleConfigReader for that Rule." lang="cs"/>
    /// <code source="Examples\ExampleLibraryCSharp\NewRule\RequiredCharacterRuleConfigReaderUsage.cs" title="Using the custom ConfigReader." lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\NewRule\RequiredCharacterRuleConfigReader.vb" title="Creating IRuleConfigReader for that Rule." lang="vbnet"/>
    /// <code source="Examples\ExampleLibraryVB\NewRule\RequiredCharacterRuleConfigReaderUsage.vb" title="Using the custom ConfigReader." lang="vbnet"/>
    /// <code source="Examples\ExampleLibraryCSharp\NewRule\ClientRegularExpressionWebValidator.cs" title="A custom RegularExpressionValidator that only validates on the client side." lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\NewRule\ClientRegularExpressionWebValidator.vb" title="A custom RegularExpressionValidator that only validates on the client side." lang="vbnet"/>
    /// </example>
    /// <seealso cref="ConfigurationService"/>
    public interface IRuleConfigReader
    {
        /// <summary>
        /// Create a <see cref="Rule"/> from a <see cref="RuleData"/>.
        /// </summary>
        /// <param name="ruleData">The <see cref="RuleData"/> that represent the xml to create the <see cref="Rule"/> for.</param>
        /// <param name="runtimeType">The <see cref="System.Type"/> for the <see cref="Type"/> to create the <see cref="Rule"/> for.</param>
        /// <returns>A <see cref="Rule"/> that <paramref name="ruleData"/> represented</returns>
        Rule ReadConfig(RuleData ruleData, Type runtimeType);
	}
	public abstract class BaseRuleConfigReader : IRuleConfigReader
	{
		/// <summary>
		/// Create a <see cref="Rule"/> from a <see cref="RuleData"/>.
		/// </summary>
		/// <param name="ruleData">The <see cref="RuleData"/> that represent the xml to create the <see cref="Rule"/> for.</param>
        /// <param name="runtimeType">The <see cref="System.Type"/> for the <see cref="Type"/> to create the <see cref="Rule"/> for.</param>
		/// <returns>A <see cref="Rule"/> that <paramref name="ruleData"/> represented</returns>
		public virtual Rule ReadConfig(RuleData ruleData, Type runtimeType)
		{
			var rule = CreateInstance(ruleData, runtimeType);
			CopyProperties(rule, ruleData);
			return rule;
		}

		/// <inheritdoc />
		public abstract Rule CreateInstance(RuleData ruleData, Type runtimeType);

		protected virtual void CopyProperties(Rule rule, RuleData ruleData)
		{
			rule.UseErrorMessageProvider = ruleData.UseErrorMessageProvider;
			rule.ErrorMessage = ruleData.ErrorMessage;
			rule.Severity = ruleData.Severity;
			rule.RuleSet = ruleData.RuleSet;
		}
	}
}