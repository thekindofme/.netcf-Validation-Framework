using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Performs a required field validation on a <see cref="string"/>.
    /// </summary>
    /// <seealso cref="RequiredStringRuleConfigReader"/>
	/// <seealso cref="RequiredStringRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class RequiredStringRule : RequiredRule<string>
	{
        #region Fields

		private StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;
		private bool trimWhiteSpace;
		private bool ignoreCase = true;

        #endregion


        #region Properties

		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
            	if (InitialValue == null)
            	{
            		if (TrimWhiteSpace)
            		{
            			return string.Format("The value must not be null or an empty string. All spaces are ignored.");
            		}
            		else
            		{
            			return string.Format("The value must not be null or an empty string.");
            		}
            	}
            	else
            	{
            		return string.Format("The value must not be {0}", InitialValue);
            	}
            }
        }



    	/// <summary>
        /// Gets a <see cref="bool"/> to indicate if whitespace should be trimmed from the value being validated.
        /// </summary>
        public bool TrimWhiteSpace
    	{
    		get { return trimWhiteSpace; }
    		set { trimWhiteSpace = value; }
    	}

    	/// <summary>
        /// Gets a <see cref="bool"/> to indicate if case should be ignored.
        /// </summary>
        public bool IgnoreCase
    	{
    		get { return ignoreCase; }
    		set
    		{
				ignoreCase = value;
				if (ignoreCase)
				{
					stringComparison = StringComparison.OrdinalIgnoreCase;
				}
				else
				{
					stringComparison = StringComparison.Ordinal;
				}
    		}
    	}

    	#endregion


        #region Methods


		/// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            //Since String can be null or empty, need to do an explicit check
            //Always check for null before casting.
            if (InitialValue != null)
            {
                if (targetMemberValue == null)
                {
                    return true;
                }
                else
                {
                    var valueString = (string) targetMemberValue;
                    if (TrimWhiteSpace)
                    {
                        valueString = valueString.Trim();
                    }
                    return !String.Equals(valueString, InitialValue, stringComparison);
                }
            }
            else
            {
                if (targetMemberValue == null)
                {
                    return false;
                }
                else
                {
					var valueString = (string)targetMemberValue;
                    if (TrimWhiteSpace)
                    {
                        valueString = valueString.Trim();
                    }
                    return (valueString.Length > 0);
                }
            }
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            var requiredStringRule = (RequiredStringRule) rule;

				bool initialValuesEqual;
			if (requiredStringRule.InitialValue == null && InitialValue == null)
			{
				initialValuesEqual = true;
			}
			else if (requiredStringRule.InitialValue != null && InitialValue != null)
			{
				initialValuesEqual = InitialValue.Trim().Equals(requiredStringRule.InitialValue.Trim());
			}
			else
    		{
				initialValuesEqual = false;
    		}

			return initialValuesEqual &&
			requiredStringRule.TrimWhiteSpace == TrimWhiteSpace &&
                requiredStringRule.IgnoreCase == IgnoreCase;
        }

        #endregion
    }
}