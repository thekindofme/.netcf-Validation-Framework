using System;
#if (SILVERLIGHT)
using System.IO;
#endif
using ValidationFramework.Configuration;
using ValidationFramework.Reflection;

namespace ValidationFramework 
{
    /// <summary>
    /// Specifies that a <see cref="CustomRule"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="CustomRule"/>
    /// <seealso cref="CustomRuleConfigReader"/>
    /// <seealso cref="CustomValidationEventArgs"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\CustomRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\CustomRuleAttributeExample.vb" lang="vbnet"/>
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]

#if (!SILVERLIGHT)
    [Serializable]
#endif
	public sealed class CustomRuleAttribute : RuleAttribute
    {

        #region Constructors

  
        /// <param name="validationTypeName">The name of the <see cref="Type"/> that the <see cref="ValidationMethod"/> exists on.</param>
        /// <param name="validationMethod">The name of the method to use when running <see cref="Rule.Validate"/>.</param>
        /// <param name="ruleInterpretation">The business interpretation of the <see cref="CustomRuleAttribute"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="ruleInterpretation"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleInterpretation"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="validationTypeName"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="validationTypeName"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="validationMethod"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="validationMethod"/> is <see cref="string.Empty"/>.</exception>
        public CustomRuleAttribute(string validationTypeName, string validationMethod, string ruleInterpretation)
        {
        	Guard.ArgumentNotNullOrEmptyString(validationTypeName, "validationTypeName");
        	Guard.ArgumentNotNullOrEmptyString(validationMethod, "validationMethod");
        	Guard.ArgumentNotNullOrEmptyString(ruleInterpretation, "ruleInterpretation");
        	try
        	{
        		ValidationType = Type.GetType(validationTypeName, true);
        	}
#if (SILVERLIGHT)
        	catch (FileLoadException fileLoadException)
        	{
        		//TODO: remove when this is fixed in silverlight
        		if (fileLoadException.Message.Contains("The requested assembly version conflicts with what is already bound in the app domain or specified in the manifest."))
        		{
					throw new ArgumentException(string.Format("CustomRule could not load the validation type {0}. If you are using silverlight please qualify the CustomRuleAttribute 'validationTypeName' with 'Version', 'Culture' and 'PublicKeyToken'.", validationTypeName), fileLoadException);
        		}
        		else
        		{
        			throw new ArgumentException(string.Format("CustomRule could not load the validation type {0}.", validationTypeName), fileLoadException);
        		}
        	}
#endif
        	catch (Exception exception)
        	{
        		throw new ArgumentException(string.Format("CustomRule could not load the validation type {0}.", validationTypeName), exception);
        	}
        	ValidationMethod = validationMethod;
        	RuleInterpretation = ruleInterpretation;
        }

    	/// <param name="validationType">The <see cref="Type"/> that the <see cref="ValidationMethod"/> exists on.</param>
        /// <param name="validationMethod">The name of the method to use when running <see cref="Rule.Validate"/>.</param>
        /// <param name="ruleInterpretation">The business interpretation of the <see cref="CustomRuleAttribute"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="ruleInterpretation"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleInterpretation"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="validationType"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="validationMethod"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="validationMethod"/> is <see cref="string.Empty"/>.</exception>
        public CustomRuleAttribute(Type validationType, string validationMethod, string ruleInterpretation)
        {
            Guard.ArgumentNotNull(validationType, "validationTypeName");
            Guard.ArgumentNotNullOrEmptyString(validationMethod, "validationMethod");
            Guard.ArgumentNotNullOrEmptyString(ruleInterpretation, "ruleInterpretation");
                ValidationType = validationType;
            ValidationMethod = validationMethod;
            RuleInterpretation = ruleInterpretation;
        }



        #endregion


        #region Properties

        /// <summary>
        /// Gets the name of the method to use when running <see cref="Rule.Validate"/>.
        /// </summary>
		public string ValidationMethod
		{
			get; 
			private set;
		}

        /// <summary>
        /// Gets a <see cref="string"/> that is a business interpretation of the <see cref="Rule"/>.
        /// </summary>
        /// <remarks>
        /// Used as a helper to document the API that <see cref="Rule"/>s area applied to.
        /// </remarks>
        public string RuleInterpretation
        {
			get;
			private set;
        }


        /// <summary>
        /// Gets the name of the <see cref="Type"/> that the <see cref="ValidationMethod"/> exists on.
        /// </summary>
        public Type ValidationType
        {
			get;
			private set;
        }

        #endregion


        #region Methods

    	/// <inheritdoc/>
    	public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            return new CustomRule(ValidationType, ValidationMethod, RuleInterpretation);
        }

        #endregion
    }
}