using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Base class for all <see cref="Attribute"/>s that define <see cref="Rule"/>s.
    /// </summary>
    /// <seealso cref="IRuleAttribute"/>
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
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public abstract class RuleAttribute : Attribute, IRuleAttribute
    {

        #region Properties



        /// <summary>
        /// Gets or sets A <see cref="string"/> used to group <see cref="Rule"/>s. Use a null to indicate no grouping.
        /// </summary>
        public string RuleSet
        {
            get;
            set;
        }


        /// <seealso cref="Rule.UseErrorMessageProvider"/>
        public bool UseErrorMessageProvider
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public short Severity
        {
            get;
            set;
        }

        #endregion


		#region Methods

		/// <inheritdoc />
		public virtual Rule CreateRule(InfoDescriptor infoDescriptor)
		{
			var rule = CreateInstance(infoDescriptor);
			CopyProperties(rule);
			return rule;
		}

    	/// <inheritdoc />
		public abstract Rule CreateInstance(InfoDescriptor infoDescriptor);

    	protected virtual void CopyProperties(Rule rule)
    	{
    		rule.UseErrorMessageProvider = UseErrorMessageProvider;
    		rule.ErrorMessage = ErrorMessage;
    		rule.RuleSet = RuleSet;
    		rule.Severity = Severity;
    	}

    	#endregion
    }
}