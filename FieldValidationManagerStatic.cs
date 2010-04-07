using System;
using System.Collections.Generic;
using ValidationFramework.Reflection;

namespace ValidationFramework
{

    /// <summary>
    /// This is the primary class that will be utilized by the consumer when validating fields. This class responsible for exposing a simple public API to the consumer while handling the internals of invoking all <see cref="Rule"/>s properly.
    /// </summary>
    /// <seealso cref="NotifyValidatableBase"/>
    /// <seealso cref="DataErrorInfoValidatableBase"/>
    /// <seealso cref="ValidatableBase"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\FieldValidationManagerExamples\CustomClassExample.cs" title="This example shows how to create your own custom class without using any of the base classes (e.g. ValidatableBase)." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\FieldValidationManagerExamples\CustomClassExample.vb" title="This example shows how to create your own custom class without using any of the base classes (e.g. ValidatableBase)." lang="vbnet" region="Example"/>
    /// <code source="Examples\ExampleLibraryCSharp\FieldValidationManagerExamples\ExternalExample.cs" title="This example shows how to validate a class from external code." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\FieldValidationManagerExamples\ExternalExample.vb" title="This example shows how to validate a class from external code." lang="vbnet" region="Example"/>
    /// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public static class FieldValidationManager
    {

        #region Methods


        #region TryThrowException



        /// <summary>
        /// Performs validation when a field is being set.
        /// </summary>
        /// <remarks>
        /// <para>Should be called before the field (representing this field) is set.</para> 
        /// </remarks>
        /// <param name="target">An instance of the object to be validated.</param>
        /// <param name="fieldName">Field to validate. Case sensitive.</param>
        /// <param name="fieldValue">The value of the field being validated.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fieldName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fieldName"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="FieldDescriptor"/> could be found named <paramref name="fieldName"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        public static void ThrowException(object target, object fieldValue, string fieldName, string ruleSet, object context)
		{
            var fieldValidationManager = ValidationManagerFactory.GetPropertyValidationManager(target, ruleSet);
            fieldValidationManager.Context = context;
            fieldValidationManager.ThrowException(fieldValue, fieldName, context);
        }




        /// <summary>
        /// Performs validation when a field is being set.
        /// </summary>
        /// <remarks>
        /// <para>Should be called before the field (representing this field) is set.</para>
        /// </remarks>
        /// <param name="targetType">A <see cref="Type"/> representing the static <see cref="Type"/> to validate.</param>
        /// <param name="fieldName">Field to validate. Case sensitive.</param>
        /// <param name="fieldValue">The value of the field being validated.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fieldName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fieldName"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="FieldDescriptor"/> could be found named <paramref name="fieldName"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
		public static void ThrowException(Type targetType, object fieldValue, string fieldName, string ruleSet, object context)
        {
            var fieldValidationManager = ValidationManagerFactory.GetPropertyValidationManager(targetType, ruleSet);
			fieldValidationManager.Context= context;
            fieldValidationManager.ThrowException(fieldValue, fieldName, context);
        }

        #endregion



        #region Validate

        /// <summary>
        /// Performs validation for an object.
        /// </summary>
        /// <param name="target">An instance of the object to be validated.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        /// <returns>All <see cref="ValidationError"/>s for a given field.</returns>
        public static IList<ValidationError> ValidateAll(object target, string ruleSet, object context)
		{
            var fieldValidationManager = ValidationManagerFactory.GetFieldValidationManager(target, ruleSet);
            fieldValidationManager.Context = context;
            fieldValidationManager.ValidateAll();
        	return fieldValidationManager.ValidatorResultsInError;
        }


        /// <summary>
        /// Performs validation for a specific field.
        /// </summary>
        /// <param name="targetHandle">A <see cref="RuntimeTypeHandle"/> representing the static <see cref="Type"/> to validate.</param>
        /// <param name="fieldName">Field to validate. Case sensitive.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fieldName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fieldName"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="FieldDescriptor"/> could be found named <paramref name="fieldName"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="Rule"/>s could be found on the <see cref="FieldDescriptor"/>,for <paramref name="fieldName"/>, that have the RuleSet equal to <see cref="MemberValidationManager.RuleSet"/>.</exception>
        /// <returns>All <see cref="ValidationError"/>s for a given field.</returns>
        public static IList<ValidationError> Validate(RuntimeTypeHandle targetHandle, string fieldName, string ruleSet, object context)
		{
            var fieldValidationManager = ValidationManagerFactory.GetPropertyValidationManager(targetHandle, ruleSet);
            fieldValidationManager.Context = context;
            fieldValidationManager.Validate(fieldName);
			return fieldValidationManager.ValidatorResultsInError;
        }


        /// <summary>
        /// Performs validation for a specific field.
        /// </summary>
        /// <param name="targetHandle">A <see cref="RuntimeTypeHandle"/> representing the static <see cref="Type"/> to validate.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="Rule"/>s could be found on the <see cref="FieldDescriptor"/>, that have the RuleSet equal to <paramref name="ruleSet"/>.</exception>
        /// <returns>All <see cref="ValidationError"/>s for a given field.</returns>
        public static IList<ValidationError> ValidateAll(RuntimeTypeHandle targetHandle, string ruleSet, object context)
		{
            var fieldValidationManager = ValidationManagerFactory.GetPropertyValidationManager(targetHandle, ruleSet);
			fieldValidationManager .Context = context;
            fieldValidationManager.ValidateAll();
			return fieldValidationManager.ValidatorResultsInError;
        }


        /// <summary>
        /// Performs validation for a specific field.
        /// </summary>
        /// <param name="target">An instance of the object to be validated.</param>
        /// <param name="fieldName">Field to validate. Case sensitive.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fieldName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fieldName"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="FieldDescriptor"/> could be found named <paramref name="fieldName"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="Rule"/>s could be found on the <see cref="FieldDescriptor"/>,for <paramref name="fieldName"/>, that have the RuleSet equal to <see cref="MemberValidationManager.RuleSet"/>.</exception>
        /// <returns>All <see cref="ValidationError"/>s for a given field.</returns>
        public static IList<ValidationError> Validate(object target, string fieldName, string ruleSet, object context)
        {
            var fieldValidationManager = ValidationManagerFactory.GetPropertyValidationManager(target, ruleSet);
            fieldValidationManager.Context = context;
            fieldValidationManager.Validate(fieldName);
        	return fieldValidationManager.ValidatorResultsInError;
        }

       
        #endregion


        #endregion
    }
}