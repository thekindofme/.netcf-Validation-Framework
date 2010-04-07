using System.Collections.Generic;
using System.Reflection;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Provides access to all <see cref="TypeDescriptor"/>s for an <see cref="Assembly"/>.
    /// </summary>
    public static class RuleDescriber
    {
        /// <summary>
        /// Read all the <see cref="TypeDescriptor"/>s for an <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to read attributes from.</param>
        /// <returns>All the <see cref="TypeDescriptor"/>s for an <see cref="Assembly"/>.</returns>
        public static IList<TypeDescriptor> GetPropertyRules(Assembly assembly)
        {
            var list = new List<TypeDescriptor>();

            var types = assembly.GetTypes();
            for (var typeIndex = 0; typeIndex < types.Length; typeIndex++)
            {
                var type = types[typeIndex];
                //we only care about classes.
                if (type.IsClass)
                {
                    var typeDescriptor = TypeCache.GetType(type.TypeHandle);
                    if (typeDescriptor != null)
                    {
                        foreach (var propertyDescriptor in typeDescriptor.Properties)
                        {
                            if (propertyDescriptor.Rules.Count > 0)
                            {
                                list.Add(typeDescriptor);
                                break;
                            }
                        }
                    }
                }
            }
            return list;
        }


        /// <summary>
        /// Read all the <see cref="MethodDescriptor"/>s for an <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to read attributes from.</param>
        /// <returns>All the <see cref="MethodDescriptor"/>s for an <see cref="Assembly"/>.</returns>
        public static IList<MethodDescriptor> GetMethodRules(Assembly assembly)
        {
            var list = new List<MethodDescriptor>();

            for (var typeIndex = 0; typeIndex < assembly.GetTypes().Length; typeIndex++)
            {
                var type = assembly.GetTypes()[typeIndex];
                //we only care about classes and interfaces.
                if (type.IsClass || type.IsInterface)
                {
                    foreach (var methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                    {
                        //TODO: support generic methods
                        if (!TypeExtensions.IsPropertyMethod(methodInfo) && !methodInfo.ContainsGenericParameters && methodInfo.GetParameters().Length > 0)
                        {
                            var descriptor = MethodCache.GetMethod(methodInfo.MethodHandle);
                            foreach (var parameter in descriptor.Parameters)
                            {
                                if (parameter.Rules.Count > 0)
                                {
                                    list.Add(descriptor);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}