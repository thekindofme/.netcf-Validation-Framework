using System;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="RangeRule{T}"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>minimum</c> (required): Used to populate <see cref="ValidationFramework.RangeRule{T}.Maximum"/>.   Accepted formats for <see cref="DateTime"/> are "dd MMM yyyy HH:mm:ss.ff", "yyyy-MM-ddTHH:mm:ss", "dd MMM yyyy hh:mm tt", "dd MMM yyyy hh:mm:ss tt", "dd MMM yyyy HH:mm:ss", "dd MMM yyyy HH:mm" and "dd MMM yyyy" 
    ///     </li>
    ///     <li>
    ///       <c>maximum</c> (required): Used to populate <see cref="ValidationFramework.RangeRule{T}.Minimum"/>.   Accepted formats for <see cref="DateTime"/> are "dd MMM yyyy HH:mm:ss.ff", "yyyy-MM-ddTHH:mm:ss", "dd MMM yyyy hh:mm tt", "dd MMM yyyy hh:mm:ss tt", "dd MMM yyyy HH:mm:ss", "dd MMM yyyy HH:mm" and "dd MMM yyyy" 
    ///     </li>
    ///     <li>
    ///       <c>equalsMinimumIsValid</c> (optional): Used to populate <see cref="RangeRule{T}.EqualsMinimumIsValid"/>.   
    ///     </li>
    ///     <li>
    ///       <c>equalsMaximumIsValid</c> (optional): Used to populate <see cref="RangeRule{T}.EqualsMaximumIsValid"/>.   
    ///     </li>
    ///    </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for RangeRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// typeName="RangeRule" 
    /// minimum="1" 
    /// maximum="5"
    /// equalsMinimumIsValid="true" 
    /// equalsMaximumIsValid="true"/>
    /// </code>
    /// </example>
    /// <seealso cref="RangeRule{T}"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class RangeRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, Type genericType)
        {
        	Guard.ArgumentNotNull(ruleData, "ruleData");
        	var genericRangeRuleType = typeof (RangeRule<>);
            //var genericType = Type.GetTypeFromHandle(runtimeType);
        	if (genericType.IsGenericType && (genericType.GetGenericTypeDefinition().Equals(TypePointers.NullableType)))
        	{
        		genericType = genericType.GetGenericArguments()[0];
        	}
        	Type[] typeArgs = {genericType};
        	var constructedRangeRuleType = genericRangeRuleType.MakeGenericType(typeArgs);


            var minimum = TypeConverterEx.ChangeType(ruleData.XmlAttributes["minimum"], genericType);
            var maximum = TypeConverterEx.ChangeType(ruleData.XmlAttributes["maximum"], genericType);


            var equalsMinimumIsValid = RuleData.TryGetValue(ruleData.XmlAttributes, "equalsMinimumIsValid", true);
            var equalsMaximumIsValid = RuleData.TryGetValue(ruleData.XmlAttributes, "equalsMaximumIsValid", true);



            var constructorInfo = constructedRangeRuleType.GetPublicInstanceConstructor(genericType, genericType);
			var rule = (Rule)constructorInfo.Invoke(new[] {minimum, maximum});
			rule.SetProperty("EqualsMinimumIsValid", equalsMinimumIsValid);
			rule.SetProperty("EqualsMaximumIsValid", equalsMaximumIsValid);
			return rule;
        }

    	#endregion
    }
}