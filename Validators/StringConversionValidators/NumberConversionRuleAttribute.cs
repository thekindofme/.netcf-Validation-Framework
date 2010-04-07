using System;
using System.Globalization;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
	/// Specifies that a <see cref="NumberConversionRule"/> should be applied to the program element.
    /// </summary>
	/// <seealso cref="NumberConversionRule"/>
    /// <example>
	/// <code source="Examples\ExampleLibraryCSharp\Validators\NumberConversionRuleAttributeExample.cs" lang="cs"/>
	/// <code source="Examples\ExampleLibraryVB\Validators\NumberConversionRuleAttributeExample.vb" lang="vbnet"/>
	/// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class NumberConversionRuleAttribute : RuleAttribute
    {

        #region Constructors

/// <inheritdoc/>
		public NumberConversionRuleAttribute(TypeCode typeCode)
        {
        	TypeCode = typeCode;
        }
    	#endregion


        #region Properties


		/// <summary>
		/// Gets the <see cref="TypeCode"/> that this <see cref="NumberConversionRule"/> attempts to convert to.
		/// </summary>
		/// <seealso cref="NumberConversionRule.TypeCode"/>
		public TypeCode TypeCode
		{
			get;
			private set;
		}


		/// <summary>
		/// Gets or sets a <see cref="bool"/> to indicate if whitespace should be removed from the value being validated.
		/// </summary>
		/// <seealso cref="NumberConversionRule.RemoveWhiteSpace"/>
		public bool RemoveWhiteSpace
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the type name for the type to get <see cref="System.Globalization.NumberFormatInfo"/> from.
		/// </summary>
		/// <remarks>Both <see cref="NumberFormatInfoTypeName"/> and <see cref="NumberFormatInfoPropertyName"/> have to be null or not null.</remarks>
		/// <seealso cref="NumberConversionRule.NumberFormatInfo"/>
		public string NumberFormatInfoTypeName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the name of the static property to get <see cref="System.Globalization.NumberFormatInfo"/> from. 
		/// </summary>
		/// <remarks>Both <see cref="NumberFormatInfoTypeName"/> and <see cref="NumberFormatInfoPropertyName"/> have to be null or not null.</remarks>
		/// <seealso cref="NumberConversionRule.NumberFormatInfo"/>
		public string NumberFormatInfoPropertyName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the <see cref="System.Globalization.NumberStyles"/> that this <see cref="NumberConversionRule"/> uses to convert with.
		/// </summary>
		/// <seealso cref="NumberConversionRule.NumberStyles"/>
		public NumberStyles? NumberStyles
		{
			get;
			set;
		}

        #endregion


        #region Methods

		/// <inheritdoc/>
		public override Rule CreateInstance(InfoDescriptor infoDescriptor)
		{
			var numberFormatInfo = (NumberFormatInfo) TypeExtensions.GetStaticProperty(NumberFormatInfoTypeName, NumberFormatInfoPropertyName);
			//TODO: populate other properties
			return new NumberConversionRule(TypeCode)
			       	{
			       		NumberFormatInfo = numberFormatInfo
			       	};
		}

    	#endregion

    }
}