using System;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Base class for performing a range validation.
    /// </summary>
    /// <seealso cref="RangeRuleConfigReader"/>
    /// <seealso cref="RangeByteRuleAttribute"/>
    /// <seealso cref="RangeDateTimeRuleAttribute"/>
    /// <seealso cref="RangeDoubleRuleAttribute"/>
    /// <seealso cref="RangeFloatRuleAttribute"/>
    /// <seealso cref="RangeIntRuleAttribute"/>
    /// <seealso cref="RangeLongRuleAttribute"/>
	/// <seealso cref="RangeShortRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class RangeRule<T> : ValueRule
        where T : IComparable<T>
    {
        #region Fields

        private const string errorMessageFormat = "The {0} '{1}' must be '{2}' {3} and '{4}' {5}.";
        private const string ruleInterpretationFormat = "The value must be '{0}' {1} and '{2}' {3}.";
        private static readonly Type runtimeType = typeof (T);

        #endregion


        #region Constructors
        
        /// <param name="minimum">The minimum valid value</param>
        /// <param name="maximum">The maximum valid value</param>
        public RangeRule(T minimum, T maximum)
            : base(runtimeType)
        {
            Minimum = minimum;
            Maximum = maximum;
            EqualsMinimumIsValid = true;
            EqualsMaximumIsValid = true;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the minimum valid value
        /// </summary>
        public T Minimum
        {
			get;
			private set;
        }

        /// <summary>
        /// Gets the maximum valid value
        /// </summary>
        public T Maximum
        {
			get;
			private set;
        }

		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                CompareOperator minCompareOperator;
                CompareOperator maxCompareOperator;
                GetCompareOperators(out maxCompareOperator, out minCompareOperator);
				return string.Format(ruleInterpretationFormat, minCompareOperator.ToUserFriendlyString(), Minimum, maxCompareOperator.ToUserFriendlyString(), Maximum);
            }
        }

        /// <summary>
        /// Get or sets value indicating if the minimum value is valid.
        /// </summary>
        public bool EqualsMinimumIsValid
        {
			get;
			set;
        }

        /// <summary>
        /// Get or sets a value indicating if the maximum value is valid.
        /// </summary>
        public bool EqualsMaximumIsValid
        {
			get;
			set;
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
            CompareOperator minCompareOperator;
            CompareOperator maxCompareOperator;
            GetCompareOperators(out maxCompareOperator, out minCompareOperator);
			return string.Format(errorMessageFormat, descriptorType, tokenizedMemberName, minCompareOperator.ToUserFriendlyString(), Minimum, maxCompareOperator.ToUserFriendlyString(), Maximum);
        }


        private void GetCompareOperators(out CompareOperator maxCompareOperator, out CompareOperator minCompareOperator)
        {
            if (EqualsMaximumIsValid)
            {
                maxCompareOperator = CompareOperator.LessThanEqual;
            }
            else
            {
                maxCompareOperator = CompareOperator.LessThan;
            }
            if (EqualsMinimumIsValid)
            {
                minCompareOperator = CompareOperator.GreaterThanEqual;
            }
            else
            {
                minCompareOperator = CompareOperator.GreaterThan;
            }
        }


		/// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (targetMemberValue != null)
            {
                return RangeValidationHelper.IsRangeValid((T) targetMemberValue, Minimum, Maximum, EqualsMinimumIsValid, EqualsMaximumIsValid);
            }
            return true;
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            var rangeRule = (RangeRule<T>) rule;
            return (rangeRule.EqualsMaximumIsValid == EqualsMaximumIsValid) &&
                   (rangeRule.EqualsMinimumIsValid == EqualsMinimumIsValid) &&
                   CompareT(Minimum, rangeRule.Minimum) &&
                   CompareT(Maximum, rangeRule.Maximum);
        }


        private static bool CompareT(T left, T right)
        {
            if (left == null && right != null)
            {
                return false;
            }
            else if (left != null && right == null)
            {
                return false;
            }
            else if (left == null && right == null)
            {
                return true;
            }
            else
            {
                return left.Equals(right);
            }
        }

    
        #endregion

    }
}