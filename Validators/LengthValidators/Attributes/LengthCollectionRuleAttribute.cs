using System;
using System.Collections;
using System.Collections.Generic;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="LengthCollectionRule"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="LengthCollectionRule"/>
	/// <remarks>If <see cref="ExcludeDuplicatesFromCount"/> is true then <see cref="object.GetHashCode"/> is used to discard duplicates from the count. If the collection is null <see langword="true"/> will be returned. To validate for nulls use a <see cref="RequiredObjectRuleAttribute"/>.</remarks>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class LengthCollectionRuleAttribute : LengthRuleAttribute
    {


        #region Constructor

  
        /// <param name="maximum">The maximum length allowed.</param>
        public LengthCollectionRuleAttribute(int maximum)
            : base(maximum)
        {
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets whether to exclude duplicates when calculating the length.
        /// </summary>
        /// <remarks>Setting this to <see langword="true"/> will decrease the performance of <see cref="Rule.Validate"/></remarks>
        /// <seealso cref="LengthCollectionRule.ExcludeDuplicatesFromCount"/>
        public bool ExcludeDuplicatesFromCount
        {
        	get;
        	set;
        }

		/// <summary>
		/// Gets or sets the type name for the type to get <see cref="IEqualityComparer{T}"/> from.
		/// </summary>
		/// <remarks>Both <see cref="EqualityComparerPropertyName"/> and <see cref="EqualityComparerTypeName"/> have to be null or not null.</remarks>
		/// <seealso cref="LengthCollectionRule.Comparer"/>
		public string EqualityComparerPropertyName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the name of the static property to get <see cref="IEqualityComparer{T}"/> from. 
		/// </summary>
		/// <remarks>Both <see cref="EqualityComparerPropertyName"/> and <see cref="EqualityComparerTypeName"/> have to be null or not null.</remarks>
		/// <seealso cref="LengthCollectionRule.Comparer"/>
		public string EqualityComparerTypeName
		{
			get;
			set;
		}

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
			var equalityComparer = (IEqualityComparer)TypeExtensions.GetStaticProperty(EqualityComparerTypeName, EqualityComparerPropertyName);

			return new LengthCollectionRule(Minimum, Maximum)
			       	{
			       		ExcludeDuplicatesFromCount = ExcludeDuplicatesFromCount,
						Comparer = equalityComparer
			       	};
		}

        #endregion
    }
}