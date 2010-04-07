using System;

namespace ValidationFramework
{
	public partial class MemberValidationManager
    {


		/// <summary>
		/// Should be called if no rules can be found.
		/// </summary>
		/// <param name="ruleSet">The rule set being validated.</param>
		/// <param name="ruleFound"><see langword="true"/> if <see cref="Rule"/>s where found; otherwise <see langword="false"/>.</param>
		/// <param name="throwException"><see langword="true"/> to throw an exception if the type is not found; <see langword="false"/> to ignore.</param>
		/// <exception cref="InvalidOperationException"><paramref name="ruleFound"/> is <see langword="false"/> and <paramref name="throwException"/> is <see langword="true"/>.</exception>
        public static void ThrowNoRules(string ruleSet, bool ruleFound, bool throwException)
        {
            if (!ruleFound && throwException)
            {
                if (ruleSet == null)
                {
                    throw new InvalidOperationException("No members could be found containing rules.");
                }
                else
                {
                    throw new InvalidOperationException(string.Format("No members could be found containing rules with the ruleSet '{0}'.", ruleSet));
                }
            }
        }

   

    }
}

