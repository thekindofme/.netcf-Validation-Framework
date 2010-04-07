using System;

namespace ValidationFramework.Reflection
{
	/// <summary>
	/// Specifies a field or property should be cached even if it has no <see cref="Rule"/>s applied to it.
	/// </summary>
	/// <remarks>
	/// This can be helpful if you want to define rules using code, i.e. not with attributes or configuration),
	/// because you don't have to add the <see cref="PropertyDescriptor"/>s or <see cref="FieldDescriptor"/>s programmatically.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public sealed class IncludeMemberAttribute : Attribute
	{
	}
}