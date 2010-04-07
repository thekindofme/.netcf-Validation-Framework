using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidationFramework.ErrorMessage
{
	public class PropertyMessageContext
	{
		public string PropertyName { get; set; }
		public string TypeName { get; set; }

		/// <summary>
		/// Initializes a new instance of the PropertyMessageContext class.
		/// </summary>
		public PropertyMessageContext(string propertyName, string className)
		{
			PropertyName = propertyName;
			TypeName = className;
		}
	}
}
