using System;

namespace ValidationFramework
{
    /// <summary>
    /// Performs common argument validation.
    /// </summary>
    public static class Guard
    {
        #region Methods

        /// <summary>
        /// Checks a string argument to ensure it isn't null or empty.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentValue"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentValue"/> is <see cref="string.Empty"/>.</exception>
        public static void ArgumentNotNullOrEmptyString(string argumentValue, string argumentName)
        {
            ArgumentNotNull(argumentValue, argumentName);

            if (argumentValue.Length == 0)
            {
                throw new ArgumentException("String cannot be empty.", argumentName);
            }
        }


        /// <summary>
        /// Checks a string argument to ensure it isn't empty.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentException"><paramref name="argumentValue"/> is <see cref="string.Empty"/>.</exception>
        public static void ArgumentNotEmptyString(string argumentValue, string argumentName)
        {
            if ((argumentValue != null) && (argumentValue.Length == 0))
            {
                throw new ArgumentException("String cannot be empty.", argumentName);
            }
        }


        /// <summary>
        /// Checks an argument to ensure it isn't null.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentValue"/> is a null reference.</exception>
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        #endregion
    }
}