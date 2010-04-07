namespace ValidationFramework.Configuration
{
    /// <summary>
    /// Defines the signature for retrieving an error message.
    /// </summary>
    public interface IErrorMessageProvider
    {

        /// <summary>
        /// Retrieve an error message.
        /// </summary>
        /// <param name="rule">The <see cref="Rule"/> that is requesting the error message.</param>
        /// <param name="targetObjectValue">The value of the object containing the member to validate.</param>
        /// <param name="targetMemberValue">The value of the member to validate.</param>
        /// <param name="context">An <see cref="object"/> that contains data for the <see cref="Rule"/> to validate.</param>
        /// <returns>An error message.</returns>
        /// <remarks>
        /// Note: <see cref="PropertyValidatorGeneratorControl"/> is client server in nature. Due to the this limitation when <see cref="ISupportWebClientValidation.CreateWebClientValidators"/> is called only <paramref name="rule"/> will be passed through when populating <see cref="BaseValidator.ErrorMessage"/>. All other parameters (<paramref name="targetObjectValue"/>, <paramref name="targetMemberValue"/> and <paramref name="context"/>) will be null. Server validation however will pass through all parameters.
        /// </remarks>
        string RetrieveErrorMessage(Rule rule, object targetObjectValue, object targetMemberValue, object context);

    }
}