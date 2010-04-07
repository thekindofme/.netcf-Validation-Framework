using System;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Base class for all <see cref="Rule"/>s that performs comparison validation.
    /// </summary>
    /// <seealso cref="CompareRuleConfigReader"/>
    /// <seealso cref="CompareByteRuleAttribute"/>
    /// <seealso cref="CompareDateTimeRuleAttribute"/>
    /// <seealso cref="CompareDecimalRuleAttribute"/>
    /// <seealso cref="CompareDoubleRuleAttribute"/>
    /// <seealso cref="CompareFloatRuleAttribute"/>
    /// <seealso cref="CompareIntRuleAttribute"/>
    /// <seealso cref="CompareLongRuleAttribute"/>
    /// <seealso cref="CompareShortRuleAttribute"/>
	/// <seealso cref="CompareStringRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class CompareRule<T> : ValueRule
        where T : IComparable<T>
    {
        #region Fields

        private const string errorMessageFormat = "The {0} '{1}' must be '{2}' '{3}'.";
        private static readonly Type runtimeType = typeof(T);

        #endregion


        #region Constructors

    
        /// <param name="compareOperator">The comparison operation to perform.</param>
        /// <param name="valueToCompare">The value to compare with.</param> 
        /// <exception cref="ArgumentNullException"><paramref name="valueToCompare"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="compareOperator"/> is out of the accepted range.</exception>
        public CompareRule(T valueToCompare, CompareOperator compareOperator)
            : base(runtimeType)
        {
            Guard.ArgumentNotNull(valueToCompare, "valueToCompare");
            if ((compareOperator < CompareOperator.Equal) || (compareOperator > CompareOperator.NotEqual))
            {
                throw new ArgumentOutOfRangeException("compareOperator");
            }
            ValueToCompare = valueToCompare;
            CompareOperator = compareOperator;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the comparison operation to perform. 
        /// </summary>
        public CompareOperator CompareOperator
        {
        	get;
        	private set;
        }

		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                return
					string.Format("The value must be '{0}' '{1}'", CompareOperator.ToUserFriendlyString(),
                                  ValueToCompare);
            }
        }


        /// <summary>
        /// Gets the value to compare with.
        /// </summary>
        public T ValueToCompare
        {
        	get;
        	private set;
        }

        #endregion


        #region Methods
        
		/// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
            return
				string.Format(errorMessageFormat, descriptorType, tokenizedMemberName, CompareOperator.ToUserFriendlyString(),
                              ValueToCompare);
        }


		/// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (targetMemberValue != null)
            {
                return CompareValidationHelper.Compare(targetMemberValue, ValueToCompare, CompareOperator);
            }
            return true;
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            var compareRule = (CompareRule<T>) rule;
            return compareRule.ValueToCompare.Equals(ValueToCompare) && CompareOperator == compareRule.CompareOperator;
        }


        #endregion
    }
}