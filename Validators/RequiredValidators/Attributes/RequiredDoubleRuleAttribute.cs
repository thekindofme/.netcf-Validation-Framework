using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RequiredRule{T}"/>, that will check the existance of a <see cref="double"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="RequiredRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RequiredValidators\RequiredDoubleRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RequiredValidators\RequiredDoubleRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class RequiredDoubleRuleAttribute : RuleAttribute
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
            if (initialValue.HasValue)
            {
                return new RequiredRule<double>
                       	{
							InitialValue = initialValue.Value
                       	};
            }
            else
            {
                return new RequiredRule<double>();
            }
        }

        #endregion
    }
}