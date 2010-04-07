using System;
using System.Collections.Generic;
using System.Text;
#if(!WindowsCE)
using System.Linq.Expressions;
#endif
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;
using ValidationFramework.Configuration;
using ValidationFramework.ErrorMessage;

namespace ValidationFramework
{
	public class ValidationManger
	{
		// --- Fields ---
		private List<PropertyValueRule> propertyValueRules;
		private List<PropertyStateRule> propertyStateRules;
		private List<BrokenPropertyRuleInfo> brokenRules;
		private IValidationResultBuilder resultBuilder;

		// --- Properties ---
		/// <summary>
		/// Gets a value indicating if all members are valid.
		/// </summary>
		/// <remarks>Calling this property does not perform a validation it only checks the current state. To perform a full validation call 
		/// <see cref="MemberValidationManager{T}.ValidateAll()"/>.</remarks>
		public bool IsValid
		{
			get
			{
				return (brokenRules.Count == 0);
			}
		}

		/// <summary>
		/// Gets the instance of the object that this <see cref="MemberValidationManager{T}"/> is handling.
		/// </summary>
		/// <remarks>Will return a null for static types.</remarks>
		public Object ManagedObject
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the <see cref="RuntimeTypeHandle"/> for the <see cref="Type"/> being validated.
		/// </summary>
		public RuntimeTypeHandle TargetHandle
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the rule set to this is managing.
		/// </summary>
		/// <remarks>
		/// Only <see cref="Rule"/>s where RuleSet equals this value.
		/// Case insensitive so this will always return a upper case string no matter what is passed into the constructor.
		/// </remarks>
		public string RuleSet { get; set; }

		// --- Constructors ---

		/// <remarks>Use this constructor if an instance of an object is being validated.</remarks>
		/// <param name="instanceToManage">An instance of the object to be validated.</param>
		/// <exception cref="ArgumentNullException"><paramref name="instanceToManage"/> is null.</exception>
		protected ValidationManger(object instanceToManage)
		{
			Guard.ArgumentNotNull(instanceToManage, "targetObject");
			ManagedObject = instanceToManage;
#if(WindowsCE || SILVERLIGHT)
			TargetHandle = instanceToManage.GetType().TypeHandle;
#else
			TargetHandle = Type.GetTypeHandle(instanceToManage);
#endif
			resultBuilder = new DefaultValidationResultBuilder();
		}

		// --- Methods ---

		// --- Methods | Validation ---

		public void Validate()
		{
			brokenRules = new List<BrokenPropertyRuleInfo>();

			//this.Merge(GetBrokenRuleInfos(this.propertyValueRules), GetBrokenRuleInfos(this.propertyStateRules));

			foreach (var stateRule in propertyStateRules)
			{
				var validator = stateRule.Validator;
				var currentPropertyValue = stateRule.PropertyInfo.GetValue(this.ManagedObject, null);

				if (!validator.Validate(currentPropertyValue, this.ManagedObject))
				{
					var info = new BrokenPropertyRuleInfo()
					{
						AttemptedValue = currentPropertyValue,
						ErrorMessage = GetPropertyStateErrorMessage(stateRule, currentPropertyValue, this.ManagedObject),
						PropertyInfo = stateRule.PropertyInfo
					};
					brokenRules.Add(info);
				}
			}


		}

		private List<BrokenPropertyRuleInfo> Merge(List<BrokenPropertyRuleInfo> sourceList, List<BrokenPropertyRuleInfo> list)
		{
			foreach (var item in list)
			{
				sourceList.Add(item);
			}
			return sourceList;
		}

		private List<BrokenPropertyRuleInfo> GetBrokenRuleInfos(List<PropertyValueRule> propertyValueRules)
		{
			var brokenRules = new List<BrokenPropertyRuleInfo>();

			foreach (var valueRule in propertyValueRules)
			{
				var validator = valueRule.Validator;
				var currentPropertyValue = valueRule.PropertyInfo.GetValue(this.ManagedObject, null);

				if (!validator.Validate(currentPropertyValue))
				{
					var info = new BrokenPropertyRuleInfo()
					{
						AttemptedValue = currentPropertyValue,
						ErrorMessage = GetPropertyValueErrorMessage(valueRule, currentPropertyValue),
						PropertyInfo = valueRule.PropertyInfo
					};
					brokenRules.Add(info);
				}
			}
			return brokenRules;
		}

		private string GetPropertyStateErrorMessage(PropertyStateRule rule, object attemptedValue, object parent)
		{
			// Use message provider by consumer
			if (!string.IsNullOrEmpty(rule.ErrorMessage))
				return rule.ErrorMessage;

			// TODO: Figure out how to handle error message provider

			// Use message provided by the specified ErrorMessageProvider
			//if (rule.UseErrorProvider)
			//{
			//    if (ConfigurationService.ErrorMessageProvider == null)
			//    {
			//        throw new InvalidOperationException("useErrorMessageProvider is true but no ErrorMessageProvider is specified in the ConfigurationService.");
			//    }
			//    return ConfigurationService.ErrorMessageProvider.RetrieveErrorMessage(rule.StateValidator, parent, attemptedValue, null);
			//}

			// Use the validator's default message
			var propertyName = rule.PropertyInfo.Name;
			var className = rule.PropertyInfo.DeclaringType.Name;
			return rule.Validator.GetDefaultErrorMessage(new PropertyMessageContext(propertyName, className));
		}

		private string GetPropertyValueErrorMessage(PropertyValueRule rule, object attemptedValue)
		{
			// Use message provider by consumer
			if (!string.IsNullOrEmpty(rule.ErrorMessage))
				return rule.ErrorMessage;

			// Use the validator's default message
			var propertyName = rule.PropertyInfo.Name;
			var className = rule.PropertyInfo.DeclaringType.Name;
			return rule.Validator.GetDefaultPropertyErrorMessage(new PropertyMessageContext(propertyName, className));
		}
	}
}
