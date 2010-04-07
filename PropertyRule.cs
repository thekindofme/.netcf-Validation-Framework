using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ValidationFramework
{
	public abstract class PropertyRule : RuleBase
	{
		public PropertyInfo PropertyInfo { get; set; }
	}
}
