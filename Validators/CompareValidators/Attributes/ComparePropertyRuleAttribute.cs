using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="ComparePropertyRule"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="ComparePropertyRule"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\CompareValidators\ComparePropertyRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\CompareValidators\ComparePropertyRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class ComparePropertyRuleAttribute : CompareRuleAttribute
    {

        #region Constructors

  
        /// <param name="operator">The comparison operation to perform.</param>
        /// <param name="propertyToCompare">The property to compare with.</param>
        public ComparePropertyRuleAttribute(string propertyToCompare, CompareOperator @operator)
            : base(@operator)
        {
            PropertyToCompare = propertyToCompare;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the value to compare.
        /// </summary>
        /// <seealso cref="ComparePropertyRule.PropertyToCompare"/>
        public string PropertyToCompare
        {
			get;
			private set;
        }

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            return new ComparePropertyRule(PropertyToCompare, Operator);
        }

        #endregion
    }
}