using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RangeRule{T}"/>, that will check the range of a <see cref="long"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="RangeRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RangeValidators\RangeLongRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RangeValidators\RangeLongRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class RangeLongRuleAttribute : RangeRuleAttribute
    {
 
        #region Constructors

 
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        public RangeLongRuleAttribute(long minimum, long maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <seealso cref="RangeRule{T}.Minimum"/>
        public long Minimum
        {
			get;
			private set;
        }


        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <seealso cref="RangeRule{T}.Maximum"/>
        public long Maximum
        {
			get;
			private set;
        }

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
			return new RangeRule<long>(Minimum, Maximum)
			{
				EqualsMaximumIsValid = EqualsMaximumIsValid,
				EqualsMinimumIsValid = EqualsMinimumIsValid
			};
        }

        #endregion
    }
}