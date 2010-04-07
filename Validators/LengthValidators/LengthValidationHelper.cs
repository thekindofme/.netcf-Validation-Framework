namespace ValidationFramework
{
    /// <summary>
    /// Static class for all LengthValidation methods. 
    /// </summary>
    internal static class LengthValidationHelper
    {
        #region Methods

        internal static bool IsLengthValid(int length, int min, int max)
        {
            return ((length >= min) && (length <= max));
        }

        #endregion
    }
}