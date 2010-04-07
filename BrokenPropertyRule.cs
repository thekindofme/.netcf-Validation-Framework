using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ValidationFramework
{
	public class BrokenPropertyRuleInfo
	{
		public string ErrorMessage { get; set; }
		public object AttemptedValue { get; set; }
		public PropertyInfo PropertyInfo { get; set; }
	}
}
