using System;
using System.Text.RegularExpressions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Performs a Regular Expression validation.
    /// </summary>
    /// <remarks>If the value being validated is null the rule will evaluate to true.</remarks>
    /// <seealso cref="RegexRuleConfigReader"/>
	/// <seealso cref="RegexRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class RegexRule : ValueRule
    {


        #region Constructors



        /// <param name="validationExpression">The regular expression pattern to match.</param>
        /// <exception cref="ArgumentNullException"><paramref name="validationExpression"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="validationExpression"/> is <see cref="string.Empty"/>.</exception>
        public RegexRule(string validationExpression)
            : base(TypePointers.StringTypeHandle)
        {
			Guard.ArgumentNotNullOrEmptyString(validationExpression, "validationExpression");

ValidationExpression = validationExpression;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the regular expression pattern to match.  
        /// </summary>
        public string ValidationExpression
        {
			get;
			private set;
        }

		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                return string.Format("The value must match the regular expression '{0}'.", ValidationExpression);
            }
        }

    	private RegexOptions regexOptions;

    	///<summary>
        /// Gets A bitwise OR combination of <see cref="System.Text.RegularExpressions.RegexOptions"/> enumeration values.
        ///</summary>
        public RegexOptions RegexOptions
    	{
    		get { return regexOptions; }
    		set
    		{
				if ((value < RegexOptions.None) || ((((int)value) >> 10) != 0))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				regexOptions = value;
    		}
    	}

    	#endregion


        #region Methods
        


        /// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (targetMemberValue == null)
            {
                return true;
            }
            else
            {
                var s = (string) targetMemberValue;
                if (s.Length == 0)
                {
                    return true;
                }
                else
                {
                    var m = Regex.Match(s, ValidationExpression, RegexOptions);
                    return (m.Success && (m.Index == 0) && (m.Length == s.Length));
                }
            }

        }


		/// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
            return string.Format("The {0} '{1}' is an invalid format.", descriptorType, tokenizedMemberName);
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            var regexRule = (RegexRule) rule;
            return regexRule.RegexOptions == RegexOptions && regexRule.ValidationExpression == ValidationExpression;
        }


        #endregion
    }
}