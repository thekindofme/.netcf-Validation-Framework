using System;
using System.Globalization;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    internal static class RequiredEnumRuleCreator
    {
        #region Methods


        public static Rule ReadConfig(string errorMessage, bool useErrorMessageProvider, Type runtimeType)
        {
            CheckTypeIsEnum(runtimeType);
            bool targetMemberIsNullable;
            Type constructedRequiredRuleType;

            var targetMemberType = GetTargetMemberType(runtimeType, out targetMemberIsNullable, out constructedRequiredRuleType);

            if (targetMemberIsNullable)
            {
                var constructorInfo = constructedRequiredRuleType.GetPublicInstanceConstructor();
                return (Rule)constructorInfo.Invoke(null);
            }
            else
            {
#if (WindowsCE || SILVERLIGHT)
              var initialValueObject = EnumExtensions.GetValues(targetMemberType).GetValue(0);
#else
                var initialValueObject = Enum.GetValues(targetMemberType).GetValue(0);
#endif
				var constructorInfo = constructedRequiredRuleType.GetPublicInstanceConstructor();
				var rule = (Rule)Activator.CreateInstance(constructedRequiredRuleType);
				rule.SetProperty("InitialValue", initialValueObject);
				return rule;
            }
        }

        private static void CheckTypeIsEnum(Type runtimeType)
        {
            if (runtimeType.Equals(TypePointers.EnumType))
            {
                throw new ArgumentException("Incorrect type to be validated. Should be an enum not an instance of Enum", "runtimeType");
            }
        }


        public static Rule ReadConfig(string initialValue, string errorMessage, bool useErrorMessageProvider, Type runtimeType)
        {
            CheckTypeIsEnum(runtimeType);
            bool targetMemberIsNullable;
            Type constructedRequiredRuleType;

            var targetMemberType = GetTargetMemberType(runtimeType, out targetMemberIsNullable, out constructedRequiredRuleType);

            var enumInitialValue = Enum.Parse(targetMemberType, initialValue, true);

            var constructorInfo = constructedRequiredRuleType.GetPublicInstanceConstructor();
        	var rule = (Rule)constructorInfo.Invoke(new object[] {  });
			rule.SetProperty("InitialValue", enumInitialValue);
        	return rule;
        }

        public static Rule ReadConfig(long initialValue, string errorMessage, bool useErrorMessageProvider, Type runtimeType)
        {
            CheckTypeIsEnum(runtimeType);
            bool targetMemberIsNullable;
            Type constructedRequiredRuleType;
            var targetMemberType = GetTargetMemberType(runtimeType, out targetMemberIsNullable, out constructedRequiredRuleType);

            var enumInitialValue = GenEnumFromValue(initialValue, targetMemberType);
			var constructorInfo = constructedRequiredRuleType.GetPublicInstanceConstructor();
			var rule = (Rule)constructorInfo.Invoke(new object[] { });
			rule.SetProperty("InitialValue", enumInitialValue);
			return rule;
        }

        private static Type GetTargetMemberType(Type runtimeType, out bool targetMemberIsNullable, out Type constructedRequiredRuleType)
        {
            var genericRequiredRuleType = typeof (RequiredRule<>);
            //var targetMemberType = Type.GetTypeFromHandle(runtimeType);
            var underlyingType = Nullable.GetUnderlyingType(runtimeType);
            targetMemberIsNullable = underlyingType != null;
            if (targetMemberIsNullable)
            {
                runtimeType = underlyingType;
            }
            constructedRequiredRuleType = genericRequiredRuleType.MakeGenericType(runtimeType);
            return runtimeType;
        }

        internal static object GenEnumFromValue(long value, Type enumType)
        {
#if (WindowsCE || SILVERLIGHT)
           var values = EnumExtensions.GetValues(enumType);
#else
            var values = Enum.GetValues(enumType);
#endif
            foreach (var enumValue in values)
            {
                var changeType = (long)Convert.ChangeType(enumValue, typeof(long), CultureInfo.InvariantCulture);
                if (changeType == value)
                {
                    return enumValue;
                }
            }
            throw new Exception(string.Format("Value '{0}' not defined in enum '{1}'.", value, enumType.ToUserFriendlyString()));
        }
        
        
        #endregion
    }
}