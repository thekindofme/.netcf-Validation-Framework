using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RangeRule{T}"/>, that will check the range a <see cref="decimal"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="RangeRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RangeValidators\RangeDecimalRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RangeValidators\RangeDecimalRuleAttributeExample.vb" lang="vbnet"/>
    /// </example>
#if (!SILVERLIGHT)
	[Serializable]
#endif
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class RangeDecimalRuleAttribute : RangeRuleAttribute
    {

        #region Constructors


        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        public RangeDecimalRuleAttribute(double minimum, double maximum)
        {
            Minimum = (decimal) minimum;
            Maximum = (decimal) maximum;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <seealso cref="RangeRule{T}.Minimum"/>
        public decimal Minimum
        {
			get;
			private set;
        }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <seealso cref="RangeRule{T}.Maximum"/>
        public decimal Maximum
        {
			get;
			private set;
        }

        #endregion


        #region Methods


		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
			return new RangeRule<decimal>(Minimum, Maximum)
			{
				EqualsMaximumIsValid = EqualsMaximumIsValid,
				EqualsMinimumIsValid = EqualsMinimumIsValid
			};
        }

        #endregion
    }
}