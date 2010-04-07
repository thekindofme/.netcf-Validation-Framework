using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="ValidatableRule"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="ValidatableRule"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\ValidatableRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\ValidatableRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class ValidatableRuleAttribute : RuleAttribute
    {


        #region Properties


        /// <summary>
        /// Gets or sets a value indicating if <see cref="IValidatable.ErrorMessages"/> should be use as the <see cref="Rule.ErrorMessage"/>.  
        /// </summary>
        public bool UseMemberErrorMessages
        {
        	get;
        	set;
        }

        #endregion

        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
		{
			var instance = new ValidatableRule();
			if (UseMemberErrorMessages)
			{
				if (ErrorMessage == null)
				{
					instance.UseMemberErrorMessages = true;
				}
				else
				{
					throw new InvalidOperationException("Cannot use 'useMemberErrorMessages'");
				}
			}
			return instance;
		}

    	#endregion
    }
}