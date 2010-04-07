using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RequiredRule{T}"/>, that will check the existance of a <see cref="short"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="RequiredRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RequiredValidators\RequiredShortRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RequiredValidators\RequiredShortRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class RequiredShortRuleAttribute : RuleAttribute
    {
        #region Fields

        private short? initialValue;

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the initial and invalid value.
        /// </summary>
        /// <seealso cref="RequiredRule{T}.InitialValue"/>
        public short InitialValue
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
                return new RequiredRule<short>
                       	{
							InitialValue = initialValue.Value
                       	};
            }
            else
            {
                return new RequiredRule<short>();
            }
        }

        #endregion
    }
}