using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidationFramework.ValueValidators
{
    /// <summary>
    /// Specifies that a <see cref="RequiredValidator<bool>"/> should be applied to the tagged property.
    /// </summary>
    /// <seealso cref="RequiredBoolRule"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RequiredValidators\RequiredBoolRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RequiredValidators\RequiredBoolRuleAttributeExample.vb" lang="vbnet"/>
    /// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class RequiredBoolAttribute : PropertyValueRuleAttributeBase
    {
        public override IValueValidator CreateValidator()
        {
            return new RequiredValidator<bool>();
        }
    }
}
