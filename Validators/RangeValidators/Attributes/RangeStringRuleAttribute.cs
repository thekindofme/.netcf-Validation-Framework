using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RangeStringRule"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="RangeStringRule"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RangeValidators\RangeStringRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RangeValidators\RangeStringRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class RangeStringRuleAttribute : RangeRuleAttribute
    {

        #region Constructors


        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="minimum"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="minimum"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="maximum"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="maximum"/> is <see cref="string.Empty"/>.</exception>
        public RangeStringRuleAttribute(string minimum, string maximum)
        {
            Guard.ArgumentNotNullOrEmptyString(minimum, "minimum");
            Guard.ArgumentNotNullOrEmptyString(maximum, "maximum");
            Minimum = minimum;
            Maximum = maximum;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <seealso cref="RangeRule{T}.Minimum"/>
        public string Minimum
        {
			get;
			private set;
        }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <seealso cref="RangeRule{T}.Maximum"/>
        public string Maximum
        {
			get;
			private set;
        }

        #endregion


        #region Methods


		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
			return new RangeStringRule(Minimum, Maximum)
			{
				EqualsMaximumIsValid = EqualsMaximumIsValid,
				EqualsMinimumIsValid = EqualsMinimumIsValid
			};
        }

        #endregion
    }
}