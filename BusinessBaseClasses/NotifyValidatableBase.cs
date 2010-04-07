using System;
using System.ComponentModel;
#if (!WindowsCE)
using System.Linq.Expressions;
using ValidationFramework.Extensions;
#endif

namespace ValidationFramework
{
    /// <summary>
    /// Provides base class that developer can (optionally) inherit from to provide <see cref="INotifyPropertyChanged"/> and <see cref="IDataErrorInfo"/> functionality for all sub-classes.
    /// </summary>
    /// <remarks>
    /// This ideal for windows forms applications to get immediate validation feedback on data bound controls.
    /// </remarks>
    /// <example>
	/// <code source="Examples\ExampleLibraryCSharp\BusinessBaseClasses\NotifyValidatableBaseStringExample.cs" title="The following code example shows how to inherit from NotifyValidatableBase using strings to validate properties." lang="cs" region="Example"/>
	/// <code source="Examples\ExampleLibraryVB\BusinessBaseClasses\NotifyValidatableBaseStringExample.vb" title="The following code example shows how to inherit from NotifyValidatableBase using strings to validate properties." lang="vbnet" region="Example"/>
	/// <code source="Examples\ExampleLibraryCSharp\BusinessBaseClasses\NotifyValidatableBaseLambdaExample.cs" title="The following code example shows how to inherit from NotifyValidatableBase using Lambda to validate properties." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\BusinessBaseClasses\NotifyValidatableBaseLambdaExample.vb" title="The following code example shows how to inherit from NotifyValidatableBase using Lambda to validate properties." lang="vbnet" region="Example"/>
    /// </example>
    /// <seealso cref="IValidatable"/>
    /// <seealso cref="DataErrorInfoValidatableBase"/>
	/// <seealso cref="ValidatableBase"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public abstract class NotifyValidatableBase : DataErrorInfoValidatableBase, INotifyPropertyChanged
    {
  

        #region Methods

        /// <summary>
        /// Perform validation and <see cref="INotifyPropertyChanged"/> functionality for specified property.
        /// </summary>
		/// <param name="propertyName">The name of the property to validate.  The parameter is case sensitive.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is a null reference.</exception>
		/// <exception cref="ArgumentException"><paramref name="propertyName"/> is <see cref="string.Empty"/>.</exception>
        protected void NotifyAndValidate(string propertyName)
        {
            ValidateProperty(propertyName);
            NotifyPropertyChanged(propertyName);
            NotifyPropertyChanged("IsValid");
        }
        
#if (!WindowsCE)

		/// <summary>
		/// Perform validation and <see cref="INotifyPropertyChanged"/> functionality for specified property.
		/// </summary>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> representing the property to validate.</param>
		/// <exception cref="ArgumentNullException"><paramref name="expression"/> is a null reference.</exception>
		protected void NotifyAndValidate<TProperty>(Expression<Func<TProperty>> expression)
        {
			NotifyAndValidate(TypeExtensions.GetMemberName(expression));
        }

    	/// <summary>
        /// Performs INotifyPropertyChanged functionality for specified property.
		/// </summary>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> representing the property to validate.</param>
		protected void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> expression)
        {
			NotifyPropertyChanged(TypeExtensions.GetMemberName(expression));
        }
#endif


    	/// <summary>
        /// Performs INotifyPropertyChanged functionality for specified property.
        /// </summary>
		/// <param name="propertyName">The name of the property that has changed. The parameter is case sensitive.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is a null reference.</exception>
		/// <exception cref="ArgumentException"><paramref name="propertyName"/> is <see cref="string.Empty"/>.</exception>
        protected void NotifyPropertyChanged(string propertyName)
        {
			Guard.ArgumentNotNull(propertyName, "propertyName");  
            OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="PropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, e);
            }
        }

        #endregion


        #region Properties

    	/// <inheritdoc />
    	public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}