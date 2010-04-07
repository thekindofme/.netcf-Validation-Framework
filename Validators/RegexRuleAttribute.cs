using System;
using System.Text.RegularExpressions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RegexRule"/> should be applied to the program element.
    /// </summary>
    /// <seealso cref="RegexRule"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RegexRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RegexRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class RegexRuleAttribute : RuleAttribute
    {
        #region Fields

        private readonly string validationExpression;

        #endregion


        #region Constructors

  
        /// <param name="validationExpression">The regular expression pattern to match.</param>
        public RegexRuleAttribute(string validationExpression)
        {
            this.validationExpression = validationExpression;
        	RegexOptions = RegexOptions.None;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the regular expression pattern to match.  
        /// </summary>
        /// <seealso cref="RegexRule.ValidationExpression"/>
        public string ValidationExpression
        {
            get
            {
                return validationExpression;
            }
        }

        /// <summary>
        /// A bitwise OR combination of <see cref="System.Text.RegularExpressions.RegexOptions"/> enumeration values.
        /// </summary>
        /// <seealso cref="RegexRule.RegexOptions"/>
        public RegexOptions RegexOptions
        {
        	get;
        	set;
        }

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
        {
            return new RegexRule(validationExpression)
                   	{
						RegexOptions = RegexOptions
                   	};
        }

        #endregion
    }
}