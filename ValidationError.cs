using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// The results of any <see cref="ValidationFramework.Rule.Validate"/> operation.
    /// </summary>
    /// <remarks>An instance is only created if the <see cref="ValidationFramework.Rule.Validate"/> operation fails.</remarks>
    ///// <example>
    ///// <code source="Examples\ExampleLibraryCSharp\Reflection\GetReflectionInfoFromValidationResult.cs" title="This example shows how to get a Type or a MethodBase from a ValidationResult." lang="cs"/>
    ///// <code source="Examples\ExampleLibraryVB\Reflection\GetReflectionInfoFromValidationResult.vb"  title="This example shows how to get a Type or a MethodBase from a ValidationResult." lang="vbnet"/>
    ///// </example>
    public class ValidationError
    {
    
        #region Constructors


        /// <remarks>
        /// null is an invalid value for both <paramref name="rule"/> and <paramref name="errorMessage"/>. 
        /// Due to performance concerns <see cref="ArgumentNullException"/> will no be thrown.
        /// So just don't pass in null or an empty string.
        /// </remarks>
        /// <param name="rule">The <see cref="Rule"/> that this <see cref="ValidationError"/> has been generated from.</param>
        /// <param name="errorMessage">The error message.</param>
		/// <param name="attemptedValue">The invalid member's value</param>
        /// <param name="infoDescriptor">The <see cref="InfoDescriptor"/> that this <see cref="ValidationError"/> has been generated from.</param>
        public ValidationError(Rule rule, string errorMessage, object attemptedValue, InfoDescriptor infoDescriptor)
        {
            //Dont guard these. the performance hit is not worth the minor help it gives developers. 
            Rule = rule;
            ErrorMessage = errorMessage;
			AttemptedValue = attemptedValue;
            InfoDescriptor = infoDescriptor;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the error message for the <see cref="ValidationError"/>.
        /// </summary>
        public string ErrorMessage
        {
			get;
			private set;
        }


        /// <summary>
        /// Gets the <see cref="Rule"/> that this <see cref="ValidationError"/> has been generated from.
        /// </summary>
        /// <seealso cref="ValidationFramework.Rule.Validate"/>
        public Rule Rule
        {
			get;
			private set;
        }

		/// <summary>
		/// Gets the invalid member's value
		/// </summary>
		public object AttemptedValue
		{
			get;
			private set;
		}

        /// <summary>
        /// Gets the <see cref="InfoDescriptor"/> that this <see cref="ValidationError"/> was generated for.
        /// </summary>
        public InfoDescriptor InfoDescriptor { get; private set; }

        #endregion
    }
}