using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidationFramework.Reflection;
using ValidationFramework.ErrorMessage;

namespace ValidationFramework.ValueValidators
{
	public class RequiredValidator<T> : IValueValidator
	{
		public T InitialValue { get; set; }
		
		// Constructors
		public RequiredValidator()
		{
			this.InitialValue = default(T);
		}
		public RequiredValidator(T initialValue)
		{
			this.InitialValue = initialValue;
		}

		#region IValueValidator Members

		public string GetDefaultParamErrorMessage(ParamMessageContext context)
		{
			return string.Format("The parameter {0} of method {1} on class {2} is required", context.ParameterName, context.MethodName, context.ClassName);
		}

		public string GetDefaultPropertyErrorMessage(PropertyMessageContext context)
		{
			return string.Format("The property '{1}' is required.", context.PropertyName);
		}

		public bool Validate(object value)
		{
			if (value == null)
				return false;

			var hasInitialValue = !this.InitialValue.Equals(default(T));
			if (hasInitialValue)
			{
				var castedValue = (T)value;
				if (castedValue.Equals(this.InitialValue))
					return false;
			}

			return true;
		}

        public bool IsEquivalent(IValueValidator validator)
        {
            var requiredValidator = (RequiredValidator<T>)validator;
            return InitialValue.Equals(requiredValidator.InitialValue);
        }

		#endregion
	}
}
