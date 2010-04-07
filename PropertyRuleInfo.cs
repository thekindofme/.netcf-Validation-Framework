using System;

namespace ValidationFramework
{
	public class PropertyRuleInfo
	{
		//TODO: Make this readonly
		public Type Type { get; set; }
		public string PropertyName { get; set; }
		public Rule Rule { get; set; }
	}
}
