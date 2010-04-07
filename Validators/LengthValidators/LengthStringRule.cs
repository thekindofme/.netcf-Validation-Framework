using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Performs a string length validation.
    /// </summary>
    /// <seealso cref="LengthStringRuleConfigReader"/>
	/// <seealso cref="LengthStringRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class LengthStringRule : LengthRule
    {
        

        #region Constructors

  

        /// <param name="maximum">The maximum length allowed.</param>
        /// <param name="minimum">The minimum length allowed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="minimum"/> is less than 0.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="maximum"/> is not greater than or equal to <paramref name="minimum"/>.</exception>
        public LengthStringRule(int minimum, int maximum)
            : base(TypePointers.StringTypeHandle, minimum, maximum)
        {
            TrimWhiteSpace = true;
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (targetMemberValue != null)
            {
                var valueAsString = (string) targetMemberValue;
                if (TrimWhiteSpace)
                {
                    valueAsString = valueAsString.Trim();
                }
                return LengthValidationHelper.IsLengthValid(valueAsString.Length, Minimum, Maximum);
            }
            return true;
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            return (base.IsEquivalent(rule) && ((LengthStringRule) rule).TrimWhiteSpace == TrimWhiteSpace);
        }


        
        #endregion


        #region Properties

        /// <summary>
        /// Gets a <see cref="bool"/> to indicate if whitespace should be trimmed from the value being validated.
        /// </summary>
        public bool TrimWhiteSpace
        {
			get;
			set;
        }


		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                if (Minimum == Maximum)
                {
                    return string.Format("The value must be {0} characters in length.", Minimum);
                }
                else
                {
                    return string.Format("The value must be between {0} and {1} characters in length.", Minimum, Maximum);
                }
            }
        }


		/// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
          if (Minimum == Maximum)
            {
                return string.Format("The {0} '{1}' must be {2} characters in length.", descriptorType, tokenizedMemberName, Minimum);
            }
            else
            {
                return string.Format("The {0} '{1}' must be between {2} and {3} characters in length.", descriptorType, tokenizedMemberName, Minimum, Maximum);
            }
        }

        #endregion
    }
}