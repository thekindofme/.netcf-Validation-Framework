using System;
using ValidationFramework.Configuration;
using ValidationFramework.Reflection;

namespace ValidationFramework
{

    /// <summary>
    /// Validates a member that implements <see cref="IValidatable"/> or attempts to validate it using a <see cref="PropertyValidationManager"/>.
    /// </summary>
    /// <remarks>
    /// Allow hierarchical validation of objects. 
    /// If <see cref="IValidatable.IsValid"/> returns <c>false</c> then <see cref="Validate"/> will return a <see cref="ValidationResult"/> with <see cref="ValidationResult.ErrorMessage"/> populated with the concatenated value of <see cref="IValidatable.ErrorMessages"/>.
    /// </remarks>
    /// <seealso cref="ValidatableRuleConfigReader"/>
	/// <seealso cref="ValidatableRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class ValidatableRule: Rule
    {
        #region Fields

        //private static readonly RuntimeType Type = typeof(IValidatable).TypeHandle;

        #endregion


        #region Constructors

        public ValidatableRule()
            : base(null)
        {
        }

        #endregion


        #region Properties


        /// <summary>
        /// Gets or sets a value indicating if <see cref="IValidatable.ErrorMessages"/> should be use as the <see cref="Rule.ErrorMessage"/>.  
        /// </summary>
        public  bool UseMemberErrorMessages
        {
			get;
			set;
        }


		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                return string.Format("The value must return true for IValidatable.IsValid. See Rules for the Type of this member.");
            }
        }


        #endregion


        #region Methods


		/// <inheritdoc />
        public override ValidationError BaseValidate(object targetObjectValue, object targetMemberValue, object context, InfoDescriptor infoDescriptor)
		{
		    if (targetMemberValue != null)
		    {
		        var validatable = targetMemberValue as IValidatable;
		        bool isValid;
		        string errorMessages = null;
		        if (validatable == null)
		        {
		            var validationErrors = PropertyValidationManager.ValidateAll(targetMemberValue, RuleSet, context);
		            isValid = validationErrors.Count == 0;
                    if (!isValid)
                    {
                        errorMessages = ResultFormatter.GetConcatenatedErrorMessages(validationErrors);
                    }
		        }
		        else
		        {
		            isValid = validatable.IsValid;
		            var messages = validatable.ErrorMessages;
                    if (!isValid)
                    {
                        errorMessages = ResultFormatter.GetConcatenatedErrorMessages(messages);
                    }
		        }
		        if (!isValid)
		        {
		            if (UseErrorMessageProvider)
		            {
		                var errorMessage = ConfigurationService.ErrorMessageProvider.RetrieveErrorMessage(this, targetObjectValue, targetMemberValue, context);
                        return new ValidationError(this, errorMessage, infoDescriptor, targetMemberValue);
		            }
		            else if (UseMemberErrorMessages)
		            {
                        return new ValidationError(this, errorMessages, infoDescriptor, targetMemberValue);
		            }
		            else
		            {
		                return CreateValidationError(targetObjectValue, targetMemberValue, context, infoDescriptor);
		            }
		        }
		    }
		    return null;
		}

        public override bool Validate(object targetObjectValue, object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
            if (UseMemberErrorMessages)
            {
                return null;
            }
            else
            {
                return string.Format("The {0} '{1}' is invalid.", descriptorType, tokenizedMemberName);
            }
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            return true;
        }


        #endregion
    }
}