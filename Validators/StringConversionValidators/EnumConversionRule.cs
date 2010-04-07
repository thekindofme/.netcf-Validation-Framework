using System;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{

    /// <summary>
    /// Performs a string to number conversion validation.
    /// </summary>
    /// <remarks>If the value being validated is null the rule will evaluate to true.</remarks>
    /// <seealso cref="EnumConversionRuleConfigReader"/>
	/// <seealso cref="EnumConversionRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class EnumConversionRule : ValueRule
    {
        #region Fields
		private const string invalidTypeFormat = "Member '{0}' must be a byte, short, int, long, ushort, ulong, uint or string to be used for the {1}. Actual Type '{2}'.";

    	#endregion


        #region Constructors

  

		/// <param name="enumType">The <see cref="Type"/> that will attempted to be converted to.</param>
		public EnumConversionRule(Type enumType)
            : base(TypePointers.StringTypeHandle)
        {
			Guard.ArgumentNotNull(enumType,"enumType");
        	EnumType = enumType;
        }

    	#endregion


        #region Properties


		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                return "The value must be convertible to Enum.";
            }
        }

		/// <summary>
		/// Gets the <see cref="Type"/> that will attempted to be converted to.
		/// </summary>
		public Type EnumType
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets whether to ignore case. 
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



		/// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            bool isValid;
            if (targetMemberValue == null)
            {
                return true;
            }
            else
            {
                return TypeExtensions.IsEnumDefined(EnumType, infoDescriptor.RuntimeTypeHandle, targetMemberValue, IgnoreCase);
            }

        }


		/// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
            return string.Format("The {0} '{1}' is an invalid format.", descriptorType, tokenizedMemberName);
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
			var enumConversionRule = (EnumConversionRule)rule;
        	return enumConversionRule.EnumType == EnumType;
        }
		/// <inheritdoc />
		internal override void CheckType(InfoDescriptor infoDescriptor)
		{
		    var targetMemberRuntimeTypeHandle = infoDescriptor.RuntimeTypeHandle;
			if (!TypePointers.IsNumericType(targetMemberRuntimeTypeHandle) &&
				!targetMemberRuntimeTypeHandle.Equals(TypePointers.StringTypeHandle))
			{
				var friendlyRuleTypeName = GetType().ToUserFriendlyString();
				var targetMemberRuntimeType = Type.GetTypeFromHandle(targetMemberRuntimeTypeHandle);
				var friendlyTargetMemberTypeName = targetMemberRuntimeType.ToUserFriendlyString();
                var exceptionMessage = string.Format(invalidTypeFormat, infoDescriptor.Name, friendlyRuleTypeName, friendlyTargetMemberTypeName);
				throw new ArgumentException(exceptionMessage, "value");
			}
		}


        #endregion
    }
}