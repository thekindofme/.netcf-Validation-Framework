using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ValidationFramework
{
	/// <summary>
	/// Configuration info for validators applied to properties.
	/// </summary>
	public class PropertyValueRule : PropertyRule
	{
		public IValueValidator Validator { get; set; }
        public override bool IsEquivalent(RuleBase rule)
        {
            //var propertyStateRule = (PropertyStateRule)rule;
            //if (this.ErrorMessage.Equals(rule.ErrorMessage))

            throw new NotImplementedException();
        }
	}
}
