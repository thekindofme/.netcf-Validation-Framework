using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidationFramework
{
	/// <summary>
	/// Configuration info for validators applied to classes.
	/// </summary>
	public class PropertyStateRule : PropertyRule
	{
		public IStateValidator Validator { get; set; }

        public override bool IsEquivalent(RuleBase rule)
        {
            //var propertyStateRule = (PropertyStateRule)rule;
            //if (this.ErrorMessage.Equals(rule.ErrorMessage))

           throw new NotImplementedException();
        }

	}
}
