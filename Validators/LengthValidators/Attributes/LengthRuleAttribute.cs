using System;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="LengthRule"/> should be applied to the program element.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
	public abstract class LengthRuleAttribute : RuleAttribute
    {
        #region Fields

        private int minimum;

        #endregion


        #region Constructors


        /// <param name="maximum">The maximum length.</param>
        protected LengthRuleAttribute(int maximum)
        {
            Maximum = maximum;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the maximum length.
        /// </summary>
        /// <seealso cref="LengthRule.Maximum"/>
        public int Maximum
        {
        	get;
        	private set;
        }


        /// <summary>
        /// Gets the minimum length.
        /// </summary>
        /// <exception cref="ArgumentException">Value is less than 0.</exception>
        /// <seealso cref="LengthRule.Minimum"/>
        public int Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Minimum too low.", "value");
                }
                minimum = value;
            }
        }

        #endregion

   }
}