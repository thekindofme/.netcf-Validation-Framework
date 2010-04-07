using System;
using System.Collections.Generic;
#if(!WindowsCE)
using System.Linq.Expressions;
#endif
namespace ValidationFramework.Reflection
{
    /// <summary>
    /// Provides an in memory cache of <see cref="TypeDescriptor"/>s.
    /// </summary>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Reflection\AddWithTypeCacheExample.cs" title="The following example shows how to programmatically add validation Rules to a property." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\Reflection\AddWithTypeCacheExample.vb" title="The following example shows how to programmatically add validation Rules to a property." lang="vbnet" region="Example"/>
    /// <code source="Examples\ExampleLibraryCSharp\Reflection\TypeCacheExample.cs" title="The following example shows how to programmatically add validation Rules to a property." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\Reflection\TypeCacheExample.vb" title="The following example shows how to programmatically add validation Rules to a property." lang="vbnet" region="Example"/>
    /// <code source="Examples\ExampleLibraryCSharp\Reflection\AddCustomRuleWithTypeCacheExample.cs" lang="cs" title="This example shows how to programmatically add a CustomRule to a property." region="Example"/>
    /// <code source="Examples\ExampleLibraryCSharp\Reflection\AddCustomRuleWithTypeCacheStrongTypedExample.cs" lang="cs" title="This example shows how to programmatically add a CustomRule to a property." region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\Reflection\AddCustomRuleWithTypeCacheExample.vb" lang="vbnet" title="This example shows how to programmatically add a CustomRule to a property." region="Example"/>
    /// </example>
    public static class TypeCache
    {
        #region Fields

        private static readonly Dictionary<RuntimeTypeHandle, TypeDescriptor> typeDescriptorDictionary = new Dictionary<RuntimeTypeHandle, TypeDescriptor>();
        private static readonly object typeDescriptorDictionaryLock = new object();
        #endregion


        #region Methods

        /// <summary>
        /// Clear all <see cref="TypeDescriptor"/>s from <see cref="TypeCache"/>.
        /// </summary>
        public static void Clear()
        {
            typeDescriptorDictionary.Clear();
        }



        /// <summary>
        /// Get, and adds to the cache, a <see cref="TypeDescriptor"/> for a <see cref="Type"/>. 
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> for which to get the <see cref="TypeDescriptor"/>.</typeparam>
        /// <returns>A <see cref="TypeDescriptor"/> corresponding  to <typeparamref name="T"/>.</returns>
        public static TypeDescriptor GetType<T>() where T: class
        {
            return GetType(typeof(T).TypeHandle);
        }

        /// <summary>
        /// Get, and adds to the cache, a <see cref="TypeDescriptor"/> for a <see cref="RuntimeTypeHandle"/>. 
        /// </summary>
        /// <param name="runtimeTypeHandle">The <see cref="RuntimeTypeHandle"/> for which to get the <see cref="TypeDescriptor"/>.</param>
        /// <returns>A <see cref="TypeDescriptor"/> corresponding  to <paramref name="runtimeTypeHandle"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="runtimeTypeHandle"/> represents an interface.</exception>
        public static TypeDescriptor GetType(RuntimeTypeHandle runtimeTypeHandle)
        {
            //no need to check for null as RuntimeTypeHandle is a struct
            TypeDescriptor typeDescriptor;

            if (!typeDescriptorDictionary.TryGetValue(runtimeTypeHandle, out typeDescriptor))
            {
                lock (typeDescriptorDictionaryLock)
                {
                    if (!typeDescriptorDictionary.TryGetValue(runtimeTypeHandle, out typeDescriptor))
                    {
                        typeDescriptor = new TypeDescriptor(runtimeTypeHandle);
                        typeDescriptorDictionary.Add(runtimeTypeHandle, typeDescriptor);
                    }
                }
            }
            return typeDescriptor;
        }
		/// <summary>
		/// Removes <see cref="TypeDescriptor"/> for <see cref="Type"/> from cache. 
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> for which to get the <see cref="TypeDescriptor"/>.</typeparam>
		public static void RemoveType<T>() where T : class
		{
			RemoveType(typeof(T).TypeHandle);
		}

		/// <summary>
		/// Removes <see cref="TypeDescriptor"/> for <see cref="Type"/> from cache. 
		/// </summary>
		/// <param name="runtimeTypeHandle">The <see cref="RuntimeTypeHandle"/> for which to get the <see cref="TypeDescriptor"/>.</param>
		/// <exception cref="ArgumentException"><paramref name="runtimeTypeHandle"/> represents an interface.</exception>
		public static void RemoveType(RuntimeTypeHandle runtimeTypeHandle)
		{
			//no need to check for null as RuntimeTypeHandle is a struct
			TypeDescriptor typeDescriptor;

			if (typeDescriptorDictionary.TryGetValue(runtimeTypeHandle, out typeDescriptor))
			{
				lock (typeDescriptorDictionaryLock)
				{
					if (typeDescriptorDictionary.TryGetValue(runtimeTypeHandle, out typeDescriptor))
					{
						typeDescriptorDictionary.Remove(runtimeTypeHandle);
					}
				}
			}
		}
        /// <summary>
        /// A helper method that gets all <see cref="Rule"/>s of a specific type for a property on an object.
        /// </summary>
        /// <remarks>
        /// If performance is a concern it is better to call <see cref="GetRulesForProperty{TRule}(string,RuntimeTypeHandle)"/>.
        /// </remarks>
        /// <typeparam name="TRule">The type of <see cref="Rule"/> to retrieve.</typeparam>
        /// <typeparam name="TTarget">The target type to to retrieve attributes from.</typeparam>
        /// <param name="propertyName">The name of the property to get the <see cref="Rule"/>s from.</param>
        /// <returns>A <see cref="IList{T}"/> containing all <see cref="Rule"/>s for the specified property.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName"/> is a <see cref="string.Empty"/>.</exception>
        public static IList<TRule> GetRulesForProperty<TRule, TTarget>(string propertyName)
            where TRule : Rule
            where TTarget : class
        {
            return GetRulesForProperty<TRule>(propertyName, typeof(TTarget).TypeHandle);
        }


        /// <summary>
        /// A helper method that gets all <see cref="Rule"/>s for a property on an object.
        /// </summary>
        /// <typeparam name="TRule">The type of <see cref="Rule"/> to retrieve.</typeparam>
        /// <param name="propertyName">The name of the property to get the <see cref="Rule"/>s from.</param>
        /// <param name="runtimeTypeHandle">The <see cref="RuntimeTypeHandle"/> representing the type to get the <see cref="Rule"/>s from.</param>
        /// <returns>A <see cref="IList{T}"/> containing all <see cref="Rule"/>s for the specified property.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="runtimeTypeHandle"/> represents an interface.</exception>
        public static IList<TRule> GetRulesForProperty<TRule>(string propertyName, RuntimeTypeHandle runtimeTypeHandle) where TRule : Rule
        {
            //no need to check for null as RuntimeTypeHandle is a struct
            Guard.ArgumentNotNullOrEmptyString(propertyName, "propertyName");
            IList<TRule> list = new List<TRule>();
            var typeDescriptor = GetType(runtimeTypeHandle);
            var ruleCollection = typeDescriptor.Properties[propertyName].Rules;
            for (var index = 0; index < ruleCollection.Count; index++)
            {
                var propertyRule = ruleCollection[index];
                var attribute = propertyRule as TRule;
                if (attribute != null)
                {
                    list.Add(attribute);
                }
            }
            return list;
        }
#if(!WindowsCE)
		/// <summary>
		/// Get, or creates, a <see cref="PropertyDescriptor"/> for a <see cref="Type"/>. 
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> for which to get the <see cref="TypeDescriptor"/>.</typeparam>
        /// <returns>A <see cref="TypeDescriptor"/> corresponding  to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is <see cref="string.Empty"/>.</exception>
        public static PropertyDescriptor GetOrCreatePropertyDescriptor<T>(Expression<Func<T, object>> expression) where T : class
        {
            Guard.ArgumentNotNull(expression, "expression");
			var typeDescriptor = GetType(typeof(T).TypeHandle);
			var name = (((MemberExpression)expression.Body).Member).Name;
			return typeDescriptor.GetOrCreatePropertyDescriptor(name);
		}
        /// <summary>
        /// Get, or creates, a <see cref="PropertyDescriptor"/> for a <see cref="Type"/>. 
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> for which to get the <see cref="TypeDescriptor"/>.</typeparam>
        /// <returns>A <see cref="TypeDescriptor"/> corresponding  to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is <see cref="string.Empty"/>.</exception>
        internal static PropertyDescriptor GetOrCreatePropertyDescriptor<T>(Expression<Func<T>> expression)
        {
            Guard.ArgumentNotNull(expression, "expression");
            var body = (MemberExpression)expression.Body;
            var parentType = body.Expression.Type;
            var typeDescriptor = GetType(parentType.TypeHandle);
            return typeDescriptor.GetOrCreatePropertyDescriptor(expression);
        }

		/// <summary>
		/// Get, or creates, a <see cref="FieldDescriptor"/> for a <see cref="Type"/>. 
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> for which to get the <see cref="TypeDescriptor"/>.</typeparam>
        /// <returns>A <see cref="TypeDescriptor"/> corresponding  to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is <see cref="string.Empty"/>.</exception>
		public static FieldDescriptor GetOrCreateFieldDescriptor<T>(Expression<Func<T, object>> expression) where T:class
        {
            Guard.ArgumentNotNull(expression, "expression");
			var typeDescriptor = GetType(typeof(T).TypeHandle);
			var name = (((MemberExpression)expression.Body).Member).Name;
			return typeDescriptor.GetOrCreateFieldDescriptor(name);
		}
#endif
        
        #endregion
    }
}