using System;
using System.ComponentModel;
#if (!WindowsCE)
using System.Linq.Expressions;
using ValidationFramework.Extensions;
#endif

namespace ValidationFramework
{
    /// <summary>
    /// Provides base class that developer can inherit from to provide <see cref="INotifyPropertyChanged"/> for all sub-classes.
    /// </summary>
    /// <remarks>
    /// This ideal for windows forms applications to get immediate validation feedback on data bound controls.
    /// </remarks>
    /// <example>
	/// <code source="Examples\ExampleLibraryCSharp\BusinessBaseClasses\NotifyBaseLambdaExample.cs" title="The following code example shows how to inherit from NotifyBase and use Lambda to notify property changes." lang="cs" region="Example"/>
	/// <code source="Examples\ExampleLibraryCSharp\BusinessBaseClasses\NotifyBaseStringExample.cs" title="The following code example shows how to inherit from NotifyBase and use strings to notify property changes." lang="cs" region="Example"/>
	/// <code source="Examples\ExampleLibraryVB\BusinessBaseClasses\NotifyBaseStringExample.vb" title="The following code example shows how to inherit from NotifyBase and use strings to notify property changes." lang="vbnet" region="Example"/>
    /// </example>
	/// <seealso cref="IValidatable"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public abstract class NotifyBase : INotifyPropertyChanged
    {

        #region Methods

    	/// <summary>
        /// Performs INotifyPropertyChanged functionality for specified property.
        /// </summary>
		/// <param name="propertyName">The name of the property that has changed. The parameter is case sensitive.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is a null reference.</exception>
		/// <exception cref="ArgumentException"><paramref name="propertyName"/> is <see cref="string.Empty"/>.</exception>
		public void NotifyPropertyChanged(string propertyName)
        {
			Guard.ArgumentNotNull(propertyName, "propertyName");  
            OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        
#if (!WindowsCE)
    	/// <summary>
        /// Performs INotifyPropertyChanged functionality for specified property.
		/// </summary>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> representing the property to validate.</param>
		public void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> expression)
        {
			NotifyPropertyChanged(TypeExtensions.GetMemberName(expression));
        }
#endif

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