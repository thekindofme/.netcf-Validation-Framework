using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="CompareRule{T}"/>, that will compare a <see cref="short"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="CompareRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\CompareValidators\CompareShortRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\CompareValidators\CompareShortRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class CompareShortRuleAttribute : CompareRuleAttribute
    {
        
        #region Constructors

        /// <param name="operator">The comparison operation to perform.</param>
        /// <param name="valueToCompare">The value to compare with.</param>
        public CompareShortRuleAttribute(short valueToCompare, CompareOperator @operator)
            : base(@operator)
        {
            ValueToCompare = valueToCompare;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the value to compare.
        /// </summary>
        /// <seealso cref="CompareRule{T}.ValueToCompare"/>
        public short ValueToCompare
        {
        	get;
        	private set;
        }

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            return new CompareRule<short>(ValueToCompare, Operator);
        }

        #endregion
    }
}