using System;
using ValidationFramework.Reflection;
using System.Linq.Expressions;

namespace ValidationFramework.Fluent
{
	public class PropertyInfo<T> where T : class
	{
	    internal PropertyDescriptor propertyDescriptor;

		/// <summary>
		/// Initializes a new instance of the PropertyInfo class.
		/// </summary>
		public PropertyInfo(Expression<Func<T, object>> expression)
		{
			propertyDescriptor = TypeCache.GetOrCreatePropertyDescriptor(expression);
		}

		public PropertyInfo<T> Passes(Rule rule)
		{
            //TODO: ruleset
			propertyDescriptor.Rules.Add(rule);
			return this;
		}
	}
}
