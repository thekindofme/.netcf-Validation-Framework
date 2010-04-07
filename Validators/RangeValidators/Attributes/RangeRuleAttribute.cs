using System;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies that a <see cref="RangeRule{T}"/>, that will check the range of a <see cref="byte"/>, should be applied to the program element.
    /// </summary>
    /// <seealso cref="RangeRule{T}"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Validators\RangeValidators\RangeByteRuleAttributeExample.cs" lang="cs"/>
    /// <code source="Examples\ExampleLibraryVB\Validators\RangeValidators\RangeByteRuleAttributeExample.vb" lang="vbnet"/>
    /// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public abstract class RangeRuleAttribute : RuleAttribute
	{

		#region Constructors

		/// <summary>
		/// Instantiate a new instance of the <see cref="RangeRuleAttribute"/> class.
		/// </summary>
    	protected RangeRuleAttribute()
		{
			EqualsMinimumIsValid = true;
			EqualsMaximumIsValid = true;
		}

		#endregion

		#region Properties

		/// <summary>
        /// Get or sets a value indicating if the minimum value is valid.
        /// </summary>
        public bool EqualsMinimumIsValid
        {
			get;
			set;
        }


        /// <summary>
        /// Get or sets a value indicating if the maximum value is valid.
        /// </summary>
        public bool EqualsMaximumIsValid
        {
        	get;
        	set;
        }

        #endregion


    }
}