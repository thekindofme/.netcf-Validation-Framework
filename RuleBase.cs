using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidationFramework
{
	/// <summary>
	/// Base class for all validator feature configuration info.
	/// </summary>
	public abstract class RuleBase
	{
		public string ErrorMessage { get; set; }
		public bool UseErrorProvider { get; set; }
        public abstract bool IsEquivalent(RuleBase rule);
	}
}
