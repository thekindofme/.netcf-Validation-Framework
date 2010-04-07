
namespace ValidationFramework
{
    /// <summary>
    /// Provides the means to check if the validity of the current state of an <see langword="object"/>.
    /// </summary>
    /// <seealso cref="NotifyValidatableBase"/>
    /// <seealso cref="DataErrorInfoValidatableBase"/>
    /// <seealso cref="ValidatableBase"/>
    public interface IPropertyValidatable : IValidatable
    {
        /// <summary>
        /// Gets the <see cref="MemberValidationManager"/>.
        /// </summary>
        /// <remarks>
        /// This is exposed for advanced usage scenarios. In general it should only be used by inherited classes.
        /// </remarks>
        MemberValidationManager PropertyValidationManager
        {
            get;
        }
    }
}