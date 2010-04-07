using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Defines properties that must be implemented to allow a <see cref="Attribute"/> to define <see cref="Rule"/>s.
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
    public interface IRuleAttribute
    {
        /// <summary>
        /// Gets or sets the error message for <see cref="IRuleAttribute"/>.
        /// </summary>
        string ErrorMessage
        {
            get;
            set;
        }

        string RuleSet
        {
            get;
            set;
        }
		
		/// <seealso cref="Rule.UseErrorMessageProvider"/>
		bool UseErrorMessageProvider
		{
			get;
			set;
		}


    	/// <summary>
    	/// Create a <see cref="Rule"/> based on the provided <see cref="InfoDescriptor"/>.
    	/// </summary>
    	/// <param name="infoDescriptor">The <see cref="InfoDescriptor"/> to create a <see cref="Rule"/> for.</param>
    	/// <returns>The <see cref="Rule"/>  created.</returns>
		Rule CreateRule(InfoDescriptor infoDescriptor);
    }
}