using System;
using System.Reflection;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{

    /// <summary>
    /// Performs a custom validation via an defined method.
    /// </summary>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\CustomRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\CustomRuleAttributeExample.vb" lang="vbnet"/>
    /// <code source="Examples\ExampleLibraryCSharp\Reflection\AddCustomRuleWithTypeCacheExample.cs" lang="cs" title="This example shows how to programmatically add a CustomRule to a property." region="Example"/>
    /// <code source="Examples\ExampleLibraryCSharp\Reflection\AddCustomRuleWithTypeCacheStrongTypedExample.cs" lang="cs" title="This example shows how to programmatically add a CustomRule to a property." region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\Reflection\AddCustomRuleWithTypeCacheExample.vb" lang="vbnet" title="This example shows how to programmatically  add a CustomRule to a property." region="Example"/>
    /// </example>
    /// <seealso cref="CustomRuleConfigReader"/>
    /// <seealso cref="CustomRuleAttribute"/>
	/// <seealso cref="CustomValidationEventArgs"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class CustomRule : Rule
    {
        #region Fields

        private const string errorMessageFormat = "The {0} '{1}' is invalid.";
      private readonly string ruleInterpretation;
        private readonly long uniqueHash;

        #endregion


        #region Constructors

 
    

 
        /// <param name="validationType">The name of the type to get the validation method from.</param>
        /// <param name="validationMethod">The method on the current object to use for the validation.</param>
        /// <param name="ruleInterpretation">The business interpretation of the <see cref="CustomRuleAttribute"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="validationType"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="validationMethod"/> could not be found on the <see cref="Type"/> <paramref name="validationType"/>.</exception>
        /// <exception cref="ArgumentException">The <see cref="Type"/> defined by <paramref name="validationType"/> can not be loaded.</exception>
        /// <exception cref="ArgumentException"><paramref name="validationType"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="validationMethod"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ruleInterpretation"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleInterpretation"/> is <see cref="string.Empty"/>.</exception>
        public CustomRule(Type validationType, string validationMethod, string ruleInterpretation)
            : base(null)
        {
            Guard.ArgumentNotNull(validationType, "validationType");
            Guard.ArgumentNotNullOrEmptyString(validationMethod, "validationMethod");
            Guard.ArgumentNotNullOrEmptyString(ruleInterpretation, "ruleInterpretation");
            this.ruleInterpretation = ruleInterpretation;
    
            var validationMethodInfo = validationType.GetMethod(validationMethod, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (validationMethodInfo == null)
            {
                throw new ArgumentException(string.Format("CustomRule could not load the validation method '{0}' from type '{1}'.", validationMethod, validationType.ToUserFriendlyString()), "validationMethod");
            }
            uniqueHash = validationMethodInfo.MethodHandle.Value.ToInt64();
            Handler = Delegate.CreateDelegate(typeof(EventHandler<CustomValidationEventArgs>), null, validationMethodInfo) as EventHandler<CustomValidationEventArgs>;
            if (Handler == null)
            {
                throw new Exception("Could not Delegate.CreateDelegate.");
            }
        }


 
        /// <param name="eventHandler">The <see cref="EventHandler{CustomValidationEventArgs}"/> that represents the method to validate this <see cref="CustomRule"/>.</param>
        /// <param name="ruleInterpretation">The business interpretation of the <see cref="CustomRuleAttribute"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="ruleInterpretation"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleInterpretation"/> is <see cref="string.Empty"/>.</exception>
        public CustomRule(EventHandler<CustomValidationEventArgs> eventHandler, string ruleInterpretation)
            : base(null)
        {
            Guard.ArgumentNotNullOrEmptyString(ruleInterpretation, "ruleInterpretation");
            this.ruleInterpretation = ruleInterpretation;
            Handler = eventHandler;
            //TODO: ensure method is static????
        }

        #endregion


        #region Properties

		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                return ruleInterpretation;
            }
        }


        /// <summary>
        /// Gets the <see cref="EventHandler{TEventArgs}"/> that represents the method to validate this <see cref="CustomRule"/>.
        /// </summary>
        public EventHandler<CustomValidationEventArgs> Handler
        {
			get;
			private set;
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        public override ValidationError BaseValidate(object targetObjectValue, object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            var args = new CustomValidationEventArgs(this, targetObjectValue, targetMemberValue, context);
            Handler(this, args);


            if (args.IsValid)
            {
                return null;
            }
            else
            {
                if (string.IsNullOrEmpty(args.ErrorMessage))
                {
                    return CreateValidationError(targetObjectValue, targetMemberValue, context, infoDescriptor);
                }
                else
                {
                    return new ValidationError(this, args.ErrorMessage, infoDescriptor, targetMemberValue);
                }
            }
        }

        public override bool Validate(object targetObjectValue, object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
            return string.Format(errorMessageFormat, descriptorType, tokenizedMemberName);
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            var customRule = (CustomRule)rule;
            return uniqueHash == customRule.uniqueHash;
        }


        #endregion
    }
}