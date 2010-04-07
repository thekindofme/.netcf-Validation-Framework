using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ValidationFramework.Reflection
{
    //from http://www.codeproject.com/csharp/FastMethodInvoker.asp
    /// <summary>
    /// Delegate for calling a method that is not known at runtime.
    /// </summary>
    /// <remarks>It is a faster alternative to <see cref="MethodBase.Invoke(object,object[])"/>. An instance of <see cref="FastInvokeHandler"/> can be obtained by using <see cref="MethodInvokerCreator"/>.</remarks>
    /// <param name="target">The object on which to invoke the method. If a method is static, this argument is ignored. If a constructor is static, this argument must be a null reference.</param>
    /// <param name="parameters">An argument list for the invoked method. This is an array of objects with the same number, order, and type as the parameters of the method or constructor to be invoked. If there are no parameters, parameters should be a null reference.</param>
    /// <returns>The return value for the method or null if it doesn't return anything.</returns>
    public delegate object FastInvokeHandler(object target, params object[] parameters);


//  public delegate object FieldGetHandler(object target);
    /// <summary>
    /// Generates <see langword="FastInvokeHandler"/>s for calling methods.
    /// </summary>
    public static class MethodInvokerCreator
    {
          static readonly  Type objectType = typeof (object);
        /// <summary>
        /// Get a <see cref="FastInvokeHandler"/> for <paramref name="methodInfo"/>.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> to create the <see cref="FastInvokeHandler"/> for.</param>
        /// <returns>A <see cref="FastInvokeHandler"/> that, when invoked, will call the method represented by <paramref name="methodInfo"/>.</returns>
        public static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            Guard.ArgumentNotNull(methodInfo, "methodInfo");
            var dynamicMethod = new DynamicMethod(string.Empty, objectType, new Type[] {objectType, typeof (object[])}, methodInfo.DeclaringType);
            var ilGenerator = dynamicMethod.GetILGenerator();
            var parameterInfos = methodInfo.GetParameters();
            var parameterTypes = new Type[parameterInfos.Length];
            for (var i = 0; i < parameterTypes.Length; i++)
            {
                if (parameterInfos[i].ParameterType.IsByRef)
                {
                    parameterTypes[i] = parameterInfos[i].ParameterType.GetElementType();
                }
                else
                {
                    parameterTypes[i] = parameterInfos[i].ParameterType;
                }
            }
            var localBuilders = new LocalBuilder[parameterTypes.Length];

            for (var i = 0; i < parameterTypes.Length; i++)
            {
                localBuilders[i] = ilGenerator.DeclareLocal(parameterTypes[i], true);
            }
            for (var parameterTypeIndex = 0; parameterTypeIndex < parameterTypes.Length; parameterTypeIndex++)
            {
                ilGenerator.Emit(OpCodes.Ldarg_1);
                EmitFastInt(ilGenerator, parameterTypeIndex);
                ilGenerator.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(ilGenerator, parameterTypes[parameterTypeIndex]);
                ilGenerator.Emit(OpCodes.Stloc, localBuilders[parameterTypeIndex]);
            }
            if (!methodInfo.IsStatic)
            {
                ilGenerator.Emit(OpCodes.Ldarg_0);
            }
            for (var parameterTypeIndex = 0; parameterTypeIndex < parameterTypes.Length; parameterTypeIndex++)
            {
                if (parameterInfos[parameterTypeIndex].ParameterType.IsByRef)
                {
                    ilGenerator.Emit(OpCodes.Ldloca_S, localBuilders[parameterTypeIndex]);
                }
                else
                {
                    ilGenerator.Emit(OpCodes.Ldloc, localBuilders[parameterTypeIndex]);
                }
            }
            if (methodInfo.IsStatic)
            {
                ilGenerator.EmitCall(OpCodes.Call, methodInfo, null);
            }
            else
            {
                ilGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
            }
            if (methodInfo.ReturnType == typeof (void))
            {
                ilGenerator.Emit(OpCodes.Ldnull);
            }
            else
            {
                EmitBoxIfNeeded(ilGenerator, methodInfo.ReturnType);
            }

            for (var parameterTypeIndex = 0; parameterTypeIndex < parameterTypes.Length; parameterTypeIndex++)
            {
                if (parameterInfos[parameterTypeIndex].ParameterType.IsByRef)
                {
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(ilGenerator, parameterTypeIndex);
                    ilGenerator.Emit(OpCodes.Ldloc, localBuilders[parameterTypeIndex]);
                    if (localBuilders[parameterTypeIndex].LocalType.IsValueType)
                    {
                        ilGenerator.Emit(OpCodes.Box, localBuilders[parameterTypeIndex].LocalType);
                    }
                    ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
            }

            ilGenerator.Emit(OpCodes.Ret);
            var invoker = (FastInvokeHandler) dynamicMethod.CreateDelegate(typeof (FastInvokeHandler));
            return invoker;
        }


	  //internal static FieldGetHandler GetFieldGetInvoker(Type type, FieldInfo fieldInfo)
	  //{
	  //  DynamicMethod dynamicGet = new DynamicMethod("DynamicGet", objectType,
	  //        new Type[] { objectType }, type, true); ;
	  //  ILGenerator getGenerator = dynamicGet.GetILGenerator();

	  //  getGenerator.Emit(OpCodes.Ldarg_0);
	  //  getGenerator.Emit(OpCodes.Ldfld, fieldInfo);
	  //  if (fieldInfo.FieldType.IsValueType)
	  //  {
	  //    getGenerator.Emit(OpCodes.Box, type);
	  //  }
	  //  getGenerator.Emit(OpCodes.Ret);

	  //  return (FieldGetHandler)dynamicGet.CreateDelegate(typeof(FieldGetHandler));
	  //}

        private static void EmitCastToReference(ILGenerator ilGenerator, Type type)
        {
            if (type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Castclass, type);
            }
        }


        private static void EmitBoxIfNeeded(ILGenerator ilGenerator, Type type)
        {
            if (type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, type);
            }
        }


        private static void EmitFastInt(ILGenerator ilGenerator, int value)
        {
            switch (value)
            {
                case -1:
                    ilGenerator.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    ilGenerator.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    ilGenerator.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    ilGenerator.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    ilGenerator.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    ilGenerator.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    ilGenerator.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    ilGenerator.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    ilGenerator.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    ilGenerator.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                ilGenerator.Emit(OpCodes.Ldc_I4_S, (SByte) value);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Ldc_I4, value);
            }
        }
    }
}