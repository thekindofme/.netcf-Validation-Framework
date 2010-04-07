using System;
using System.Globalization;
using System.Text;
using ValidationFramework.Reflection;

namespace ValidationFramework
{

    /// <summary>
    /// Performs a string to number conversion validation.
    /// </summary>
    /// <remarks>If the value being validated is null the rule will evaluate to true.</remarks>
    /// <seealso cref="NumberConversionRuleConfigReader"/>
	/// <seealso cref="NumberConversionRuleAttribute"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class NumberConversionRule : ValueRule
    {

		private NumberFormatInfo numberFormatInfo;

    	#region Constructors




		/// <param name="typeCode">The <see cref="TypeCode"/> that this <see cref="NumberConversionRule"/> attempts to convert to.</param>
		public NumberConversionRule(TypeCode typeCode)
			: base(TypePointers.StringTypeHandle)
		{
			if ((typeCode == TypeCode.String) || (typeCode == TypeCode.DateTime))
			{
				throw new ArgumentException("datatype cannot be String or DateTime.", "typeCode");
			}
			TypeCode = typeCode;
			numberFormatInfo = NumberFormatInfo.CurrentInfo;
			switch (typeCode)
			{
				case TypeCode.Decimal:
					NumberStyles = NumberStyles.Number;
					break;
				case TypeCode.Double:
				case TypeCode.Single:
					NumberStyles = NumberStyles.Float | NumberStyles.AllowThousands;
					break;
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
					NumberStyles = NumberStyles.Integer;
					break;
			}
		}

    	#endregion


        #region Properties


		/// <inheritdoc />
        public override string RuleInterpretation
        {
            get
            {
                return string.Format("The value must be convertible to '{0}'.", TypeCode);
            }
        }


    	/// <summary>
    	/// Gets a <see cref="bool"/> to indicate if whitespace should be removed from the value being validated.
    	/// </summary>
    	public bool RemoveWhiteSpace { get; set; }


    	/// <summary>
		/// Gets the <see cref="TypeCode"/> that this <see cref="NumberConversionRule"/> attempts to convert to.
		/// </summary>
		public TypeCode TypeCode
		{
			get;
			private set;
		}


    	/// <summary>
		/// Gets the <see cref="System.Globalization.NumberFormatInfo"/> that this <see cref="NumberConversionRule"/> uses to convert with.
		/// </summary>
		public NumberFormatInfo NumberFormatInfo
    	{
    		get { return numberFormatInfo; }
    		set
    		{
				Guard.ArgumentNotNull(value, "value");
    			numberFormatInfo = value;
    		}

    	}

    	/// <summary>
		/// Gets the <see cref="System.Globalization.NumberStyles"/> that this <see cref="NumberConversionRule"/> uses to convert with.
		/// </summary>
		public NumberStyles NumberStyles
		{
			get;
			set;
		}

    	#endregion


        #region Methods


		/// <inheritdoc />
        public override bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            if (targetMemberValue == null)
            {
                return true;
            }
            else
            {	
                var memberValueAsString = (string) targetMemberValue;
				if (RemoveWhiteSpace)
				{
					var stringBuilder = new StringBuilder(memberValueAsString.Length);
					foreach (var c in memberValueAsString)
					{
						if (!char.IsWhiteSpace(c))
						{
							stringBuilder.Append(c);
						}
					}
					memberValueAsString = stringBuilder.ToString();
				}
            	if (memberValueAsString.Length == 0)
                {
                    return true;
                }
                else
                {
                	return GetIsValid(memberValueAsString);
                }
            }
        }
#if (WindowsCE)
        private bool GetIsValid(string memberValueAsString)
        {
            var isValid = false;
            try
            {
                switch (TypeCode)
                {
                    case TypeCode.Byte:
                        {
                            byte.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    case TypeCode.SByte:
                        {
                            sbyte.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    case TypeCode.Decimal:
                        {
                            decimal.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    case TypeCode.Double:
                        {
                            double.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    case TypeCode.Single:
                        {
                            float.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            short.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            ushort.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            int.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            uint.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            long.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            ulong.Parse(memberValueAsString, NumberStyles, NumberFormatInfo);
                            break;
                        }
                    default:
                        {
                            throw new InvalidOperationException("Invalid TypeCode on NumberConversionRule.");
                        }
                }
                isValid = true;
            }
            catch (FormatException)
            {
            }

            return isValid;
        }
#else
        private bool GetIsValid(string memberValueAsString)
        {
            bool isValid;
            switch (TypeCode)
            {
                case TypeCode.Byte:
                    {
                        byte result;
                        isValid = byte.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                case TypeCode.SByte:
                    {
                        sbyte result;
                        isValid = sbyte.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                case TypeCode.Decimal:
                    {
                        decimal result;
                        isValid = decimal.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                case TypeCode.Double:
                    {
                        double result;
                        isValid = double.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                case TypeCode.Single:
                    {
                        float result;
                        isValid = float.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                case TypeCode.Int16:
                    {
                        short result;
                        isValid = short.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        ushort result;
                        isValid = ushort.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                case TypeCode.Int32:
                    {
                        int result;
                        isValid = int.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        uint result;
                        isValid = uint.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                case TypeCode.Int64:
                    {
                        long result;
                        isValid = long.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        ulong result;
                        isValid = ulong.TryParse(memberValueAsString, NumberStyles, NumberFormatInfo, out result);
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("Invalid TypeCode on NumberConversionRule.");
                    }
            }
            return isValid;
        }
#endif

        /// <inheritdoc />
        protected override string GetComputedErrorMessage(string tokenizedMemberName, string descriptorType)
        {
			return string.Format("The {0} '{1}' must be convertible to a number.", descriptorType, tokenizedMemberName);
        }


		/// <inheritdoc />
        public override bool IsEquivalent(Rule rule)
        {
			var numberConversionRule = (NumberConversionRule)rule;
			return numberConversionRule.TypeCode == TypeCode &&
				numberConversionRule.NumberStyles == NumberStyles &&
				numberConversionRule.NumberFormatInfo == NumberFormatInfo;
        }


        #endregion
    }
}