using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidationFramework
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public abstract class ParameterRuleAttributeBase : Attribute
    {
        /// <summary>
        /// Gets or sets A <see cref="string"/> used to group <see cref="ParameterRule"/>s. Use a null to indicate no grouping.
        /// </summary>
        public string RuleSet
        {
            get;
            set;
        }

        /// <seealso cref="Rule.UseErrorMessageProvider"/>
        public bool UseErrorMessageProvider
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Subclasses will implement this to return thier custom validators 
        /// </summary>
        /// <returns></returns>
        public abstract IValueValidator CreateValidator();
    }
}
