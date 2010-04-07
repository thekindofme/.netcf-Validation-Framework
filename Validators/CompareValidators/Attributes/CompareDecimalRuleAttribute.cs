using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="CompareRule{T}"/>, that will compare a <see cref="decimal"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="CompareRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\CompareValidators\CompareDecimalRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\CompareValidators\CompareDecimalRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
	#if (!SILVERLIGHT)
	[Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class CompareDecimalRuleAttribute : CompareRuleAttribute
    {

        #region Constructors


        /// <param name="operator">The comparison operation to perform.</param>
        /// <param name="valueToCompare">The value to compare with.</param>
        public CompareDecimalRuleAttribute(double valueToCompare, CompareOperator @operator)
            : base(@operator)
        {
            ValueToCompare = (decimal) valueToCompare;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the value to compare.
        /// </summary>
        /// <seealso cref="CompareRule{T}.ValueToCompare"/>
        public decimal ValueToCompare
        {
			get;
			private set;
        }

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            return new CompareRule<decimal>(ValueToCompare, Operator);
        }

        #endregion
    }
}