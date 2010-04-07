using System;
using System.Collections;
using ValidationFramework.Extensions;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="EnumerableDuplicateRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="RuleData"/> for default attributes.
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for EnumerableDuplicateRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// typeName="EnumerableDuplicateRule"
    /// equalityComparerTypeName="System.StringComparer"
    /// equalityComparerPropertyName="InvariantCulture"
    /// />
    /// </code>
    /// </example>
    /// <seealso cref="EnumerableDuplicateRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class EnumerableDuplicateRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, Type runtimeType)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");
            if (ruleData.XmlAttributes == null)
            {
                return new EnumerableDuplicateRule();
            }
            else
            {

				string equalityComparerTypeName;
                ruleData.XmlAttributes.TryGetValue("equalityComparerTypeName", out equalityComparerTypeName);
            	string equalityComparerPropertyName;
                ruleData.XmlAttributes.TryGetValue("equalityComparerPropertyName", out equalityComparerPropertyName);
            	var equalityComparer = (IEqualityComparer)TypeExtensions.GetStaticProperty(equalityComparerTypeName, equalityComparerPropertyName);
                return new EnumerableDuplicateRule
                       	{
							Comparer = equalityComparer
                       	};
            }
        }

        #endregion
    }
}