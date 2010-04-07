using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidationFramework
{
	public class DefaultValidationResultBuilder : IValidationResultBuilder
	{


		// --- IValidationResultBuilder Members ---
		public ValidationResult GetValidationResult(PropertyStateRule rule, object attemptedValue, object parent)
		{
			throw new NotImplementedException();
		}
		public ValidationResult GetValidationResult(PropertyValueRule rule, object attemptedValue)
		{
			throw new NotImplementedException();
		}
	}
}
