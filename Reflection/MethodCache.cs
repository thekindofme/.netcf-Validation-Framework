using System;
using System.Collections.Generic;

namespace ValidationFramework.Reflection
{
    /// <summary>
    /// Provides an in memory cache of <see cref="MethodDescriptor"/>s.
    /// </summary>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Reflection\MethodCacheExample.cs" title="The following example shows how to programmatically add validation Rules to a method." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\Reflection\MethodCacheExample.vb" title="The following example shows how to programmatically add validation Rules to a method." lang="vbnet" region="Example"/>
    /// </example>
    public static class MethodCache
    {
        #region Fields

        private static readonly Dictionary<RuntimeMethodHandle, MethodDescriptor> methodCache = new Dictionary<RuntimeMethodHandle, MethodDescriptor>();
        private static readonly object methodCacheDictionaryLock = new object();

        #endregion


        #region Methods

        /// <summary>
        /// Clear all <see cref="MethodDescriptor"/>s from <see cref="MethodCache"/>.
        /// </summary>
        public static void Clear()
        {
            methodCache.Clear();
        }

		/// <summary>
		/// Get a <see cref="MethodDescriptor"/> for a <see cref="RuntimeTypeHandle"/>.
		/// </summary>
		/// <param name="methodDelegate">The <see cref="Delegate"/> representing the method for which to get the <see cref="MethodDescriptor"/>.</param>
		/// <returns>A <see cref="MethodDescriptor"/> corresponding to <paramref name="methodDelegate"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="methodDelegate"/> is null.</exception>
		public static MethodDescriptor GetMethod(Delegate methodDelegate)
		{
			Guard.ArgumentNotNull(methodDelegate, "methodDelegate");
			return GetMethod(methodDelegate.Method.MethodHandle);
		}

    	/// <summary>
        /// Get a <see cref="MethodDescriptor"/> for a <see cref="RuntimeTypeHandle"/>.
        /// </summary>
        /// <param name="runtimeMethodHandle">The <see cref="RuntimeTypeHandle"/> for which to get the <see cref="MethodDescriptor"/>.</param>
        /// <returns>A <see cref="MethodDescriptor"/> corresponding to <paramref name="runtimeMethodHandle"/>.</returns>
        public static MethodDescriptor GetMethod(RuntimeMethodHandle runtimeMethodHandle)
        {
            MethodDescriptor methodDescriptor;
            var success = methodCache.TryGetValue(runtimeMethodHandle, out methodDescriptor);
            if (!success)
            {
                lock (methodCacheDictionaryLock)
                {
                    if (!methodCache.TryGetValue(runtimeMethodHandle, out methodDescriptor))
                    {
						methodDescriptor = new MethodDescriptor(runtimeMethodHandle);
						methodCache.Add(runtimeMethodHandle, methodDescriptor);
                    }
                }
            }
            return methodDescriptor;
        }

        #endregion
    }
}