using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
	public interface IValidationResultBuilder
	{
		ValidationResult GetValidationResult(PropertyStateRule rule, object attemptedValue, object parent);
		ValidationResult GetValidationResult(PropertyValueRule rule, object attemptedValue);
	}
}
