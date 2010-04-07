using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RequiredRule{T}"/>, that will check the existance of a <see cref="decimal"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="RequiredRule{T}"/>
    /// <remarks>Since it is not possible to pass a <see cref="decimal"/> to an <see cref="Attribute"/> a <see cref="double"/> is used as the <see cref="InitialValue"/>. It is converted to a <see cref="decimal"/> at runtime.</remarks>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RequiredValidators\RequiredDecimalRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RequiredValidators\RequiredDecimalRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class RequiredDecimalRuleAttribute : RuleAttribute
    {
        #region Fields

        private double? initialValue;

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the initial and invalid value.
        /// </summary>
        /// <seealso cref="RequiredRule{T}.InitialValue"/>
        public double InitialValue
        {
            get
            {
                return initialValue.Value;
            }
            set
            {
                initialValue = value;
            }
        }

        #endregion


        #region Methods


		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            var initialValueDecimal = (decimal?) initialValue;
            if (initialValue.HasValue)
            {
                return new RequiredRule<decimal>
                       	{
InitialValue = initialValueDecimal.Value
                       	};
            }
            else
            {
                return new RequiredRule<decimal>();
            }
        }

        #endregion
    }
}