using System;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A <see cref="IRuleConfigReader"/> that creates a <see cref="CustomRule"/> from a <see cref="RuleData"/>.
    /// </summary>
    /// <remarks>
    ///   <see cref="RuleData"/> for default attributes.
    ///   <b>Extra Attributes</b>
    ///   <ul>
    ///     <li>
    ///       <c>ruleInterpretation</c> (required): Used to populate <see cref="CustomRule.RuleInterpretation"/>.  
    ///     </li>
    ///     <li>
    ///       <c>validationMethod</c> (required): Used to populate <see cref="CustomRule.Handler"/>.   
    ///     </li>
    ///     <li>
    ///       <c>validationTypeName</c> (required): Used to populate <see cref="CustomRule.Handler"/>.   
    ///     </li>
    ///   </ul>
    /// </remarks>
    /// <example>
    /// <code lang="xml" title="This example shows an xml configuration for CustomRuleConfigReader">
    /// <rule 
    /// ruleInterpretation="This is a custom rule" 
    /// validationMethod="ValMethod" 
    /// validationTypeName="MyNamespace.MyType,MyAssembly" 
    /// errorMessage="hello" 
    /// typeName="CustomRule"/>
    /// </code>
    /// </example>
    /// <example>
    /// The following example shows how to add a <see cref="CustomRule"/> using xml configuration. 
    /// <code source="Examples\ExampleLibraryCSharp\Configuration\CustomRuleConfigReaderExample.cs" lang="cs"/>
    /// </example>
    /// <seealso cref="CustomRule"/>
    /// <seealso cref="ConfigurationService"/>
	public sealed class CustomRuleConfigReader : BaseRuleConfigReader
    {
        #region Methods

		/// <inheritdoc />
        public override Rule CreateInstance(RuleData ruleData, Type runtimeType)
        {
            Guard.ArgumentNotNull(ruleData, "ruleData");

            var validationTypeName = ruleData.XmlAttributes["validationTypeName"];
            var validationMethod = ruleData.XmlAttributes["validationMethod"];
            var ruleInterpretation = ruleData.XmlAttributes["ruleInterpretation"];

                Type validationType;
            try
            {
                validationType = Type.GetType(validationTypeName, true);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(string.Format("CustomRule could not load the validation type '{0}'.", validationTypeName), exception);
            }
            return new CustomRule(validationType, validationMethod, ruleInterpretation);
        }

        #endregion
    }
}