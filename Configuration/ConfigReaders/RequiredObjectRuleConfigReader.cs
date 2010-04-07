using System;
using System.IO;
using System.Xml;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="RequiredRule{T}"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Elements</b>
    ///   <ul>
    ///     <li>
    ///       Any element (optional): Only 1 allowed. Used to populate <see cref="RequiredRule{T}.InitialValue"/>. It is converted to an object using a <see cref="System.Xml.Serialization.XmlSerializer"/>.   
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for RequiredObjectRuleConfigReader">
    /// <rule 
    /// errorMessage="hello" 
    /// typeName="RequiredObjectRule">
    /// <person name="aaa"/>
    /// </rule>
    /// </code>
    /// </example>
    /// <seealso cref="RequiredRule{T}"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class RequiredObjectRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, RuntimeTypeHandle runtimeTypeHandle)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");

            var errorMessage = ruleData.ErrorMessage;
            XmlReader initialValueXmlReader = null;
            if (!string.IsNullOrEmpty(ruleData.InnerXml))
            {
                initialValueXmlReader = XmlReader.Create(new StringReader(ruleData.InnerXml),null,(XmlParserContext)null);
            }
            return RequiredObjectRuleCreator.ReadConfig(errorMessage, ruleData.UseErrorMessageProvider, initialValueXmlReader, runtimeTypeHandle);
        }

        #endregion
    }
}