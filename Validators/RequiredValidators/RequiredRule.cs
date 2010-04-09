using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Base class for performing a required validation.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class RequiredRule<T> : ValueRule
    {
        #region Fields

        private static readonly Type runtimeType = typeof(T);
        private T initialValue;
        bool hasInitialValue;

        #endregion


        #region Constructors


        public RequiredRule()
            : base(runtimeType)
        {
        }

        #endregion


        #region Properties

   
        /// <summary>
        /// Gets or sets the initial and invalid value.
        /// </summary>
        public T InitialValue
        {
            get
            {
                return initialValue;
            }
			set
			{
				initialValue = value;
                hasInitialValue = true;
            }
        }

		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                if (initialValue == null)
                {
                    return string.Format("The value must not be null.");
                }
                else
                {
                    return string.Format("The value must not be '{0}'.", initialValue);
                }
            }
        }

        #endregion


        #region Methods
        
		/// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
		{
            //var hasInitialValue = initialValue != null;
			if ((targetMemberValue == null) && (!hasInitialValue))
            {
                return false;
            }
            else if ((targetMemberValue == null) && (hasInitialValue))
            {
                return true;
            }
            else if ((targetMemberValue != null) && (!hasInitialValue))
            {
                return true;
            }
            else
            {
                var targetMemberValueT = (T) targetMemberValue;
                return !targetMemberValueT.Equals(initialValue);
            }
		}


    	/// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
    	{
    		if (initialValue == null)
    		{
    			return string.Format("The {0} '{1}' is required.", descriptorType, tokenizedMemberName);
    		}
    		else
    		{
    			return string.Format("The {0} '{1}' is required and cannot be '{2}'.", descriptorType, tokenizedMemberName, initialValue);
    		}
    	}


    	/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
    	{
    		var requiredRule = (RequiredRule<T>) rule;

			if (requiredRule.initialValue == null && initialValue == null)
    		{
    			return true;
    		}
    		else
    		{
    			return initialValue.Equals(requiredRule.initialValue);
    		}
    	}

    	#endregion

    }
}