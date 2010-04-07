using System;
using System.Collections;
using ValidationFramework.Extensions;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="LengthCollectionRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>excludeDuplicatesFromCount</c> (optional): Used to populate <see cref="ValidationFramework.LengthCollectionRule.ExcludeDuplicatesFromCount"/>.  
    ///     </li>
    ///     <li>
    ///       <c>minimum</c> (optional): Used to populate <see cref="ValidationFramework.LengthRule.Maximum"/>.   
    ///     </li>
    ///     <li>
    ///       <c>maximum</c> (required): Used to populate <see cref="ValidationFramework.LengthRule.Minimum"/>.   
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for LengthCollectionRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// excludeDuplicatesFromCount="true" 
    /// typeName="LengthCollectionRule" 
    /// minimum="1" 
    /// maximum="5"/>
    /// </code>
    /// </example>
    /// <seealso cref="LengthCollectionRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class LengthCollectionRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

        /// <summary>
        /// Create a <see cref="Rule"/> from a <see cref="RuleData"/>.
        /// </summary>
        /// <param name="ruleData">The <see cref="RuleData"/> that represent the xml to create the <see cref="Rule"/> for.</param>
        /// <param name="runtimeType">The <see cref="System.Type"/> for the <see cref="Type"/> to create the <see cref="Rule"/> for.</param>
        /// <returns>A <see cref="Rule"/> that <paramref name="ruleData"/> represented</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ruleData"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Both equalityComparerTypeName and equalityComparerPropertyName have to be null or not null.</exception>
        public override Rule CreateInstance(RuleData ruleData, Type runtimeType)
        {
        	Guard.ArgumentNotNull(ruleData, "ruleData");

            var minimum = RuleData.TryGetValue(ruleData.XmlAttributes, "minimum", 0);
            var maximum = int.Parse(ruleData.XmlAttributes["maximum"]);
            var excludeDuplicatesFromCount = RuleData.TryGetValue(ruleData.XmlAttributes, "excludeDuplicatesFromCount", true);

        	string equalityComparerTypeName;
            ruleData.XmlAttributes.TryGetValue("equalityComparerTypeName", out equalityComparerTypeName);
        	string equalityComparerPropertyName;
            ruleData.XmlAttributes.TryGetValue("equalityComparerPropertyName", out equalityComparerPropertyName);
        	var equalityComparer = (IEqualityComparer) TypeExtensions.GetStaticProperty(equalityComparerTypeName, equalityComparerPropertyName);

			return new LengthCollectionRule(minimum, maximum)
			{
				ExcludeDuplicatesFromCount = excludeDuplicatesFromCount,
				Comparer = equalityComparer
			};
        }

    	#endregion
    }
}