using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Performs a range validation on a <see cref="string"/>.
    /// </summary>
    /// <remarks>If the value being validated is null the rule will evaluate to true.</remarks>
    /// <seealso cref="RangeStringRuleConfigReader"/>
	/// <seealso cref="RangeStringRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class RangeStringRule : RangeRule<string>
    {
        #region Constructors


        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="minimum"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="minimum"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="maximum"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="maximum"/> is <see cref="string.Empty"/>.</exception>
        public RangeStringRule(string minimum, string maximum)
            : base(minimum, maximum)
        {
            Guard.ArgumentNotNullOrEmptyString(minimum, "minimum");
            Guard.ArgumentNotNullOrEmptyString(maximum, "maximum");
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (targetMemberValue != null)
            {
                return RangeValidationHelper.IsRangeValid((string) targetMemberValue, Minimum, Maximum, EqualsMinimumIsValid, EqualsMaximumIsValid);
            }
            return true;
        }

    

        #endregion
    }
}