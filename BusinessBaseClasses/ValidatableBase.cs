#if (!SILVERLIGHT)
using System;
#endif
using System.Collections.Generic;

namespace ValidationFramework
{
    /// <summary>
    /// Base class that provides minimal validation logic.
    /// </summary>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\BusinessBaseClasses\ValidatableBaseExample.cs"  title="The following code example shows how to inherit from ValidatableBase." lang="cs" region="Example"/>
	/// <code source="Examples\ExampleLibraryVB\BusinessBaseClasses\ValidatableBaseExample.vb" title="The following code example shows how to inherit from ValidatableBase." lang="vbnet" region="Example"/>
    /// </example>
    /// <seealso cref="NotifyValidatableBase"/>
    /// <seealso cref="DataErrorInfoValidatableBase"/>
	/// <seealso cref="IValidatable"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public abstract class ValidatableBase : IPropertyValidatable
    {


        #region Constructors

        protected ValidatableBase()
        {
            PropertyValidationManager = ValidationManagerFactory.GetPropertyValidationManager(this);
        }

        #endregion


        #region Properties

        /// <summary>
        /// An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. The default is null.
        /// </summary>
        public object Context
        {
            get
            {
                return PropertyValidationManager.Context;
            }
			set
			{
				PropertyValidationManager.Context = value;
			}
        }

        /// <summary>
        /// Gets the rule set to validate.
        /// </summary>
        /// <remarks>Will be a null to validate all <see cref="Rule"/>s.</remarks>
        public string RuleSet
        {
            get
            {
                return PropertyValidationManager.RuleSet;
            }
			set
			{
				PropertyValidationManager.RuleSet = value;
			}
        }


        /// <summary>
        /// Gets a <see cref="IList{T}"/> containing all <see cref="ValidationError"/> in error.
        /// </summary>
        public virtual IList<ValidationError> ValidatorResultsInError
        {
            get
            {
                return PropertyValidationManager.ValidatorResultsInError;
            }
        }


        /// <summary>
        /// Gets the <see cref="ValidationFramework.PropertyValidationManager"/>.
        /// </summary>
        /// <remarks>
        /// This is exposed as 'public' for advanced usage scenarios. In general it should only be used by inherited classes.
        /// </remarks>
        public MemberValidationManager PropertyValidationManager
        {
			get;
			private set;
        }




		/// <inheritdoc />
        /// <remarks>
        /// Base behavior is to validate all properties and return boolean value.
        /// Sub-class can override this if, for example, they are validating on the fly.
        /// </remarks>
        public virtual bool IsValid
        {
            get
            {
                    PropertyValidationManager.ValidateAll();
                return PropertyValidationManager.IsValid;
            }
        }


		/// <inheritdoc />
        public virtual IList<string> ErrorMessages
        {
            get
            {
                return ResultFormatter.GetErrorMessages(PropertyValidationManager.ValidatorResultsInError);
            }
        }

        #endregion
    }
}