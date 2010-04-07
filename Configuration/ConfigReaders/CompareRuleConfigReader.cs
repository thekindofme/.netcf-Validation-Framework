using System;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="CompareRule{T}"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>valueToCompare</c> (required): Used to populate <see cref="ValidationFramework.CompareRule{T}.ValueToCompare"/>.  Accepted formats for <see cref="DateTime"/> are "dd MMM yyyy HH:mm:ss.ff", "yyyy-MM-ddTHH:mm:ss", "dd MMM yyyy hh:mm tt", "dd MMM yyyy hh:mm:ss tt", "dd MMM yyyy HH:mm:ss", "dd MMM yyyy HH:mm" and "dd MMM yyyy" 
    ///     </li>
    ///     <li>
    ///       <c>compareOperator</c> (required): Used to populate <see cref="CompareRule{T}.CompareOperator"/>.   
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for CompareRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// typeName="CompareRule" 
    /// valueToCompare="1" 
    /// compareOperator="Equal"/>
    /// </code>
    /// </example>
    /// <seealso cref="CompareRule{T}"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class CompareRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, Type genericType)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");

            var genericCompareStructRuleType = typeof (CompareRule<>);
            //var genericType = Type.GetTypeFromHandle(runtimeTypeHandle);
			if (genericType.IsGenericType &&  (genericType.GetGenericTypeDefinition().Equals(TypePointers.NullableType)))
			{
				genericType = genericType.GetGenericArguments()[0];
			}
            Type[] typeArgs = {genericType};
            var constructedCompareStructRuleType = genericCompareStructRuleType.MakeGenericType(typeArgs);


            var valueToCompare = TypeConverterEx.ChangeType(ruleData.XmlAttributes["valueToCompare"], genericType);
            var compareOperator = (CompareOperator)Enum.Parse(typeof(CompareOperator), ruleData.XmlAttributes["compareOperator"], true);
            var constructorInfo = constructedCompareStructRuleType.GetPublicInstanceConstructor(genericType, typeof(CompareOperator));
		    return (Rule) constructorInfo.Invoke(new[]{valueToCompare, compareOperator});
        }

        #endregion
    }
}