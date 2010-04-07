using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Performs a required field validation on a <see langword="bool"/>.
    /// </summary>
    /// <seealso cref="RequiredBoolRuleConfigReader"/>
	/// <seealso cref="RequiredBoolRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class RequiredBoolRule : RequiredRule<bool>
    {
        #region Methods
		/// <inheritdoc />
        public override bool Validate( object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
		    return (targetMemberValue != null);
        }

        #endregion
    }
}