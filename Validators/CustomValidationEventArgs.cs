using System;

namespace ValidationFramework
{
    /// <summary>
    /// Provides data for a <see cref="CustomRule"/> to perform a <see cref="Rule.Validate"/>. 
    /// </summary>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\CustomRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\CustomRuleAttributeExample.vb" lang="vbnet"/>
    /// </example>
    /// <seealso cref="CustomRuleAttribute"/>
    /// <seealso cref="CustomRule"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class CustomValidationEventArgs : EventArgs
    {
 
        #region Constructors

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomValidationEventArgs"/> class.
        /// </summary>
        /// <param name="customRule">The <see cref="ValidationFramework.CustomRule"/> that has fired the validation.</param>
        /// <param name="targetObjectValue">The value of the object containing the member to validate.</param>
        /// <param name="targetMemberValue">The value of the member to validate.</param>
        /// <param name="context">An <see cref="object"/> that contains data for the <see cref="Rule"/> to validate. The default is null.</param>
        public CustomValidationEventArgs(CustomRule customRule, object targetObjectValue, object targetMemberValue, object context)
        {
            CustomRule = customRule;
            TargetObjectValue = targetObjectValue;
           TargetMemberValue = targetMemberValue;
            Context = context;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the value of the object containing the member to validate.
        /// </summary>
        public object TargetObjectValue
        {
			get;
			private set;
        }

        /// <summary>
        /// The value of the member to validate.
        /// </summary>
        public object TargetMemberValue
        {
			get;
			private set;
        }

        /// <summary>
        /// Gets or sets <see cref="ValidationFramework.CustomRule"/> that has fired the validation.
        /// </summary>
        public CustomRule CustomRule
        {
			get;
			private set;
        }

    	/// <summary>
    	/// Gets or sets the error message.
    	/// </summary>
    	public string ErrorMessage
    	{
    		get;
    		set;
    	}

    	/// <summary>
    	/// Gets or sets a <see langword="bool"/> indicating if the validation succeeded.
    	/// </summary>
    	public bool IsValid
    	{
    		get;
    		set;
    	}

    	/// <summary>
        /// Gets the <see cref="object"/> that contains data for the <see cref="Rule"/> being validated. The default is null.
        /// </summary>
        public object Context
        {
			get;
			private set;
        }

        #endregion
    }
}