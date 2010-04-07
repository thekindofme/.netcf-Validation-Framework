#if (!SILVERLIGHT)
using System;
#endif
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ValidationFramework
{
    /// <summary>
    /// A strongly typed collection of <see cref="IValidatable"/> objects.
    /// </summary>
    /// <remarks>
    /// Allows the all items in the collection to be easily validated.
    /// </remarks>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class ValidatableCollection<T> : Collection<T>, IValidatable where T : IValidatable
    {
   

        #region Properties

        /// <summary>
        /// Check to see if each item is valid.
        /// </summary>
        /// <remarks>
        /// Even if an invalid item is found, continue to check for the rest of the items so that all error messages can be accumulated.
        /// </remarks>
        public virtual bool IsValid
        {
            get
            {
                var isValid = true;
                for (var itemIndex = 0; itemIndex < Count; itemIndex++)
                {
                    var item = this[itemIndex];
                    if (!item.IsValid)
                    {
                        isValid = false;
                    }
                }
                return isValid;
            }
        }


		/// <inheritdoc />
        public IList<string> ErrorMessages
        {
            get
            {
                var errorList = new List<string>();
                for (var itemIndex = 0; itemIndex < Count; itemIndex++)
                {
                    var item = this[itemIndex];
                    errorList.AddRange(item.ErrorMessages);
                }
                return errorList;
            }
        }

        #endregion
    }
}