using System;
using ValidationFramework.Configuration;

namespace ValidationFramework.Reflection
{

	/// <summary>
	/// Regularly used <see cref="Type"/>s.
	/// </summary>
	public static class TypePointers
	{

        /// <summary>
        /// The <see cref="Type"/> of <see cref="IRuleConfigReader"/>.
        /// </summary>
        internal static readonly Type IRuleConfigReaderType = typeof(IRuleConfigReader);
        /// <summary>
		/// The <see cref="Type"/> of <see cref="DateTime"/>.
		/// </summary>
		public static readonly Type DateTimeType = typeof(DateTime);
		/// <summary>
		/// The <see cref="Type"/> of <see cref="Nullable{T}"/>.
		/// </summary>
		public static readonly Type NullableType = typeof(Nullable<>);
		/// <summary>
		/// The <see cref="Type"/> of <see cref="String"/>.
		/// </summary>
		public static readonly Type StringType = typeof(string);
		/// <summary>
		/// The <see cref="Type"/> of <see cref="int"/>.
		/// </summary>
		public static readonly Type IntType = typeof(int);
		/// <summary>
        /// The <see cref="Type"/> of <see cref="float"/>.
		/// </summary>
		public static readonly Type FloatType = typeof(float);
		/// <summary>
        /// The <see cref="Type"/> of <see cref="float"/>.
		/// </summary>
		public static readonly Type UIntType = typeof(uint);
		/// <summary>
		/// The <see cref="Type"/> of <see cref="decimal"/>.
		/// </summary>
		public static readonly Type DecimalType = typeof(decimal);
		/// <summary>
		/// The <see cref="Type"/> of <see cref="long"/>.
		/// </summary>
		public static readonly Type LongType = typeof(long);
		/// <summary>
		/// The <see cref="Type"/> of <see cref="ulong"/>.
		/// </summary>
		public static readonly Type ULongType = typeof(ulong);
		/// <summary>
		/// The <see cref="Type"/> of <see cref="short"/>.
		/// </summary>
		public static readonly Type ShortType = typeof(short);
		/// <summary>
		/// The <see cref="Type"/> of <see cref="ushort"/>.
		/// </summary>
		public static readonly Type UShortType = typeof(ushort);
		/// <summary>	
		/// The <see cref="Type"/> of <see cref="double"/>.
		/// </summary>
		public static readonly Type DoubleType = typeof(double);
		/// <summary>
		/// The <see cref="Type"/> of <see cref="byte"/>.
		/// </summary>
		public static readonly Type ByteType = typeof(byte);
		/// <summary>
		/// The <see cref="Type"/> of <see cref="sbyte"/>.
		/// </summary>
		public static readonly Type SByteType = typeof(sbyte);

		/// <summary>
		/// The <see cref="Type"/> of <see cref="Enum"/>.
		/// </summary>
		public static readonly Type EnumType = typeof(Enum);


        /// <summary>
        /// Check if a <see cref="Type"/> represents a numeric <see cref="Type"/>.
        /// </summary>
        /// <param name="runtimeType">The <see cref="Type"/> to check.</param>
        /// <returns>A <see cref="bool"/> indicating if <paramref name="runtimeType"/> represents a numeric <see cref="Type"/>.</returns>
        public static bool IsNumericType(Type runtimeType)
        {
            return
                runtimeType.Equals(ByteType) ||
                runtimeType.Equals(IntType) ||
                runtimeType.Equals(DecimalType) ||
                runtimeType.Equals(LongType) ||
                runtimeType.Equals(ULongType) ||
                runtimeType.Equals(DoubleType) ||
                runtimeType.Equals(ShortType) ||
                runtimeType.Equals(FloatType) ||
                runtimeType.Equals(UIntType) ||
                runtimeType.Equals(UShortType);
        }

        /// <summary>
        /// Check if a <see cref="Type"/> represents a whole number <see cref="Type"/>.
        /// </summary>
        /// <remarks>
        /// Checks to see if it can contain decimal points.
        /// </remarks>
        /// <param name="runtimeType">The <see cref="Type"/> to check.</param>
        /// <returns>A <see cref="bool"/> indicating if <paramref name="runtimeType"/> represents a whole number <see cref="Type"/>.</returns>
        public static bool IsWholeNumberType(Type runtimeType)
        {
            return
                runtimeType.Equals(ByteType) ||
                runtimeType.Equals(IntType) ||
                runtimeType.Equals(UIntType) ||
                runtimeType.Equals(LongType) ||
                runtimeType.Equals(ULongType) ||
                runtimeType.Equals(ShortType) ||
                runtimeType.Equals(UShortType);
        }
	}
}
