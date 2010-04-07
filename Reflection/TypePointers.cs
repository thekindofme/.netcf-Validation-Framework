using System;
using ValidationFramework.Configuration;

namespace ValidationFramework.Reflection
{

	/// <summary>
	/// Regularly used <see cref="RuntimeTypeHandle"/>s.
	/// </summary>
	public static class TypePointers
	{

        /// <summary>
        /// The <see cref="Type"/> of <see cref="IRuleConfigReader"/>.
        /// </summary>
        internal static readonly Type IRuleConfigReaderType = typeof(IRuleConfigReader);
        /// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="DateTime"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle DateTimeTypeHandle = typeof(DateTime).TypeHandle;
		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="Nullable{T}"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle NullableTypeHandle = typeof(Nullable<>).TypeHandle;
		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="String"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle StringTypeHandle = typeof(string).TypeHandle;
		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="int"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle IntTypeHandle = typeof(int).TypeHandle;
		/// <summary>
        /// The <see cref="RuntimeTypeHandle"/> of <see cref="float"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle FloatTypeHandle = typeof(float).TypeHandle;
		/// <summary>
        /// The <see cref="RuntimeTypeHandle"/> of <see cref="float"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle UIntTypeHandle = typeof(uint).TypeHandle;
		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="decimal"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle DecimalTypeHandle = typeof(decimal).TypeHandle;
		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="long"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle LongTypeHandle = typeof(long).TypeHandle;
		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="ulong"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle ULongTypeHandle = typeof(ulong).TypeHandle;
		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="short"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle ShortTypeHandle = typeof(short).TypeHandle;
		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="ushort"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle UShortTypeHandle = typeof(ushort).TypeHandle;
		/// <summary>	
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="double"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle DoubleTypeHandle = typeof(double).TypeHandle;
		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="byte"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle ByteTypeHandle = typeof(byte).TypeHandle;
		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="sbyte"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle SByteTypeHandle = typeof(sbyte).TypeHandle;

		/// <summary>
		/// The <see cref="RuntimeTypeHandle"/> of <see cref="Enum"/>.
		/// </summary>
		public static readonly RuntimeTypeHandle EnumTypeHandle = typeof(Enum).TypeHandle;


        /// <summary>
        /// Check if a <see cref="RuntimeTypeHandle"/> represents a numeric <see cref="Type"/>.
        /// </summary>
        /// <param name="runtimeTypeHandle">The <see cref="RuntimeTypeHandle"/> to check.</param>
        /// <returns>A <see cref="bool"/> indicating if <paramref name="runtimeTypeHandle"/> represents a numeric <see cref="Type"/>.</returns>
        public static bool IsNumericType(RuntimeTypeHandle runtimeTypeHandle)
        {
            return
                runtimeTypeHandle.Equals(ByteTypeHandle) ||
                runtimeTypeHandle.Equals(IntTypeHandle) ||
                runtimeTypeHandle.Equals(DecimalTypeHandle) ||
                runtimeTypeHandle.Equals(LongTypeHandle) ||
                runtimeTypeHandle.Equals(ULongTypeHandle) ||
                runtimeTypeHandle.Equals(DoubleTypeHandle) ||
                runtimeTypeHandle.Equals(ShortTypeHandle) ||
                runtimeTypeHandle.Equals(FloatTypeHandle) ||
                runtimeTypeHandle.Equals(UIntTypeHandle) ||
                runtimeTypeHandle.Equals(UShortTypeHandle);
        }

        /// <summary>
        /// Check if a <see cref="RuntimeTypeHandle"/> represents a whole number <see cref="Type"/>.
        /// </summary>
        /// <remarks>
        /// Checks to see if it can contain decimal points.
        /// </remarks>
        /// <param name="runtimeTypeHandle">The <see cref="RuntimeTypeHandle"/> to check.</param>
        /// <returns>A <see cref="bool"/> indicating if <paramref name="runtimeTypeHandle"/> represents a whole number <see cref="Type"/>.</returns>
        public static bool IsWholeNumberType(RuntimeTypeHandle runtimeTypeHandle)
        {
            return
                runtimeTypeHandle.Equals(ByteTypeHandle) ||
                runtimeTypeHandle.Equals(IntTypeHandle) ||
                runtimeTypeHandle.Equals(UIntTypeHandle) ||
                runtimeTypeHandle.Equals(LongTypeHandle) ||
                runtimeTypeHandle.Equals(ULongTypeHandle) ||
                runtimeTypeHandle.Equals(ShortTypeHandle) ||
                runtimeTypeHandle.Equals(UShortTypeHandle);
        }
	}
}
