using System;
using System.IO;
using System.Xml;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RequiredRule{T}"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="RequiredRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RequiredValidators\RequiredObjectRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RequiredValidators\RequiredObjectRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class RequiredObjectRuleAttribute : RuleAttribute
    {
        #region Fields

        private string initialValue;

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the initial and invalid value.
        /// </summary>
        /// <seealso cref="RequiredRule{T}.InitialValue"/>
        public string InitialValue
        {
            get
            {
                return initialValue;
            }
            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                initialValue = value;
            }
        }

        #endregion


        #region Methods


		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            if (initialValue == null)
            {
                return RequiredObjectRuleCreator.ReadConfig(ErrorMessage, UseErrorMessageProvider, null, infoDescriptor.RuntimeType);
            }
            else
            {
                return RequiredObjectRuleCreator.ReadConfig(ErrorMessage, UseErrorMessageProvider, XmlReader.Create(new StringReader(initialValue)), infoDescriptor.RuntimeType);
            }
        }

        #endregion
    }
}