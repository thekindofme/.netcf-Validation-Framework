using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="CompareRule{T}"/>, that will compare a <see cref="DateTime"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="CompareRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\CompareValidators\CompareByteRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\CompareValidators\CompareByteRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class CompareDateTimeRuleAttribute : CompareRuleAttribute
    {


        #region Constructors

      
        /// <param name="operator">The comparison operation to perform.</param>
        /// <param name="valueToCompare">The value to compare with.</param>
        public CompareDateTimeRuleAttribute(string valueToCompare, CompareOperator @operator)
            : base(@operator)
        {
            ValueToCompare = valueToCompare;
            ValueToCompareDateTime = DateTimeConverter.Parse(valueToCompare);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the value to compare.
        /// </summary>
        /// <remarks>
        /// Accepted formats are "dd MMM yyyy HH:mm:ss.ff", "yyyy-MM-ddTHH:mm:ss", "dd MMM yyyy hh:mm tt", "dd MMM yyyy hh:mm:ss tt", "dd MMM yyyy HH:mm:ss", "dd MMM yyyy HH:mm" and "dd MMM yyyy"
        /// </remarks>
        /// <seealso cref="CompareRule{T}.ValueToCompare"/>
		public string ValueToCompare
		{
			get; 
			private set;
		}


        /// <summary>
        /// Gets a <see cref="DateTime"/> representation of <see cref="ValueToCompare"/>.
        /// </summary>
		public DateTime ValueToCompareDateTime
		{
			get; 
			private set;
		}

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            return new CompareRule<DateTime>(ValueToCompareDateTime, Operator);
        }

        #endregion
    }
}