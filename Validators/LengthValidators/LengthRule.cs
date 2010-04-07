using System;

namespace ValidationFramework
{
    /// <summary>
    /// Performs a length validation.
    /// </summary>
    /// <seealso cref="LengthCollectionRule"/>
	/// <seealso cref="LengthStringRule"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public abstract class LengthRule : ValueRule
    {

        #region Constructors

      
        /// <param name="propertyTypeHandle">The <see cref="RuntimeTypeHandle"/> that this <see cref="LengthRule"/> can be applied to. Use <see langword="null"/> to indicate it can be applied to any property type.</param>
        /// <param name="maximum">The maximum length allowed.</param>
        /// <param name="minimum">The minimum length allowed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="minimum"/> is less than 0.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="maximum"/> is not greater than or equal to <paramref name="minimum"/>.</exception>
        protected LengthRule(RuntimeTypeHandle? propertyTypeHandle, int minimum, int maximum)
            : base(propertyTypeHandle)
        {
            if (minimum < 0)
            {
                throw new ArgumentException("Minimum must be greater than 0.", "minimum");
            }
            if (minimum > maximum)
            {
                throw new ArgumentException("Minimum must be less than or equal to Maximum.", "minimum");
            }
            Minimum = minimum;
            Maximum = maximum;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the maximum length.
        /// </summary>
        public int Maximum
        {
			get;
			private set;
        }


        /// <summary>
        /// Gets or sets the minimum length.
        /// </summary>
        public int Minimum
        {
			get;
			private set;
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            var lengthRule = (LengthRule) rule;
            return lengthRule.Maximum == Maximum && lengthRule.Minimum == Minimum;
        }


     

        #endregion
    }
}