using System;
using System.Collections.Generic;
#if (!SILVERLIGHT)
using System.ComponentModel;
#endif
#if (!WindowsCE)
using System.Linq.Expressions;
using ValidationFramework.Extensions;
#endif

namespace ValidationFramework
{
    /// <summary>
    /// Provides base class that developer can inherit from to provide <see cref="INotifyPropertyChanged"/> and <see cref="IDataErrorInfo"/> functionality for all sub-classes.
    /// </summary>
    /// <remarks>This ideal for windows forms applications to get immediate validation feedback on dataBound controls.</remarks>
    /// <example>
	/// <code source="Examples\ExampleLibraryCSharp\BusinessBaseClasses\DataErrorInfoValidatableBaseLambdaExample.cs" title="The following example shows how to inherit from DataErrorInfoValidatableBase and use Lambda to validate properties." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\BusinessBaseClasses\DataErrorInfoValidatableBaseLambdaExample.VB" title="The following example shows how to inherit from DataErrorInfoValidatableBase and use Lambda to validate properties." lang="vbnet" region="Example"/>
	/// <code source="Examples\ExampleLibraryCSharp\BusinessBaseClasses\DataErrorInfoValidatableBaseStringExample.cs" title="The following example shows how to inherit from DataErrorInfoValidatableBase and use strings to validate properties." lang="cs" region="Example"/>
	/// <code source="Examples\ExampleLibraryVB\BusinessBaseClasses\DataErrorInfoValidatableBaseStringExample.vb" title="The following example shows how to inherit from DataErrorInfoValidatableBase and use strings to validate properties." lang="vbnet" region="Example"/>
    /// </example>
    /// <seealso cref="NotifyValidatableBase"/>
    /// <seealso cref="IValidatable"/>
	/// <seealso cref="ValidatableBase"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public abstract class DataErrorInfoValidatableBase : ValidatableBase, IDataErrorInfo
    {
   

        #region Properties

		/// <inheritdoc />
        public override bool IsValid
        {
            get
            {
                return PropertyValidationManager.IsValid;
            }
        }

		/// <inheritdoc />
        /// <remarks>Uses <see cref="ResultFormatter.GetConcatenatedErrorMessages(ICollection{ValidationError})"/> to merge the <see cref="ValidationError.ErrorMessage"/>s of <see cref="ValidatableBase.ValidatorResultsInError"/>.</remarks>
        public virtual string Error
        {
            get
            {
                return ResultFormatter.GetConcatenatedErrorMessages(ValidatorResultsInError);
            }
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        public virtual string this[string columnName]
        {
            get
            {
                var validationErrors = PropertyValidationManager.GetResultsInError(columnName);
                return ResultFormatter.GetConcatenatedErrorMessages(validationErrors);
            }
        }

 
#if (!WindowsCE)
		
		/// <summary>
		/// Gets the error for a property.
		/// </summary>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> representing the property whose error message to get.  The parameter is case sensitive.</param>
		/// <returns>The error message for the property. The default is an <see cref="string.Empty"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="expression"/> is a null reference.</exception>
		public string GetError<TProperty>(Expression<Func<TProperty>> expression)
		{
				return this[TypeExtensions.GetMemberName(expression)];
		}

        /// <summary>
        /// Perform validation for specified property.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{TDelegate}"/> representing the property to validate.</param>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is a null reference.</exception>
        protected void ValidateProperty<TProperty>(Expression<Func<TProperty>> expression)
        {
            ValidateProperty(TypeExtensions.GetMemberName(expression));
        }
#endif
    	/// <summary>
        /// Perform validation for specified property.
        /// </summary>
		/// <param name="propertyName">The name of the property to validate. The parameter is case sensitive.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is a null reference.</exception>
		/// <exception cref="ArgumentException"><paramref name="propertyName"/> is <see cref="string.Empty"/>.</exception>
        protected void ValidateProperty(string propertyName)
        {
                PropertyValidationManager.Validate(propertyName);
        }

        #endregion
    }
}