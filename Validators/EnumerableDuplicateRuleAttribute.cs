using System;
using System.Collections;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="EnumerableDuplicateRule"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="EnumerableDuplicateRule"/>  
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\EnumerableDuplicateRuleAttributeExample.cs"  lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\EnumerableDuplicateRuleAttributeExample.vb"  lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class EnumerableDuplicateRuleAttribute : RuleAttribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the type name for the type to get <see cref="IEqualityComparer"/> from.
        /// </summary>
        /// <seealso cref="EnumerableDuplicateRule.Comparer"/>
        public string EqualityComparerTypeName
        {
        	get;
        	set;
        }

        /// <summary>
        /// Gets or sets the name of the static property to get <see cref="IEqualityComparer"/> from. 
        /// </summary>
        /// <seealso cref="EnumerableDuplicateRule.Comparer"/>
        public string EqualityComparerPropertyName
        {
        	get;
        	set;
        }

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            var equalityComparer = (IEqualityComparer) TypeExtensions.GetStaticProperty(EqualityComparerTypeName, EqualityComparerPropertyName);
            return new EnumerableDuplicateRule()
                   	{
						Comparer = equalityComparer
                   	};
        }

        #endregion
    }
}