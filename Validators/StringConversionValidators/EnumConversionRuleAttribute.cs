using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
	/// Specifies that a <see cref="EnumConversionRule"/> should be applied to the program element.
    /// </summary>
	/// <seealso cref="EnumConversionRule"/>
    /// <example>
	/// <code source="Examples\ExampleLibraryCSharp\Validators\EnumConversionRuleAttributeExample.cs" lang="cs"/>
	/// <code source="Examples\ExampleLibraryVB\Validators\EnumConversionRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class EnumConversionRuleAttribute : RuleAttribute
    {



        #region Constructors

        /// <param name="enumTypeName">A string representing the <see cref="Enum"/> <see cref="Type"/> to convert to.</param>
		public EnumConversionRuleAttribute(string enumTypeName)
        {
        	EnumType = Type.GetType(enumTypeName);
        }

        /// <param name="enumType">The <see cref="Enum"/> <see cref="Type"/> to convert to.</param>
		public EnumConversionRuleAttribute(Type enumType)
        {
            EnumType = enumType;
        }

    	#endregion


        #region Properties


	


		/// <summary>
		/// Gets or sets the enum <see cref="Type"/>.
		/// </summary>
		public Type EnumType
		{
			get; private set;
		}

		/// <summary>
		/// Gets or sets weather to ignore case. 
		/// </summary>
		/// <remarks>
		/// Only applicable if the property type is string.
		/// </remarks>
		public bool IgnoreCase
		{
			get;
			set;
		}

	

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
		{
			return new EnumConversionRule(EnumType)
			       	{
			       		IgnoreCase = IgnoreCase
			       	};
		}

    	#endregion
    }
}