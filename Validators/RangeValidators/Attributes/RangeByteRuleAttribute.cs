using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RangeRule{T}"/>, that will check the range of a <see cref="byte"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="RangeRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RangeValidators\RangeByteRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RangeValidators\RangeByteRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class RangeByteRuleAttribute : RangeRuleAttribute
    {

        #region Constructors


        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        public RangeByteRuleAttribute(byte minimum, byte maximum)
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
        public byte Minimum
        {
			get;
			private set;
        }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <seealso cref="RangeRule{T}.Maximum"/>
        public byte Maximum
        {
			get;
			private set;
        }

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
			return new RangeRule<byte>(Minimum, Maximum)
			{
				EqualsMaximumIsValid = EqualsMaximumIsValid,
				EqualsMinimumIsValid = EqualsMinimumIsValid
			};
        }

        #endregion
    }
}