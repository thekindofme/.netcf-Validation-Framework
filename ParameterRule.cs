using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidationFramework
{
	/// <summary>
	/// Configuration info for validators applied to parameters.
	/// </summary>
	public class ParameterRule : RuleBase
	{
        public IValueValidator Validator {get; set; }

        public override bool IsEquivalent(RuleBase rule)
        {
            //var propertyStateRule = (PropertyStateRule)rule;
            //if (this.ErrorMessage.Equals(rule.ErrorMessage))

            throw new NotImplementedException();
        }
	}
}
