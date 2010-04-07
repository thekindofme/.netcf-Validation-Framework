using System;
using ValidationFramework.Configuration;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Base class for all Rules.
    /// </summary>
    /// <remarks>
    /// All <see cref="Rule"/>s must be immutable i.e. its state cannot be modified after it is created. This requirement is for performance reasons. 
    /// </remarks>
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
    public abstract class Rule
    {
        #region Fields

        private const string invalidTypeFormat = "Member '{0}' must be a '{1}' to be used for the {2}. Actual Type '{3}'.";
        private string errorMessage;
        private bool useErrorMessageProvider;
    	private string ruleSet;

    	#endregion


        #region Constructors


        /// <param name="Type">The <see cref="Type"/> that this <see cref="Rule"/> can be applied to. Use <see langword="null"/> to indicate it can be applied to any member type.</param>
        protected Rule(Type runtimeType)
        {
            RuntimeType = runtimeType;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets a <see cref="string"/> that is a business interpretation of the <see cref="Rule"/>.
        /// </summary>
        /// <remarks>
        /// Used as a helper to document the API that <see cref="Rule"/>s are applied to.
        /// The code for this property must not use the <see cref="InfoDescriptor"/> property as it may not be set.
        /// </remarks>
        public abstract string RuleInterpretation
        {
            get;
        }



      /// <summary>
        /// Gets the error message for this <see cref="Rule"/>.
        /// </summary>
        /// <remarks>
        /// In the case of an application with a user interface this property would often be displayed to the user. So it should contain proper grammar and punctuation.
        /// </remarks>
        public string ErrorMessage
      {
        get { return errorMessage; }
        set
        {
			if (value != null)
			{
				if (value.Length == 0)
				{
					throw new ArgumentException("Error message cannot be string.Empty", "value");
				}
			} 
			errorMessage = value;
        }
      }


      /// <summary>
        /// Gets the <see cref="RuntimeType"/> that this <see cref="Rule"/> can be applied to. A <see langword="null"/> is returned if it can be applied to any member type.
        /// </summary>
        public Type RuntimeType
        {
			get;
			private set;
        }



      /// <summary>
        /// Get a <see cref="bool"/> indicating if <see cref="ConfigurationService.ErrorMessageProvider"/> should be used when determining the error message for this <see cref="Rule"/>.
        /// </summary>
        public bool UseErrorMessageProvider
      {
        get { return useErrorMessageProvider; }
        set { useErrorMessageProvider = value; }
      }


    	public string RuleSet
		{
			get { return ruleSet; }
			set
			{
				ruleSet = value.ToUpperIgnoreNull();
			}
    	}

    	public short Severity{get; set;}

    	#endregion


        #region Methods

        /// <summary>
        /// Called after rule is added to a <see cref="RuleCollection"/> is set but only when <see cref="ErrorMessage"/> is null.
        /// </summary>
        /// <remarks>
        /// Used by inheritors to provide a customized default <see cref="ErrorMessage"/>.
        /// </remarks>
        /// <returns>The error message for the <see cref="Rule"/>.</returns>
        /// <param name="tokenizedMemberName">A user friendly representation of the member name.</param>
        /// <param name="descriptorType">
        /// If <see cref="InfoDescriptor"/> is a <see cref="PropertyDescriptor"/> then <paramref name="descriptorType"/> will be 'property'.
        /// If <see cref="InfoDescriptor"/> is a <see cref="ParameterDescriptor"/> then <paramref name="descriptorType"/> will be 'parameter'.
        /// </param>
        protected abstract string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType);


        /// <summary>
        /// A helper method for creating <see cref="ValidationError"/>s.
        /// </summary>
        /// <remarks>
        /// If <see cref="UseErrorMessageProvider"/> is <c>true</c> then <see cref="ConfigurationService.ErrorMessageProvider"/> will be called to get the error message. Otherwise the <see cref="ErrorMessage"/> will be used.
        /// </remarks>
        /// <param name="targetObjectValue">The value of the object containing the member to validate.</param>
        /// <param name="targetMemberValue">The value of the member to validate.</param>
        /// <param name="context">An <see cref="object"/> that contains data for the <see cref="Rule"/> to validate.</param>
        /// <param name="infoDescriptor">The <see cref="InfoDescriptor"/> for the <see cref="ValidationError"/> being generated.</param>
        /// <returns>A constructed <see cref="ValidationError"/>.</returns>
        protected ValidationError CreateValidationError(object targetObjectValue, object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (UseErrorMessageProvider)
            {
				if (ConfigurationService.ErrorMessageProvider == null)
				{
					throw new InvalidOperationException("useErrorMessageProvider is true but no ErrorMessageProvider is specified in the ConfigurationService.");
				}
                var retrievedErrorMessage = ConfigurationService.ErrorMessageProvider.RetrieveErrorMessage(this, targetObjectValue, targetMemberValue, context);
                return new ValidationError(this, retrievedErrorMessage, infoDescriptor, targetMemberValue);
            }
            else
            {
                return new ValidationError(this, ErrorMessage, infoDescriptor, targetMemberValue);
            }
        }


        /// <summary>
        /// Check that the <see cref="RuntimeTypeHandle"/> is valid.
        /// </summary>
        internal virtual void CheckType(InfoDescriptor infoDescriptor)
        {
            //var targetMemberRuntimeTypeHandle = infoDescriptor.RuntimeType;
            //Validate that the attribute is applied to the correct type of property
            if (RuntimeType != null)
            {
                //var targetMemberRuntimeType = Type.GetTypeFromHandle(targetMemberRuntimeTypeHandle);
                var targetMemberRuntimeType = infoDescriptor.RuntimeType;
                //TODO: Hack for ref params. Should be an easier way of doing this???
                var fullName = targetMemberRuntimeType.FullName;
                if (fullName.EndsWith("&"))
                {
                    targetMemberRuntimeType = Type.GetType(fullName.Substring(0, fullName.Length - 1), true);
                }
                //var requiredMemberType = Type.GetTypeFromHandle(RuntimeType);

                var underlyingType = Nullable.GetUnderlyingType(targetMemberRuntimeType);
                if ((underlyingType == null) || (underlyingType != RuntimeType))
                {
                    if (!RuntimeType.IsAssignableFrom(targetMemberRuntimeType, true))
                    {
                      var friendlyRuleTypeName = GetType().ToUserFriendlyString();
                      var friendlyTargetMemberTypeName = targetMemberRuntimeType.ToUserFriendlyString();
                      var friendlyRequiredMemberTypeName = RuntimeType.ToUserFriendlyString();
                      var exceptionMessage = string.Format(invalidTypeFormat, infoDescriptor.Name, friendlyRequiredMemberTypeName, friendlyRuleTypeName, friendlyTargetMemberTypeName);
                        throw new ArgumentException(exceptionMessage, "value");
                    }
                }
            }
        }


        internal void SetDefaultErrorMessage(InfoDescriptor infoDescriptor)
        {
            //if the error message has not been set by the user ask the inheritor 
            if (!UseErrorMessageProvider && (ErrorMessage == null))
            {
				var tokenizedParameterName = infoDescriptor.Name.ToCamelTokenized();
                if (infoDescriptor is PropertyDescriptor)
                {
                    ErrorMessage = GetComputedErrorMessage(tokenizedParameterName, "property");
                }
                else if (infoDescriptor is ParameterDescriptor)
                {
                    ErrorMessage = GetComputedErrorMessage(tokenizedParameterName, "parameter");
                }
                else
                {
                    //Fail over to "member" for unit testing purposes
                    ErrorMessage = GetComputedErrorMessage(tokenizedParameterName, "member");
                }
            }
        }


        /// <summary>
        /// Validate the member this <see cref="Rule"/> is applied to.
        /// </summary>
        /// <returns><see langword="true"/> if the member is valid; otherwise <see langword="false"/>.</returns>
        /// <param name="targetObjectValue">The value of the object containing the member to validate.</param>
        /// <param name="targetMemberValue">The value of the member to validate.</param>
        /// <param name="context">An <see cref="object"/> that contains data for the <see cref="Rule"/> to validate. The default is null.</param>
        /// <param name="infoDescriptor">The <see cref="InfoDescriptor"/> for the member being validated.</param>
        public virtual ValidationError BaseValidate(object targetObjectValue, object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (Validate(targetObjectValue, targetMemberValue, context, infoDescriptor))
            {
                return null;
            }
            else
            {
                if (UseErrorMessageProvider)
                {
                    if (ConfigurationService.ErrorMessageProvider == null)
                    {
                        throw new InvalidOperationException("useErrorMessageProvider is true but no ErrorMessageProvider is specified in the ConfigurationService.");
                    }
                    var errorMessage = ConfigurationService.ErrorMessageProvider.RetrieveErrorMessage(this, targetObjectValue, targetMemberValue, context);
                    return new ValidationError(this, errorMessage, infoDescriptor, targetMemberValue);
                }
                else
                {
                    return new ValidationError(this, ErrorMessage, infoDescriptor, targetMemberValue);
                }
            }
        }
 public abstract bool Validate(object targetObjectValue, object targetMemberValue, object context, InfoDescriptor infoDescriptor);


        /// <summary>
        /// Checks if the current <see cref="Rule"/> is equivalent to another <see cref="Rule"/>.
        /// </summary>
        /// <remarks>
        /// Called for each <see cref="Rule"/> in <see cref="RuleCollection"/> when a new <see cref="Rule"/> is added. This method is only called when both the existing <see cref="Rule"/> and the <see cref="Rule"/> being are of the same <see cref="Type"/> and have the same <see cref="RuleSet"/>. So it is safe to directly cast <paramref name="rule"/> to the current type. All properties in <paramref name="rule"/> should be compared to the propeties of the current <see cref="Rule"/>.
        /// </remarks>
        /// <param name="rule">The <see cref="Rule"/> to check for equivalence.</param>
        /// <returns><see langword="true"/> if <paramref name="rule"/> is equivalent to the current <see cref="Rule"/>; otherwise <see langword="false"/>.</returns>
        public abstract bool IsEquivalent(Rule rule);

        #endregion
    }
}