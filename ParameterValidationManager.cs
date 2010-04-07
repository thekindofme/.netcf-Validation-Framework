using System;
using System.Collections.Generic;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    /// <summary>
    /// Handles the validation of method parameters.
    /// </summary>
    /// <example>
	/// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\BasicHandleExample.cs" title="This example shows how to validate the parameters of a method using a RuntimeType." lang="cs" region="Example"/>
	/// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\BasicHandleExample.vb" title="This example shows how to validate the parameters of a method using a RuntimeType." lang="vbnet" region="Example"/>
	/// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\BasicDelegateExample.cs" title="This example shows how to validate the parameters of a method using a Delegate." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\BasicDelegateExample.vb" title="This example shows how to validate the parameters of a method using a Delegate." lang="vbnet" region="Example"/>
	/// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\ExplicitHandleExample.cs" title="This example shows how to validate the parameters of a that is an explicit implementation of an interface using a RuntimeType." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\ExplicitHandleExample.vb" title="This example shows how to validate the parameters of a that is an explicit implementation of an interface using a RuntimeType." lang="vbnet" region="Example"/>
	/// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\ExplicitDelegateExample.cs" title="This example shows how to validate the parameters of a that is an explicit implementation of an interface using a Delegate." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\ExplicitDelegateExample.vb" title="This example shows how to validate the parameters of a that is an explicit implementation of an interface using a Delegate." lang="vbnet" region="Example"/>
	/// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\InheritedFromInterfaceHandleExample.cs" title="This example shows how to validate the parameters of a method by using attributes applied to an interface using a RuntimeType." lang="cs" region="Example"/>
	/// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\InheritedFromInterfaceHandleExample.vb" title="This example shows how to validate the parameters of a method by using attributes applied to an interface using a RuntimeType." lang="vbnet" region="Example"/>
  	/// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\InheritedFromInterfaceDelegateExample.cs" title="This example shows how to validate the parameters of a method by using attributes applied to an interface using a Delegate." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\InheritedFromInterfaceDelegateExample.vb" title="This example shows how to validate the parameters of a method by using attributes applied to an interface using a Delegate." lang="vbnet" region="Example"/>
    /// </example>
    #if (!SILVERLIGHT)
    [Serializable]
#endif
    public static class ParameterValidationManager
    {

        #region Methods

        /// <summary>
        /// Validate the parameters of a method. An <see cref="ArgumentException"/> will be thrown on the first invalid parameter.
        /// </summary>
        /// <exception cref="ArgumentException">If any of the <paramref name="parameters"/> is invalid.</exception>
        /// <param name="target">The instance that the method exists on. Null for static types.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
        /// <param name="runtimeMethodHandle">The <see cref="RuntimeMethodHandle"/> that represents the method to be validated.</param>
        /// <example>
		/// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\BasicHandleExample.cs" title="This example shows how to validate the parameters of a method using a RuntimeMethodHandle." lang="cs" region="Example"/>
		/// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\BasicHandleExample.vb" title="This example shows how to validate the parameters of a method using a RuntimeMethodHandle" lang="vbnet" region="Example"/>
        /// </example>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        public static void ThrowException(object target, RuntimeMethodHandle runtimeMethodHandle, string ruleSet, params object[] parameters)
        {
			InternalValidate(runtimeMethodHandle, ruleSet, parameters, target, null, true, true);
        }


		

        /// <summary>
        /// Validate the parameters of a method. An <see cref="ArgumentException"/> will be thrown on the first invalid parameter.
        /// </summary>
        /// <exception cref="ArgumentException">If any of the <paramref name="parameters"/> is invalid.</exception>
        /// <param name="target">The instance that the method exists on. Null for static types.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
        /// <param name="runtimeMethodHandle">The <see cref="RuntimeMethodHandle"/> that represents the method to be validated.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <example>
		/// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\BasicHandleExample.cs" title="This example shows how to validate the parameters of a method using a RuntimeMethodHandle." lang="cs" region="Example"/>
		/// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\BasicHandleExample.vb" title="This example shows how to validate the parameters of a method using a RuntimeMethodHandle" lang="vbnet" region="Example"/>
        /// </example>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        public static void ThrowException(object target, RuntimeMethodHandle runtimeMethodHandle, string ruleSet, object context, params object[] parameters)
        {
			InternalValidate(runtimeMethodHandle, ruleSet, parameters, target, context, true, true);
        }


        /// <summary>
        /// Validate the parameters of a method. An <see cref="ArgumentException"/> will be thrown on the first invalid parameter.
        /// </summary>
        /// <exception cref="ArgumentException">If any of the <paramref name="parameters"/> is invalid.</exception>
        /// <param name="target">The instance that the method exists on. Null for static types.</param>
        /// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
        /// <param name="runtimeMethodHandle">The <see cref="RuntimeMethodHandle"/> that represents the method to be validated.</param>
        /// <example>
		/// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\BasicHandleExample.cs" title="This example shows how to validate the parameters of a method using a RuntimeMethodHandle." lang="cs" region="Example"/>
		/// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\BasicHandleExample.vb" title="This example shows how to validate the parameters of a method using a RuntimeMethodHandle" lang="vbnet" region="Example"/>
        /// </example>
        public static void ThrowException(object target, RuntimeMethodHandle runtimeMethodHandle, params object[] parameters)
        {
			InternalValidate(runtimeMethodHandle, null, parameters, target, null, true, true);
        }




        /// <summary>
        /// Validate the parameters of a method.
        /// </summary>
        /// <param name="target">The instance that the method exists on. Null for static types.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
        /// <param name="runtimeMethodHandle">The <see cref="RuntimeMethodHandle"/> that represents the method to be validated.</param>
        /// <returns>A <see cref="IList{T}"/> containing all <see cref="ValidationError"/>s for invalid parameters.</returns>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        public static IList<ValidationError> Validate(object target, RuntimeMethodHandle runtimeMethodHandle, string ruleSet, params object[] parameters)
        {
            return InternalValidate(runtimeMethodHandle, ruleSet, parameters, target, null, true, false);
        }



        /// <summary>
        /// Validate the parameters of a method.
        /// </summary>
        /// <param name="target">The instance that the method exists on. Null for static types.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
        /// <param name="runtimeMethodHandle">The <see cref="RuntimeMethodHandle"/> that represents the method to be validated.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <returns>A <see cref="IList{T}"/> containing all <see cref="ValidationError"/>s for invalid parameters.</returns>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        public static IList<ValidationError> Validate(object target, RuntimeMethodHandle runtimeMethodHandle, string ruleSet, object context, params object[] parameters)
        {
            return InternalValidate(runtimeMethodHandle, ruleSet, parameters, target, context, true, false);
        }



		/// <summary>
		/// Validate the parameters of a method.
		/// </summary>
		/// <param name="target">The instance that the method exists on. Null for static types.</param>
		/// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
		/// <param name="runtimeMethodHandle">The <see cref="RuntimeMethodHandle"/> that represents the method to be validated.</param>
		/// <returns>A <see cref="IList{T}"/> containing all <see cref="ValidationError"/>s for invalid parameters.</returns>
		public static IList<ValidationError> Validate(object target, RuntimeMethodHandle runtimeMethodHandle, params object[] parameters)
		{
			return InternalValidate(runtimeMethodHandle, null, parameters, target, null, true, false);
		}


        /// <summary>
        /// Validate the parameters of a method. An <see cref="ArgumentException"/> will be thrown on the first invalid parameter.
        /// </summary>
        /// <exception cref="ArgumentException">If any of the <paramref name="parameters"/> is invalid.</exception>
        /// <param name="target">The instance that the method exists on. Null for static types.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
		/// <param name="methodDelegate">The <see cref="Delegate"/> that represents the method to be validated.</param>
        /// <example>
        /// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\BasicDelegateExample.cs" title="This example shows how to validate the parameters of a method using a RuntimeMethodHandle." lang="cs" region="Example"/>
        /// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\BasicDelegateExample.vb" title="This example shows how to validate the parameters of a method using a Delegate." lang="vbnet" region="Example"/>
        /// </example>
		/// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="methodDelegate"/> is null.</exception>
		public static void ThrowException(object target, Delegate methodDelegate, string ruleSet, params object[] parameters)
		{
			Guard.ArgumentNotNull(methodDelegate, "methodDelegate");
			InternalValidate(methodDelegate.Method.MethodHandle, ruleSet, parameters, target, null, true, true);
        }


        /// <summary>
        /// Validate the parameters of a method. An <see cref="ArgumentException"/> will be thrown on the first invalid parameter.
        /// </summary>
        /// <exception cref="ArgumentException">If any of the <paramref name="parameters"/> is invalid.</exception>
        /// <param name="target">The instance that the method exists on. Null for static types.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
		/// <param name="methodDelegate">The <see cref="Delegate"/> that represents the method to be validated.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <example>
        /// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\BasicDelegateExample.cs" title="This example shows how to validate the parameters of a method using a RuntimeMethodHandle." lang="cs" region="Example"/>
        /// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\BasicDelegateExample.vb" title="This example shows how to validate the parameters of a method using a Delegate." lang="vbnet" region="Example"/>
        /// </example>
		/// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="methodDelegate"/> is null.</exception>
		public static void ThrowException(object target, Delegate methodDelegate, string ruleSet, object context, params object[] parameters)
		{
			Guard.ArgumentNotNull(methodDelegate, "methodDelegate");
			InternalValidate(methodDelegate.Method.MethodHandle, ruleSet, parameters, target, context, true, true);
        }



        /// <summary>
        /// Validate the parameters of a method. An <see cref="ArgumentException"/> will be thrown on the first invalid parameter.
        /// </summary>
        /// <exception cref="ArgumentException">If any of the <paramref name="parameters"/> is invalid.</exception>
        /// <param name="target">The instance that the method exists on. Null for static types.</param>
        /// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
		/// <param name="methodDelegate">The <see cref="Delegate"/> that represents the method to be validated.</param>
        /// <example>
        /// <code source="Examples\ExampleLibraryCSharp\ParameterValidationManagerExamples\BasicDelegateExample.cs" title="This example shows how to validate the parameters of a method using a RuntimeMethodHandle." lang="cs" region="Example"/>
        /// <code source="Examples\ExampleLibraryVB\ParameterValidationManagerExamples\BasicDelegateExample.vb" title="This example shows how to validate the parameters of a method using a Delegate." lang="vbnet" region="Example"/>
		/// </example>
		/// <exception cref="ArgumentNullException"><paramref name="methodDelegate"/> is null.</exception>
		public static void ThrowException(object target, Delegate methodDelegate, params object[] parameters)
		{
			Guard.ArgumentNotNull(methodDelegate, "methodDelegate");
			InternalValidate(methodDelegate.Method.MethodHandle, null, parameters, target, null, true, true);
        }


        /// <summary>
        /// Validate the parameters of a method.
        /// </summary>
        /// <param name="target">The instance that the method exists on. Null for static types.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
		/// <param name="methodDelegate">The <see cref="Delegate"/> that represents the method to be validated.</param>
        /// <returns>A <see cref="IList{T}"/> containing all <see cref="ValidationError"/>s for invalid parameters.</returns>
		/// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="methodDelegate"/> is null.</exception>
		public static IList<ValidationError> Validate(object target, Delegate methodDelegate, string ruleSet, params object[] parameters)
		{
			Guard.ArgumentNotNull(methodDelegate, "methodDelegate");
			return InternalValidate(methodDelegate.Method.MethodHandle, ruleSet, parameters, target, null, true, false);
        }



        /// <summary>
        /// Validate the parameters of a method.
        /// </summary>
        /// <param name="target">The instance that the method exists on. Null for static types.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
		/// <param name="methodDelegate">The <see cref="Delegate"/> that represents the method to be validated.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <returns>A <see cref="IList{T}"/> containing all <see cref="ValidationError"/>s for invalid parameters.</returns>
		/// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="methodDelegate"/> is null.</exception>
		public static IList<ValidationError> Validate(object target, Delegate methodDelegate, string ruleSet, object context, params object[] parameters)
		{
			Guard.ArgumentNotNull(methodDelegate, "methodDelegate");
			return InternalValidate(methodDelegate.Method.MethodHandle, ruleSet, parameters, target, context, true, false);
        }


		/// <summary>
		/// Validate the parameters of a method.
		/// </summary>
		/// <param name="target">The instance that the method exists on. Null for static types.</param>
		/// <param name="parameters">The values of the parameters to validate. In the same order as they appear in the methods signature.</param>
		/// <param name="methodDelegate">The <see cref="Delegate"/> that represents the method to be validated.</param>
		/// <returns>A <see cref="IList{T}"/> containing all <see cref="ValidationError"/>s for invalid parameters.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="methodDelegate"/> is null.</exception>
		public static IList<ValidationError> Validate(object target, Delegate methodDelegate, params object[] parameters)
		{
			Guard.ArgumentNotNull(methodDelegate, "methodDelegate");
			return InternalValidate(methodDelegate.Method.MethodHandle, null, parameters, target, null, true, false);
		}



        private static IList<ValidationError> InternalValidate(RuntimeMethodHandle runtimeMethodHandle, string ruleSet, object[] parameters, object target, object context, bool throwOnNoRulesFoundException, bool throwOnFirstInvalid)
        {
            Guard.ArgumentNotEmptyString(ruleSet, "ruleSet");
            ruleSet = ruleSet.ToUpperIgnoreNull();
            var list = new List<ValidationError>();
            var methodDescriptor = MethodCache.GetMethod(runtimeMethodHandle);
			if (methodDescriptor.Parameters.Count != parameters.Length)
			{
				throw new ArgumentException(string.Format("Incorrect number of parameters. All parameter have to be provided in the same order that they exist in the method. Method name={0}. Expected parameter count={1}. Parameters passed in count={2}.", methodDescriptor.Name, methodDescriptor.Parameters.Count, parameters.Length), "parameters");
			}

			foreach (var parameter in methodDescriptor.Parameters)
			{
				var parameterValue = parameters[parameter.Position];
			    var rules = parameter.Rules.GetRulesForRuleSet(ruleSet);
                for (var ruleIndex = 0; ruleIndex < rules.Count; ruleIndex++)
                {
                    var rule = rules[ruleIndex];
                    var validationError = rule.BaseValidate(target, parameterValue, context, null);
                    if (validationError != null)
                    {
                        if (throwOnFirstInvalid)
                        {
                            throw new ArgumentException(rule.ErrorMessage, parameter.Name);
                        }
                        list.Add(validationError);
                    }
                }
			}
            return list;
        }


		
	

        #endregion
    }
}