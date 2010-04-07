using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="CompareRule{T}"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="CompareRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\CompareValidators\CompareStringRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\CompareValidators\CompareStringRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class CompareStringRuleAttribute : CompareRuleAttribute
    {

        #region Constructors

        /// <param name="operator">The comparison operation to perform.</param>
        /// <param name="valueToCompare">The value to compare with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="valueToCompare"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="valueToCompare"/> is <see cref="string.Empty"/>.</exception>
        public CompareStringRuleAttribute(string valueToCompare, CompareOperator @operator)
            : base(@operator)
        {
            Guard.ArgumentNotNullOrEmptyString(valueToCompare, "valueToCompare");
            ValueToCompare = valueToCompare;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the value to compare.
        /// </summary>
        /// <seealso cref="CompareRule{T}.ValueToCompare"/>
        public string ValueToCompare
        {
			get;
			private set;
        }

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            return new CompareRule<string>(ValueToCompare, Operator);
        }

        #endregion
    }
}