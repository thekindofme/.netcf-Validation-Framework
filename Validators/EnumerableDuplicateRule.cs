using System;
using System.Collections;
using System.Collections.Generic;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Performs duplicate validation check on <see cref="IEnumerable"/>s.
    /// </summary>
    /// <remarks>If the value being validated is null the rule will evaluate to true.</remarks>
    /// <seealso cref="EnumerableDuplicateRuleConfigReader"/>
	/// <seealso cref="EnumerableDuplicateRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public sealed class EnumerableDuplicateRule : Rule
    {
        #region Fields

        private static readonly Type type = typeof (IEnumerable);

        #endregion


        #region Constructor

        public EnumerableDuplicateRule()
            : base(type)
        {
        }

        #endregion


        #region Properties

		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                return string.Format("The collection must not contain duplicates.");
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IEqualityComparer"/> object that defines how to compare objects for equality.
        /// </summary>
        public IEqualityComparer Comparer
        {
			get;
			set;
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        public override bool Validate(object targetObjectValue, object targetMemberValue, object context, InfoDescriptor infoDescriptor)
		{
		    if (targetMemberValue != null)
		    {
		        var enumerable = (IEnumerable) targetMemberValue;
		        var list = new List<object>();
		        if (Comparer == null)
		        {
		            foreach (var enumerableValue in enumerable)
		            {
		                foreach (var listValue in list)
		                {
		                    if (Equals(enumerableValue, listValue))
		                    {
		                        return false;
		                    }
		                }
		                list.Add(enumerableValue);
		            }
		        }
		        else
		        {
		            foreach (var enumerableValue in enumerable)
		            {
		                foreach (var listValue in list)
		                {
		                    if (Comparer.Equals(enumerableValue, listValue))
		                    {
		                        return false;
		                    }
		                }
		                list.Add(enumerableValue);
		            }
		        }
		    }
		    return true;
		}


        /// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
            return string.Format("The {0} '{1}' may not contain duplicates.", descriptorType, tokenizedMemberName);
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            var enumerableDuplicateRule = (EnumerableDuplicateRule) rule;
            return enumerableDuplicateRule.Comparer == Comparer;
        }




        #endregion
    }
}