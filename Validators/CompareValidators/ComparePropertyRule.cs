using System;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;
#if(WindowsCE || SILVERLIGHT)
using System.Reflection;
#endif

namespace ValidationFramework
{
    /// <summary>
    /// For comparing two properties.
    /// </summary>
    /// <seealso cref="ComparePropertyRuleAttribute"/>
	/// <seealso cref="ComparePropertyRuleConfigReader"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class ComparePropertyRule : Rule
    {
        #region Fields

        private const string errorMessageFormat = "The property '{0}' must be '{1}' the property '{2}'.";
        #region Fields
#if(WindowsCE || SILVERLIGHT)
		private MethodInfo propertyToCompareMethodInfo;
#else
        private FastInvokeHandler propertyToCompareHandler;
#endif
        #endregion

        #endregion


        #region Constructors




        /// <param name="operator">The comparison operation to perform.</param>
        /// <param name="propertyToCompare">The name of the property to compare with.</param> 
        /// <exception cref="ArgumentException"><paramref name="propertyToCompare"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="propertyToCompare"/> is null.</exception>
        public ComparePropertyRule(string propertyToCompare, CompareOperator @operator)
            : base(null)
        {
            Guard.ArgumentNotNullOrEmptyString(propertyToCompare, "propertyToCompare");
            PropertyToCompare = propertyToCompare;
            CompareOperator = @operator;
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
				return string.Format("The value must be '{0}' the value of the property '{1}'", CompareOperator.ToUserFriendlyString(), PropertyToCompare);
            }
        }


        /// <summary>
        /// Gets the name of the property to compare with.
        /// </summary>
        public string PropertyToCompare
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
				string.Format(errorMessageFormat, tokenizedMemberName, CompareOperator.ToUserFriendlyString(),
                              PropertyToCompare);
        }


		/// <inheritdoc />
        public override bool Validate(object targetObjectValue, object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (targetMemberValue != null)
			{
#if(WindowsCE || SILVERLIGHT)
				if (propertyToCompareMethodInfo == null)
                {
                    var propertyInfo = targetObjectValue.GetType().GetProperty(PropertyToCompare, TypeDescriptor.PropertyFlags);
                    if (propertyInfo == null)
                    {
                        throw new InvalidOperationException(string.Format("Could not find the property '{0}'", PropertyToCompare));
                    }
                    propertyToCompareMethodInfo = propertyInfo.GetGetMethod(true);
                }
               var propertyToCompareValue = propertyToCompareMethodInfo.Invoke(targetObjectValue, null);
#else
				if (propertyToCompareHandler == null)
                {
                    var propertyInfo = targetObjectValue.GetType().GetProperty(PropertyToCompare, TypeDescriptor.PropertyFlags);
                    if (propertyInfo == null)
                    {
                        throw new InvalidOperationException(string.Format("Could not find the property '{0}'", PropertyToCompare));
                    }
                    var getMethodInfo = propertyInfo.GetGetMethod(true);

                    propertyToCompareHandler = MethodInvokerCreator.GetMethodInvoker(getMethodInfo);
                }
                var propertyToCompareValue = propertyToCompareHandler(targetObjectValue);
#endif

                if ((propertyToCompareValue != null) && !CompareValidationHelper.Compare(targetMemberValue, propertyToCompareValue, CompareOperator))
                {
                    return false;
                }
            }
            return true;
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            var comparePropertyRule = (ComparePropertyRule) rule;
            return ((comparePropertyRule.PropertyToCompare == PropertyToCompare) && (CompareOperator == comparePropertyRule.CompareOperator));
        }


		/// <inheritdoc />
		internal override void CheckType(InfoDescriptor infoDescriptor)
        {
            base.CheckType(infoDescriptor);
            if (infoDescriptor is ParameterDescriptor)
            {
                throw new InvalidOperationException("ComparePropertyRule cannot be applied to a parameter.");
            }
        }



        #endregion
    }
}