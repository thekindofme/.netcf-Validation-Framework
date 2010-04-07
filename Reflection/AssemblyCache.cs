using System.Collections.Generic;
using System.Reflection;
using ValidationFramework.Extensions;

namespace ValidationFramework.Reflection
{
    internal static class AssemblyCache
    {
        #region Fields

        private static readonly Dictionary<string, bool> reflectionAssemblies;

        #endregion


        static AssemblyCache()
        {
        	reflectionAssemblies = new Dictionary<string, bool>();
        }

        #region Methods

        internal static bool CanReflectOverAssembly(Assembly currentTypeAssembly, Assembly baseTypeAssembly)
        {
            if (currentTypeAssembly == baseTypeAssembly)
            {
                return true;
            }
            else
            {
                bool reflectOverAssembly;
                var fullName = baseTypeAssembly.FullName;
                if (!reflectionAssemblies.TryGetValue(fullName, out reflectOverAssembly))
                {
                    if (baseTypeAssembly.ContainsAttribute<IncludeAssemblyAttribute>())
                    {
                        reflectionAssemblies.Add(fullName, reflectOverAssembly);
                    }
                }
                return reflectOverAssembly;
            }
        }

        #endregion
    }
}
