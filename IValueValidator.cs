using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidationFramework.Reflection;
using ValidationFramework.ErrorMessage;

namespace ValidationFramework
{
	public interface IValueValidator
	{
		string GetDefaultParamErrorMessage(ParamMessageContext context);
		string GetDefaultPropertyErrorMessage(PropertyMessageContext context);
		bool Validate(object value);
        bool IsEquivalent(IValueValidator rule);
	}
}
