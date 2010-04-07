using System;
using System.Xml;
using System.Xml.Serialization;
using ValidationFramework.Extensions;

namespace ValidationFramework
{
    internal static class RequiredObjectRuleCreator
    {
        #region Methods

		public static Rule ReadConfig(string errorMessage, bool useErrorMessageProvider, XmlReader initialValueXmlReader, RuntimeTypeHandle runtimeTypeHandle)
		{
			var genericRequiredRuleType = typeof (RequiredRule<>);
			var targetMemberType = Type.GetTypeFromHandle(runtimeTypeHandle);
			var constructedRequiredRuleType = genericRequiredRuleType.MakeGenericType(targetMemberType);

			var rule = (Rule) Activator.CreateInstance(constructedRequiredRuleType);
			if (initialValueXmlReader != null)
			{
				var objectType = Type.GetTypeFromHandle(runtimeTypeHandle);
				var xmlSerializer = new XmlSerializer(objectType);

				var initialValue = xmlSerializer.Deserialize(initialValueXmlReader);
				rule.SetProperty("InitialValue", initialValue);
			}
			return rule;
		}

    	#endregion
    }
}