using System;
using ValidationFramework.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ValidationFramework.Fluent
{
	public static class Validation
	{
		public static void ClearRulesFor<T>() where T : class
		{
			TypeCache.RemoveType<T>();
		}

		public static PropertyInfo<T> Ensure<T>(Expression<Func<T, object>> expression) where T : class
		{
			return new PropertyInfo<T>(expression);
		}
		//TODO: I don't like how configuration and validation calls are mixed
		public static IList<ValidationError> Validate(object objectToValidate)
		{
			return PropertyValidationManager.ValidateAll(objectToValidate, null, null);
		}
		public static IList<ValidationError> Validate(object objectToValidate, object context)
		{
			return PropertyValidationManager.ValidateAll(objectToValidate, null, context);
		}
		public static IList<ValidationError> Validate(object objectToValidate, string ruleSet)
		{
			return PropertyValidationManager.ValidateAll(objectToValidate, ruleSet, null);
		}
		public static IList<ValidationError> Validate(object objectToValidate, string ruleSet, object context)
		{
			return PropertyValidationManager.ValidateAll(objectToValidate, null, context);
		}
	}
}
