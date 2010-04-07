using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidationFramework.Reflection;
using ValidationFramework.ErrorMessage;

namespace ValidationFramework
{
	public interface IStateValidator
	{
		string GetDefaultErrorMessage(PropertyMessageContext context);
		bool Validate(object value, object parent);
        bool IsEquivalent(IStateValidator rule);
	}
}
