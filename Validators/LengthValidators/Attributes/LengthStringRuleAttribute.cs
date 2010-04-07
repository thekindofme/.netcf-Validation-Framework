using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="LengthStringRule"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="LengthStringRule"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\LengthValidators\LengthStringRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\LengthValidators\LengthStringRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class LengthStringRuleAttribute : LengthRuleAttribute
    {


        #region Constructor

        /// <param name="maximum">The maximum length allowed.</param>
        public LengthStringRuleAttribute(int maximum)
            : base(maximum)
        {
        }

        #endregion


        #region Methods

  
		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            return new LengthStringRule(Minimum, Maximum)
                   	{
                   		TrimWhiteSpace = TrimWhiteSpace
                   	};
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets a <see cref="bool"/> to indicate if whitespace should be trimmed from the value being validated.
        /// </summary>
        /// <seealso cref="LengthStringRule.TrimWhiteSpace"/>
        public bool TrimWhiteSpace
        {
        	get;
        	set;
        }

        #endregion
    }
}