using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidationFramework.ErrorMessage
{
	public class ParamMessageContext
	{
		public string ParameterName { get; set; }
		public string MethodName { get; set; }
		public string ClassName { get; set; }

		/// <summary>
		/// Initializes a new instance of the ParamMessageContext class.
		/// </summary>
		public ParamMessageContext(string parameterName, string methodName, string className)
		{
			ParameterName = parameterName;
			MethodName = methodName;
			ClassName = className;
		}
	}
}
