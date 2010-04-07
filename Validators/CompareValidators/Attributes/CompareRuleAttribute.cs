#if (!SILVERLIGHT)
using System;
#endif


namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a comparison operation should be applied to the program element.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
	public abstract class CompareRuleAttribute : RuleAttribute
    {

        #region Constructors


        /// <param name="operator">The comparison operation to perform.</param>
		protected CompareRuleAttribute(CompareOperator @operator)
        {
			Operator = @operator;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the comparison operation to perform. 
        /// </summary>
        /// <seealso cref="CompareRule{T}.CompareOperator"/>
        public CompareOperator Operator
        {
			get;
			private set;
        }

        #endregion


    }
}