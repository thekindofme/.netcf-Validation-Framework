using System;
using System.Collections.Generic;
using ValidationFramework.Reflection;

namespace ValidationFramework
{

    /// <summary>
    /// This is the primary class that will be utilized by the consumer when validating properties. This class responsible for exposing a simple public API to the consumer while handling the internals of invoking all <see cref="Rule"/>s properly.
    /// </summary>
    /// <seealso cref="NotifyValidatableBase"/>
    /// <seealso cref="DataErrorInfoValidatableBase"/>
    /// <seealso cref="ValidatableBase"/>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\PropertyValidationManagerExamples\CustomClassExample.cs" title="This example shows how to create your own custom class without using any of the base classes (e.g. ValidatableBase)." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\PropertyValidationManagerExamples\CustomClassExample.vb" title="This example shows how to create your own custom class without using any of the base classes (e.g. ValidatableBase)." lang="vbnet" region="Example"/>
    /// <code source="Examples\ExampleLibraryCSharp\PropertyValidationManagerExamples\ExternalExample.cs" title="This example shows how to validate a class from external code." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\PropertyValidationManagerExamples\ExternalExample.vb" title="This example shows how to validate a class from external code." lang="vbnet" region="Example"/>
    /// <code source="Examples\ExampleLibraryCSharp\PropertyValidationManagerExamples\ExternalExplicitExample.cs" title="This example shows how to validate a class that implements an interface explicitly." lang="cs" region="Example"/>
    /// <code source="Examples\ExampleLibraryVB\PropertyValidationManagerExamples\ExternalExplicitExample.vb" title="This example shows how to validate a class that implements an interface explicitly." lang="vbnet" region="Example"/>
    /// </example>
#if (!SILVERLIGHT)
    [Serializable]
#endif
	public static class PropertyValidationManager
    {
        #region Methods


        #region TryThrowException



        /// <summary>
        /// Performs validation when a property is being set.
        /// </summary>
        /// <remarks>
        /// <para>Should be called before the field (representing this property) is set.</para> 
        /// </remarks>
        /// <param name="target">An instance of the object to be validated.</param>
        /// <param name="propertyName">Property to validate. Case sensitive.</param>
        /// <param name="propertyValue">The value of the property being validated.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="PropertyDescriptor"/> could be found named <paramref name="propertyName"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        public static void ThrowException(object target, object propertyValue, string propertyName, string ruleSet, object context)
        {
            var propertyValidationManager = ValidationManagerFactory.GetPropertyValidationManager(target, ruleSet);
			propertyValidationManager.Context = context;
            propertyValidationManager.ThrowException(propertyValue, propertyName, context);
        }




        /// <summary>
        /// Performs validation when a property is being set.
        /// </summary>
        /// <remarks>
        /// <para>Should be called before the field (representing this property) is set.</para>
        /// </remarks>
        /// <param name="targetType">A <see cref="Type"/> representing the static <see cref="Type"/> to validate.</param>
        /// <param name="propertyName">Property to validate. Case sensitive.</param>
        /// <param name="propertyValue">The value of the property being validated.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="PropertyDescriptor"/> could be found named <paramref name="propertyName"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
		public static void ThrowException(Type targetType, object propertyValue, string propertyName, string ruleSet, object context)
        {
            var propertyValidationManager = ValidationManagerFactory.GetPropertyValidationManager(targetType, ruleSet);
            propertyValidationManager.ThrowException(propertyValue, propertyName, context);
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
        /// <returns>All <see cref="ValidationError"/>s for a given property.</returns>
        public static IList<ValidationError> ValidateAll(object target, string ruleSet, object context)
        {
            var propertyValidationManager = ValidationManagerFactory.GetPropertyValidationManager(target, ruleSet);
            propertyValidationManager.Context = context;
            propertyValidationManager.ValidateAll();
            return propertyValidationManager.ValidatorResultsInError;
        }


        /// <summary>
        /// Performs validation for a specific property.
        /// </summary>
        /// <remarks>
        /// <param name="targetHandle">A <see cref="RuntimeTypeHandle"/> representing the static <see cref="Type"/> to validate.</param>
        /// <param name="propertyName">Property to validate. Case sensitive.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="PropertyDescriptor"/> could be found named <paramref name="propertyName"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="Rule"/>s could be found on the <see cref="PropertyDescriptor"/>,for <paramref name="propertyName"/>, that have the RuleSet equal to <see cref="MemberValidationManager{T}.RuleSet"/>.</exception>
        /// <returns>All <see cref="ValidationError"/>s for a given property.</returns>
        public static IList<ValidationError> Validate(RuntimeTypeHandle targetHandle, string propertyName, string ruleSet, object context)
        {
            var propertyValidationManager = ValidationManagerFactory.GetPropertyValidationManager(targetHandle, ruleSet);
            propertyValidationManager.Context = context;
				propertyValidationManager.Validate(propertyName);
            return propertyValidationManager.ValidatorResultsInError;
        }


        /// <summary>
        /// Performs validation for a specific property.
        /// </summary>
        /// <param name="targetHandle">A <see cref="RuntimeTypeHandle"/> representing the static <see cref="Type"/> to validate.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="Rule"/>s could be found on the <see cref="PropertyDescriptor"/>, that have the RuleSet equal to <paramref name="ruleSet"/>.</exception>
        /// <returns>All <see cref="ValidationError"/>s for a given property.</returns>
        public static IList<ValidationError> ValidateAll(RuntimeTypeHandle targetHandle, string ruleSet, object context)
        {
            var propertyValidationManager = ValidationManagerFactory.GetPropertyValidationManager(targetHandle, ruleSet);
propertyValidationManager .Context = context;
            propertyValidationManager.ValidateAll();
            return propertyValidationManager.ValidatorResultsInError;
        }


        /// <summary>
        /// Performs validation for a specific property.
        /// </summary>
        /// <param name="target">An instance of the object to be validated.</param>
        /// <param name="propertyName">Property to validate. Case sensitive.</param>
        /// <param name="ruleSet">The rule set to validate. Use null to validate all rules. Is converted to uppercase.</param>
        /// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName"/> is <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="PropertyDescriptor"/> could be found named <paramref name="propertyName"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="ruleSet"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="Rule"/>s could be found on the <see cref="PropertyDescriptor"/>,for <paramref name="propertyName"/>, that have the RuleSet equal to <see cref="MemberValidationManager{T}.RuleSet"/>.</exception>
        /// <returns>All <see cref="ValidationError"/>s for a given property.</returns>
        public static IList<ValidationError> Validate(object target, string propertyName, string ruleSet, object context)
        {
            var propertyValidationManager = ValidationManagerFactory.GetPropertyValidationManager(target, ruleSet);
            propertyValidationManager.Context = context; 
            propertyValidationManager.Validate(propertyName);
            return propertyValidationManager.ValidatorResultsInError;
        }

        #endregion


        #endregion
    }
}