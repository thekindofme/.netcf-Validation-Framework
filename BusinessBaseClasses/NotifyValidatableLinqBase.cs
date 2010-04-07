using System.Collections.Generic;
using System.ComponentModel;
#if (!WindowsCE)
using System.Linq.Expressions;
using System;
using ValidationFramework.Extensions;
#endif
using System.Text;

namespace ValidationFramework
{
	/// <summary>
	/// Used as a base for classes that implement their own <see cref="INotifyPropertyChanged"/> functionality.
	/// </summary>
	/// <remarks>
	/// On construction the inherited instance is cast to a <see cref="INotifyPropertyChanged"/>. 
	/// The <see cref="INotifyPropertyChanged.PropertyChanged"/> event is then attached to so properties can be validated after they have changed.
	/// </remarks>
    public abstract class NotifyValidatableLinqBase : IDataErrorInfo, IPropertyValidatable
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="NotifyValidatableLinqBase"/> class.
		/// </summary>
		protected NotifyValidatableLinqBase()
		{
			PropertyValidationManager = ValidationManagerFactory.GetPropertyValidationManager(this);
			var notifyPropertyChanged = (INotifyPropertyChanged) this;
			notifyPropertyChanged.PropertyChanged += PropertyChanged;
		}

		void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			PropertyValidationManager.Validate(e.PropertyName);
		}

		#region Properties

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
		public bool IsValid
		{
			get
			{
				return PropertyValidationManager.IsValid;
			}
		}


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

		/// <inheritdoc />
		/// <param name="columnName">The name of the property whose error message to get.  The parameter is case sensitive.</param>
		/// <returns>The error message for the property. The default is an <see cref="string.Empty"/>.</returns>
		public virtual string this[string columnName]
		{
            get
            {
                var property = PropertyValidationManager.GetResultsInError(columnName);
                var errors = new StringBuilder();
                for (var propertyIndex = 0; propertyIndex < property.Count; propertyIndex++)
                {
                    var validationError = property[propertyIndex];
                    errors.AppendLine(validationError.ErrorMessage);
                }

                return errors.ToString();
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
#endif

		/// <inheritdoc />
		public virtual string Error
		{
			get
			{
				return ResultFormatter.GetConcatenatedErrorMessages(ValidatorResultsInError);
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