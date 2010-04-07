using System;
using System.Collections;
using System.Collections.Generic;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Performs a length validation in <see cref="ICollection"/>s.
    /// </summary>
    /// <remarks>If the value being validated is null the rule will evaluate to true.</remarks>
    /// <remarks>If <see cref="ExcludeDuplicatesFromCount"/> is true then <see cref="object.GetHashCode"/> is used to discard duplicates from the count. If the collection is null <see langword="true"/> will be returned. To validate for nulls use a <see cref="RequiredObjectRuleAttribute"/>.</remarks>
    /// <seealso cref="LengthCollectionRuleConfigReader"/>
	/// <seealso cref="LengthCollectionRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class LengthCollectionRule : LengthRule
    {
        #region Fields

        private static readonly Type collectionType = typeof (ICollection);
        private static readonly Type genericCollectionType = typeof (ICollection<>);

        #endregion


        #region Constructor


        /// <summary>
        /// Initializes a new, empty instance of the <see cref="EnumerableDuplicateRule"/> class using the specified <see cref="IEqualityComparer"/> object. 
        /// </summary>
        /// <param name="maximum">The maximum length allowed.</param>
        /// <param name="minimum">The minimum length allowed.</param>
        /// -or- 
        /// a null reference to use the default hash code provider and the default comparer. The default comparer is each item's implementation of Object.Equals. 
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="minimum"/> is less than 0.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="maximum"/> is not greater than or equal to <paramref name="minimum"/>.</exception>
        public LengthCollectionRule(int minimum, int maximum)
            : base(null, minimum, maximum)
        {
        }

        #endregion


        #region Properties

		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                if (Minimum == Maximum)
                {
                    return string.Format("The collection must contain {0}.", Maximum);
                }
                else
                {
                    return string.Format("The collection must contain between {0} and {1} items.", Minimum, Maximum);
                }
            }
        }

        /// <summary>
        /// Gets whether to exclude duplicates when calculating the length.
        /// </summary>
        public bool ExcludeDuplicatesFromCount
        {
			get;
			set;
        }


        /// <summary>
        /// Gets the <see cref="IEqualityComparer"/> for the <see cref="LengthCollectionRule"/>.
        /// </summary>
        public IEqualityComparer Comparer
        {
			get;
			set;
        }

        #endregion


        #region Methods

		/// <inheritdoc />
		internal override void CheckType(InfoDescriptor infoDescriptor)
        {
		    //var targetMemberRuntimeTypeHandle = infoDescriptor.RuntimeType;
            //var typeToCheck = Type.GetTypeFromHandle(targetMemberRuntimeTypeHandle);
            var typeToCheck = infoDescriptor.RuntimeType;
            var isGenericICollection = genericCollectionType.IsAssignableFrom(typeToCheck, true);
            var isICollection = collectionType.IsAssignableFrom(typeToCheck, true);
            
            if (!isICollection && !isGenericICollection)
            {
                throw new ArgumentException("Property must be a ICollection<T> or ICollection to be used for the LengthCollectionRule.");
            }
        }


		/// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (targetMemberValue != null)
            {
                int count;
                if (ExcludeDuplicatesFromCount)
                {
                    count = GetCountExcludeDuplicates(targetMemberValue);
                }
                else
                {
                    count = GetCount(targetMemberValue);
                }
                return LengthValidationHelper.IsLengthValid(count, Minimum, Maximum);
            }

            return true;
        }


        private static int GetCount(object targetMemberValue)
        {
            //You cant do this till you have a not null value.
            var iCollection = targetMemberValue as ICollection;
            if (iCollection == null)
            {
                //Dont cache countPropertyInfo as most classes that implement ICollection<> also implement ICollection so it should usually not get here
                var countPropertyInfo = targetMemberValue.GetType().GetProperty("Count");
                return (int) countPropertyInfo.GetValue(targetMemberValue, null);
            }
            else
            {
                return iCollection.Count;
            }
        }


        private int GetCountExcludeDuplicates(object targetMemberValue)
        {
            var enumerable = (IEnumerable)targetMemberValue;
            var list = new List<object>();
        	if (Comparer == null)
        	{
        		foreach (var enumerableValue in enumerable)
        		{
        			var found = false;
        			foreach (var listValue in list)
        			{
        				if (Equals(enumerableValue, listValue))
        				{
        					found = true;
        				}
        			}
        			if (!found)
        			{
        				list.Add(enumerableValue);
        			}
        		}
        	}
        	else
        	{
        		foreach (var enumerableValue in enumerable)
        		{
        			var found = false;
        			foreach (var listValue in list)
        			{
        				if (Comparer.Equals(enumerableValue, listValue))
        				{
        					found = true;
        				}
        			}
        			if (!found)
        			{
        				list.Add(enumerableValue);
        			}
        		}
        	}

        	return list.Count;
        }


		/// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
            if (Minimum == Maximum)
            {
                return string.Format("The number of items in {0} '{1}' must be {2}.", descriptorType, tokenizedMemberName, Minimum);
            }
            else
            {
                return string.Format("The number of items in {0} '{1}' must be between {2} and {3}.", descriptorType, tokenizedMemberName, Minimum, Maximum);
            }
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
            var lengthCollectionRule = (LengthCollectionRule) rule;
            return lengthCollectionRule.ExcludeDuplicatesFromCount == ExcludeDuplicatesFromCount && lengthCollectionRule.Comparer == Comparer;
        }


     
        #endregion
    }
}