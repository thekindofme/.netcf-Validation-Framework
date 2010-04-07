using System;
using System.Reflection;
using ValidationFramework.Extensions;

namespace ValidationFramework.Reflection
{
    /// <summary>
    /// A light-weight wrapper for <see cref="MethodBase"/>.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class MethodDescriptor
    {
   
        #region Constructors

        /// <summary>
        /// Initialize a new instance of <see cref="MethodDescriptor"/> class.
        /// </summary>
        /// <param name="runtimeMethodHandle">The <see cref="System.RuntimeMethodHandle"/> of the <see cref="MethodBase"/> to wrap.</param>
        internal MethodDescriptor(RuntimeMethodHandle runtimeMethodHandle)
        {
            var method = MethodBase.GetMethodFromHandle(runtimeMethodHandle);
            if (TypeExtensions.IsPropertyMethod(method))
            {
                //Dont throw for indexer properties
                //TODO: hack should be a better way of doing this
                if ((method.Name != "get_Item") && !method.Name.StartsWith("set_Item"))
                {
                    throw new ArgumentException("Creating a MethodDescriptor for a property is not supported.", "runtimeMethodHandle");
                }
            }
            Parameters = new ParameterCollection(this);
			RuntimeMethodHandle = runtimeMethodHandle;
            Name = method.Name;

            IsStatic = method.IsStatic;
            AppendParameters(method);
        	ProcessInterfaces(method);
            ProcessBaseMethods(method);
            Parameters.SetToReadOnly();
        }

        private void ProcessBaseMethods(MethodBase method)
        {
            var baseMethod = method.GetBaseMethod();
            if (baseMethod != null && AssemblyCache.CanReflectOverAssembly(method.DeclaringType.Assembly, baseMethod.DeclaringType.Assembly))
            {
                var methodDescriptor = MethodCache.GetMethod(baseMethod.MethodHandle);
                foreach (var baseParameterDescriptor in methodDescriptor.Parameters)
                {
                    var existingParameterDescriptor = Parameters[baseParameterDescriptor.Position];
                    existingParameterDescriptor.Rules.Merge(baseParameterDescriptor.Rules);
                }
            }
        }

#if(WindowsCE)

        private void ProcessInterfaces(MethodBase method)
        {
            if (!method.IsStatic)
            {
                var declaringType = method.DeclaringType;
                var interfaces = declaringType.GetInterfaces();
            	var parameters = method.GetParameters();
            	for (var index = 0; index < interfaces.Length; index++)
            	{
            		var interfaceType = interfaces[index];
            		var interfaceMethods = interfaceType.GetMethods();
            		foreach (var interfaceMethod in interfaceMethods)
            		{
            			var name = method.Name.GetLastAfter(".");
            			if (interfaceMethod.Name == name)
						{
							var interfaceParameters = interfaceMethod.GetParameters();
							if (interfaceParameters.Length == parameters.Length)
							{
								var parametersMatch = true;
								for (var paramIndex = 0; paramIndex < interfaceParameters.Length; paramIndex++)
								{
									var interfaceMethodParameter = interfaceParameters[paramIndex];
									var methodParameter = parameters[paramIndex];
									if (interfaceMethodParameter.ParameterType != methodParameter.ParameterType)
									{
										parametersMatch = false;
										break;
									}
								}
								if (parametersMatch)
								{
									ProcessMethodInfo(interfaceMethod);
									break;
								}

							}
						}
            		}
            	}
            }
        }

#else
        
        private void ProcessInterfaces(MethodBase method)
        {
            if (!method.IsStatic)
            {
                var declaringType = method.DeclaringType;
                var interfaces = declaringType.GetInterfaces();
              	for (var index = 0; index < interfaces.Length; index++)
            	{
            		var interfaceType = interfaces[index];
                    var map = declaringType.GetInterfaceMap(interfaceType);
                    for (var interfaceMapIndex = 0; interfaceMapIndex < map.InterfaceMethods.Length; interfaceMapIndex++)
                    {
                        var interfaceMethodInfo = map.InterfaceMethods[interfaceMapIndex];
                        var targetMethodInfo = map.TargetMethods[interfaceMapIndex];
                        if (targetMethodInfo == method)
                        {
                            ProcessMethodInfo(interfaceMethodInfo);
                        }
                    }
                }
            }
        }

#endif
		private void ProcessMethodInfo(MethodBase interfaceMethodInfo)
        {
            var parameterInfos = interfaceMethodInfo.GetParameters();

            for (var index = 0; index < parameterInfos.Length; index++)
            {
                var parameterInfo = parameterInfos[index];
                var parameterDescriptor = Parameters[index];
                parameterDescriptor.AddRulesForParameterInfo(parameterInfo);
            }
        }

        private void AppendParameters(MethodBase method)
    	{
    		var parameterInfos = method.GetParameters();

    		for (var index = 0; index < parameterInfos.Length; index++)
    		{
    		    var parameterInfo = parameterInfos[index];
    		    var parameterDescriptor = new ParameterDescriptor(this, parameterInfo);
    		    Parameters.Add(parameterDescriptor);
    		}
    	}

    	#endregion


        #region Properties

        /// <summary>
        /// Gets the name of the <see cref="MethodDescriptor"/>.
        /// </summary>
        public string Name
        {
			get;
			private set;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="MethodDescriptor"/> is static. 
        /// </summary>
        public bool IsStatic
		{
			get;
			private set;
        }

        /// <summary>
        /// Gets a <see cref="ParameterCollection"/> containing all <see cref="ParameterDescriptor"/>s for the <see cref="MethodDescriptor"/>.
        /// </summary>
        /// <remarks>This <see cref="ParameterCollection"/> is set to readonly. <see cref="AutoKeyDictionary{TKey,TValue}.IsReadOnly"/></remarks>
        public ParameterCollection Parameters
        {
			get;
			private set;
        }


        /// <summary>
        /// Gets the <see cref="System.RuntimeMethodHandle"/> that represents the <see cref="MethodInfo"/> that this <see cref="MethodDescriptor"/> returns.
        /// </summary>
        public RuntimeMethodHandle RuntimeMethodHandle
        {
			get;
			private set;
        }

        #endregion
    }
}