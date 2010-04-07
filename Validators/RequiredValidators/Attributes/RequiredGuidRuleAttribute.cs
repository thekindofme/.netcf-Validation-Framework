using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RequiredRule{T}"/>, that will check the existance of a <see cref="Guid"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="RequiredRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RequiredValidators\RequiredGuidRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RequiredValidators\RequiredGuidRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class RequiredGuidRuleAttribute : RuleAttribute
    {
   

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

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            Guid? initialValueGuid;
            if (string.IsNullOrEmpty(InitialValue))
            {
                initialValueGuid = null;
            }
            else
            {
                initialValueGuid = new Guid(InitialValue);
            }
            if (initialValueGuid.HasValue)
            {
                return new RequiredRule<Guid>
                       	{
							InitialValue = initialValueGuid.Value
                       	};
            }
            else
            {
                return new RequiredRule<Guid>();
            }
        }

        #endregion
    }
}