using System;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="RequiredRule{T}"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>initialValue</c> (optional): Used to populate <see cref="ValidationFramework.RequiredRule{T}.InitialValue"/>. Accepted formats for <see cref="DateTime"/> are "dd MMM yyyy HH:mm:ss.ff", "yyyy-MM-ddTHH:mm:ss", "dd MMM yyyy hh:mm tt", "dd MMM yyyy hh:mm:ss tt", "dd MMM yyyy HH:mm:ss", "dd MMM yyyy HH:mm" and "dd MMM yyyy" 
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for RequiredRuleConfigReader">
    /// <rule errorMessage="hello" 
    /// initialValue="10" 
    /// typeName="RequiredRule"/>
    /// </code>
    /// </example>
    /// <seealso cref="RequiredRule{T}"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class RequiredRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, Type genericType)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");
            var genericRequiredRuleType = typeof (RequiredRule<>);
            //var genericType = Type.GetTypeFromHandle(runtimeType);
			if (genericType.IsGenericType && (genericType.GetGenericTypeDefinition().Equals(TypePointers.NullableType)))
			{
				genericType = genericType.GetGenericArguments()[0];
			}
            Type[] typeArgs = {genericType};
            var constructedRequiredRuleType = genericRequiredRuleType.MakeGenericType(typeArgs);

            object initialValue = null;

            if (ruleData.XmlAttributes != null)
            {
                string initialValueString;
                if (ruleData.XmlAttributes.TryGetValue("initialValue", out initialValueString))
                {
                    initialValue = TypeConverterEx.ChangeType(initialValueString, genericType);
                }
            }
			var rule = (Rule)Activator.CreateInstance(constructedRequiredRuleType);
               rule.SetProperty("InitialValue", initialValue );
            	return rule;
        }

        #endregion
    }
}