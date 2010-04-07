using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ValidationFramework
{
    public class RuleFactory
    {
        public static PropertyStateRule CreatePropertyStateRule(PropertyInfo info, PropertyStateRuleAttributeBase attrib)
        {
            return new PropertyStateRule()
            {
                ErrorMessage = attrib.ErrorMessage,
                PropertyInfo = info,
                UseErrorProvider = attrib.UseErrorMessageProvider,
                Validator = attrib.CreateValidator()
            };
        }
        public static PropertyValueRule CreatePropertyValueRule(PropertyInfo info, PropertyValueRuleAttributeBase attrib)
        {
            return new PropertyValueRule()
            {
                ErrorMessage = attrib.ErrorMessage,
                PropertyInfo = info,
                UseErrorProvider = attrib.UseErrorMessageProvider,
                Validator = attrib.CreateValidator()
            };
        }
        public static ParameterRule CreateParameterRule(ParameterRuleAttributeBase attrib)
        {
            return new ParameterRule()
            {
                ErrorMessage = attrib.ErrorMessage,
                UseErrorProvider = attrib.UseErrorMessageProvider,
                Validator = attrib.CreateValidator()
            };
        }
    }
}
