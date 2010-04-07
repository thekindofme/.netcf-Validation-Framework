using System.Collections.Generic;

namespace ValidationFramework
{
    /// <summary>
    /// Provides the means to check if the validity of the current state of an <see langword="object"/>.
    /// </summary>
    /// <seealso cref="NotifyValidatableBase"/>
    /// <seealso cref="DataErrorInfoValidatableBase"/>
    /// <seealso cref="ValidatableBase"/>
    public interface IValidatable
    {
        /// <summary>
        /// Gets a <see langword="bool"/> indicating if the current state is valid.
        /// </summary>
        bool IsValid
        {
            get;
        }

        /// <summary>
        /// Gets a <see see="IList{T}"/> of <see langword="string"/>s that contain all the error messages.
        /// </summary>
        IList<string> ErrorMessages
        {
            get;
        }
    }
}