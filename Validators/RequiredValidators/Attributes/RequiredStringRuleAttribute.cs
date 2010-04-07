using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RequiredStringRule"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="RequiredStringRule"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RequiredValidators\RequiredStringRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RequiredValidators\RequiredStringRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class RequiredStringRuleAttribute : RuleAttribute
    {    
		#region Constructor
        /// <inheritdoc />
		public RequiredStringRuleAttribute()
		{
			
        IgnoreCase = true;
        TrimWhiteSpace = true;
		}
#endregion

        #region Properties

        /// <summary>
        /// Gets or sets the initial and invalid value.
        /// </summary>
        /// <seealso cref="RequiredRule{T}.InitialValue"/>
        public string InitialValue
        {
        	get;
        	set;
        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> to indicate if whitespace should be trimmed from the value being validated.
        /// </summary>
        /// <seealso cref="RequiredStringRule.TrimWhiteSpace"/>
        public bool TrimWhiteSpace
        {
        	get;
        	set;
        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> to indicate if case should be ignored.
        /// </summary>
        /// <seealso cref="RequiredStringRule.IgnoreCase"/>
        public bool IgnoreCase
        {
        	get;
        	set;
        }

        #endregion


        #region Methods


		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            if (InitialValue == null)
            {
                return new RequiredStringRule
                       	{
                       		TrimWhiteSpace = TrimWhiteSpace
                       	};
            }
            else
            {
                return new RequiredStringRule
                       	{
                       		InitialValue = InitialValue,
							TrimWhiteSpace = TrimWhiteSpace,
							IgnoreCase = IgnoreCase
                       	};
            }
        }

        #endregion
    }
}