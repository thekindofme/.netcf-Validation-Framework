using System;
using System.Reflection;

namespace ValidationFramework.Reflection
{
	/// <summary>
	/// Specifies an <see cref="Assembly"/> should be included when using reflection to look for <see cref="RuleAttribute"/>s.
	/// </summary>
	/// <remarks>
	/// This does not effect adding <see cref="Rule"/>s programmatically or adding <see cref="Rule"/>s with configuration.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public sealed class IncludeAssemblyAttribute : Attribute
	{
	}
}