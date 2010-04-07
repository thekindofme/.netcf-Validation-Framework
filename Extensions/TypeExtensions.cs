using System;
using System.Collections.Generic;
using System.Globalization;
#if (!WindowsCE)
using System.Linq.Expressions;
#endif
using System.Reflection;
using System.Text;
using ValidationFramework.Reflection;

namespace ValidationFramework.Extensions
{
    /// <summary>
    /// Methods to help with reflection.
    /// </summary>
    public static class TypeExtensions
    {

        #region Methods


        /// <summary>
        /// Gets a friendly name for a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the friendly name for.</param>
        /// <returns>A friendly name for a <paramref name="type"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        public static string ToUserFriendlyString(this Type type)
        {
            Guard.ArgumentNotNull(type,"type");
            var stringBuilder = new StringBuilder(TrimGenericText(type));
            var arguments = type.GetGenericArguments();
            if (arguments.Length > 0)
            {
                stringBuilder.Append("<");
                foreach (var genericArgument in arguments)
                {
                    stringBuilder.Append(ToUserFriendlyString(genericArgument));
                    stringBuilder.Append(",");
                }
                stringBuilder.Remove(stringBuilder.Length-1,1);
                stringBuilder.Append(">");
            }
            return stringBuilder.ToString();
        }


        internal static bool IsEnumDefined(Type enumType, RuntimeTypeHandle valueTypeHandle, object value, bool ignoreCase)
        {
            if (valueTypeHandle.Equals(TypePointers.StringTypeHandle))
            {
                var valueAsString = (string)value;
                if (valueAsString.Length==0)
                {
                    return true;
				}
#if (WindowsCE || SILVERLIGHT)
				var  names = EnumExtensions.GetNames(enumType);
#else
				var names = Enum.GetNames(enumType);
#endif
                if (ignoreCase)
                {
                    var toUpper = valueAsString.ToUpper();
                    for (var index = 0; index < names.Length; index++)
                    {
                        var name = names[index];
                        if (name.ToUpper() == toUpper)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    for (var index = 0; index < names.Length; index++)
                    {
                        var name = names[index];
                        if (name == valueAsString)
                        {
                            return true;
                        }
                    }
                }
            }
            else 
            {
                var valueAsLong = Convert.ToInt64(value, CultureInfo.InvariantCulture);
                if (valueAsLong < 0)
                {
                    return false;
                }
#if (WindowsCE || SILVERLIGHT)
               var values = EnumExtensions.GetValues(enumType);
#else
				var values = Enum.GetValues(enumType);
#endif
                foreach (var enumValue in values)
                {
                    if (valueAsLong == Convert.ToInt64(enumValue, CultureInfo.InvariantCulture))
                    {
                        return true;
                    }
                }
            }

            return false;
        }



        private static string TrimGenericText(Type type)
        {
            if (type.FullName != null)
            {
                var fullName = type.FullName;
                var indexOfQuote = fullName.IndexOf("`");
                if (indexOfQuote > 0)
                {
                    fullName = fullName.Substring(0, indexOfQuote); 
                }
                return fullName;
            }
            else
            {
                return type.Name;
            }
        }
        internal static ConstructorInfo GetPublicInstanceConstructor(this Type type, params Type[] types)
        {
            return type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, types, null);
        }

        internal static bool IsOutParam(this ParameterInfo parameterInfo)
        {
            return ((parameterInfo.Attributes & ParameterAttributes.Out) != ParameterAttributes.None);
        }

        internal static void SetProperty(this object target, string property, object value)
        {
        	var propertyInfo = target.GetType().GetProperty(property,BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			propertyInfo.SetValue(target, value, null);
        }


    	/// <summary>
        /// Determines whether the <see cref="Type"/> represented by the <paramref name="typeToCheck"/> derives from the <see cref="Type"/> represented by the <paramref name="baseType"/>. 
        /// </summary>
        /// <param name="typeToCheck">The child <see cref="Type"/> to check for.</param>
        /// <param name="baseType">The base <see cref="Type"/> to check for.</param>
        /// <param name="processGenericTypeDefinition"><see langword="true"/> to process the GenericTypeDefinition for each interface and base class.</param>
        /// <exception cref="ArgumentNullException"><paramref name="typeToCheck"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="baseType"/> is null.</exception>
        /// <returns>true if the <paramref name="typeToCheck"/> derives from <paramref name="baseType"/> or if they are the same type; otherwise, false.</returns>
        public static bool IsAssignableFrom(this Type baseType, Type typeToCheck, bool processGenericTypeDefinition)
        {
            Guard.ArgumentNotNull(typeToCheck, "typeToCheck");
            Guard.ArgumentNotNull(baseType, "baseType");

            if (baseType.IsAssignableFrom(typeToCheck))
            {
                return true;
            }
            if (processGenericTypeDefinition)
            {
                for (var index = 0; index < typeToCheck.GetInterfaces().Length; index++)
                {
                    var interfaceType = typeToCheck.GetInterfaces()[index];
                    if (interfaceType.IsGenericType && (interfaceType.GetGenericTypeDefinition() == baseType))
                    {
                        return true;
                    }
                }
                while (typeToCheck != null)
                {
                    if (typeToCheck.IsGenericType && (typeToCheck.GetGenericTypeDefinition() == baseType))
                    {
                        return true;
                    }
                    typeToCheck = typeToCheck.BaseType;
                }
            }

            return false;
        }



        internal static object GetStaticProperty(string typeName, string propertyName)
        {

            if ((typeName == null) && (propertyName == null))
            {
                return null;
            }
            else if ((typeName != null) && (propertyName != null))
            {
                var type = Type.GetType(typeName, true);
                return GetStaticProperty(type, propertyName);
            }
            else
            {
                throw new InvalidOperationException("Both typeName and propertyName have to be null or not null.");
            }
        }

        internal static object GetStaticProperty(Type type,string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public);
            return propertyInfo.GetValue(null, null);
        }


        /// <summary>
        /// Determine if a <see cref="MethodBase"/> represents a 'get' or 'set' method.
        /// </summary>
        /// <param name="methodBase">The <see cref="MethodBase"/> to check.</param>
        /// <returns><c>true</c> if <see cref="MethodBase"/> represents a 'get' or 'set' method; otherwise <c>false</c></returns>
        internal static bool IsPropertyMethod(MethodBase methodBase)
        {
            if (methodBase.Name.StartsWith("set_") || methodBase.Name.StartsWith("get_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static MethodBase GetBaseMethod(this MethodBase methodBase)
        {
        	var type = methodBase.DeclaringType;
        	var baseType = type.BaseType;
        	if (baseType == null)
        	{
        		return null;
        	}
        	const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        	var list = new List<Type>();
        	foreach (var parameterInfo in methodBase.GetParameters())
        	{
        		list.Add(parameterInfo.ParameterType);
        		//TODO: param modifiers
        	}
        	return baseType.GetMethod(methodBase.Name, bindingFlags, null, list.ToArray(), null);
        }

#if (!WindowsCE)
        /// <summary>
        /// Get the name of a member from an <see cref="Expression{TDelegate}"/>.
        /// </summary>
        /// <typeparam name="TMember">The <see cref="Type"/> of the member.</typeparam>
        /// <param name="expression">The lambda <see cref="Expression{TDelegate}"/> representing the member.</param>
        /// <returns>The name of the member.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is a null reference.</exception>
    	public static string GetMemberName<TMember>(Expression<Func<TMember>> expression)
        {
            Guard.ArgumentNotNull(expression, "expression");
            return (((MemberExpression)expression.Body).Member).Name;
        }
        //internal static string GetMemberName(Expression<Func> expression)
        //{
        //    Guard.ArgumentNotNull(expression, "expression");
        //    return ((MemberExpression) (((UnaryExpression)expression.Body).Operand)).Member.Name;
        //}


#endif
 

        #endregion
    }
}