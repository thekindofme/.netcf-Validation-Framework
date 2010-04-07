using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RequiredRule{T}"/>, that validates and enum, should be applied to the program element.
    /// </summary>
    /// <seealso cref="RequiredRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RequiredValidators\RequiredEnumRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RequiredValidators\RequiredEnumRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class RequiredEnumRuleAttribute : RuleAttribute
    {
        #region Constructors


        /// <param name="initialValue">The initial invalid value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="initialValue"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="initialValue"/> is <see cref="string.Empty"/>.</exception>
        public RequiredEnumRuleAttribute(string initialValue)
        {
            Guard.ArgumentNotNullOrEmptyString(initialValue, "initialValue");
            InitialValueAsString = initialValue;
        }

        /// <param name="initialValue">The initial invalid value.</param>
        public RequiredEnumRuleAttribute(long initialValue)
        {
            InitialValueAsLong = initialValue;
        }
        /// <inheritdoc />
        public RequiredEnumRuleAttribute()
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the initial and invalid value.
        /// </summary>
        /// <seealso cref="RequiredRule{T}.InitialValue"/>
        public string InitialValueAsString
        {
        	get;
        	private set;
        }

        /// <summary>
        /// Gets or sets the initial and invalid value.
        /// </summary>
        /// <seealso cref="RequiredRule{T}.InitialValue"/>
        public long? InitialValueAsLong
        {
        	get;
            private set;
        }
      
        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
		{
		    Guard.ArgumentNotNull(infoDescriptor, "infoDescriptor");
		    if (InitialValueAsString == null)
		    {
		        if (InitialValueAsLong == null)
		        {
		            return RequiredEnumRuleCreator.ReadConfig(ErrorMessage, UseErrorMessageProvider, infoDescriptor.RuntimeType);
		        }
		        else
		        {
		            return RequiredEnumRuleCreator.ReadConfig(InitialValueAsLong.Value, ErrorMessage, UseErrorMessageProvider, infoDescriptor.RuntimeType);
		        }
		    }
		    else
		    {
		        return RequiredEnumRuleCreator.ReadConfig(InitialValueAsString, ErrorMessage, UseErrorMessageProvider, infoDescriptor.RuntimeType);
		    }
		}

        #endregion

    	
    }
}