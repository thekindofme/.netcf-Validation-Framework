using System;
using System.Collections.Generic;
#if(!WindowsCE)
using System.Linq.Expressions;
#endif
using System.Reflection;
using ValidationFramework.Extensions;
using System.Linq;

namespace ValidationFramework.Reflection
{
	//Design aims
	//-only one instance of TypeDescriptor should exists for each type
	/// <summary>
	/// A light-weight wrapper for <see cref="Type"/>.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
	public class TypeDescriptor
	{

		#region Fields
		
		internal const BindingFlags PropertyFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
		
		#endregion


		#region Constructors



		/// <summary>
		/// Initialize a new instance of the <see cref="TypeDescriptor"/> class.
		/// </summary>
        /// <param name="runtimeType">The <see cref="System.Type"/> for the <see cref="Type"/> to wrap.</param>
        /// <exception cref="ArgumentException"><paramref name="runtimeType"/> represents an interface.</exception>
        internal TypeDescriptor(Type runtimeType)
		{
		    //var currentType = Type.GetTypeFromHandle(runtimeTypeHandle);
            if (runtimeType.IsInterface)
		    {
                throw new ArgumentException(string.Format("Cannot create a TypeDescriptor for an interface. Interface = {0}'", runtimeType.ToUserFriendlyString()), "runtimeType");
		    }
            RuntimeType = runtimeType;

			Properties = new PropertyCollection(this);
			Fields = new FieldCollection(this);

            ProccessProperties(runtimeType);
            ProcessFields(runtimeType);

			Properties.SetToReadOnly();
			Fields.SetToReadOnly();
           
		}


		#region process properties
		private struct PropertyLink
		{
			public PropertyDescriptor PropertyDescriptor{ get; set;}
			public PropertyInfo PropertyInfo{ get; set;}
		}

		private void ProccessProperties(Type currentType)
		{
			var interfaceMapping = GetInterfaceMapping(currentType);
			var dictionary = new Dictionary<string, PropertyLink>();
			foreach (var keyValuePair in interfaceMapping)
			{
				var propertyInfo = keyValuePair.Key;
				if (!propertyInfo.PropertyType.IsGenericParameter)
				{
					var getMethod = propertyInfo.GetGetMethod(true);
					if (getMethod != null)
					{
						var propertyDescriptor = new PropertyDescriptor(this, propertyInfo, getMethod);
						var propertyLink = new PropertyLink
						                   	{
						                   		PropertyInfo = propertyInfo,
						                   		PropertyDescriptor = propertyDescriptor
						                   	};
						dictionary.Add(propertyDescriptor.Name, propertyLink);
						foreach (var interfacePropertyInfo in keyValuePair.Value)
						{
							propertyDescriptor.AddRulesForPropertyInfo(interfacePropertyInfo);
						}
					}
				}
			}
			var staticPropertyInfoList = currentType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
			foreach (var propertyInfo in staticPropertyInfoList)
			{
                if (!propertyInfo.PropertyType.IsGenericParameter)
				{
					var getMethod = propertyInfo.GetGetMethod(true);
					if (getMethod != null)
					{
						var propertyDescriptor = new PropertyDescriptor(this, propertyInfo, getMethod);
						var propertyLink = new PropertyLink
						                   	{
						                   		PropertyInfo = propertyInfo, 
						                   		PropertyDescriptor = propertyDescriptor
						                   	};
						dictionary.Add(propertyDescriptor.Name, propertyLink);
					}
				}
			} 
			ProcessBaseClassProperties(dictionary, currentType);
			AddPropertiesWithRules(dictionary);
		}

		private void AddPropertiesWithRules(Dictionary<string, PropertyLink> properties)
		{
			foreach (var propertyLink in properties.Values)
			{
				var propertyInfo = propertyLink.PropertyInfo;
				var propertyDescriptor = propertyLink.PropertyDescriptor;
				if (propertyDescriptor.Rules.Count > 0 || propertyInfo.ContainsAttribute<IncludeMemberAttribute>())
				{
					Properties.Add(propertyDescriptor);
				}
			}
		}
		private void ProcessBaseClassProperties(IDictionary<string, PropertyLink> dictionary, Type currentType)
        {
            var baseType = currentType.BaseType;
            if (baseType != null)
            {
                if (AssemblyCache.CanReflectOverAssembly(currentType.Assembly, baseType.Assembly))
                {
					var baseTypeDescriptor = TypeCache.GetType(baseType);
					foreach (var basePropertyDescriptor in baseTypeDescriptor.Properties)
					{
						if (!basePropertyDescriptor.IsStatic)
						{
							PropertyDescriptor existingPropertyDescriptor;
							PropertyLink propertyLink;
							if (dictionary.TryGetValue(basePropertyDescriptor.Name, out propertyLink))
							{
								existingPropertyDescriptor = propertyLink.PropertyDescriptor;
							}
							else
							{
								var propertyInfo = baseType.GetProperty(basePropertyDescriptor.Name, PropertyFlags);
                                var getMethodInfo = propertyInfo.GetGetMethod(true);
                                existingPropertyDescriptor = new PropertyDescriptor(this, propertyInfo, getMethodInfo);
								dictionary.Add(existingPropertyDescriptor.Name, new PropertyLink
								                                                	{
								                                                		PropertyDescriptor = existingPropertyDescriptor, 
																						PropertyInfo = propertyInfo
								                                                	});
							}
							existingPropertyDescriptor.Rules.Merge(basePropertyDescriptor.Rules);
						}
					}
                }
            }
		}


		private static Dictionary<PropertyInfo, List<PropertyInfo>> GetInterfaceMapping(Type type)
		{
			var dictionary = new Dictionary<PropertyInfo, List<PropertyInfo>>();
			var declaredPropertyDictionary = new Dictionary<string, PropertyInfo>();
			var bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			var properties = type.GetProperties(bindingFlags);
			for (var index = 0; index < properties.Length; index++)
			{
				var propertyInfo = properties[index];
				declaredPropertyDictionary.Add(propertyInfo.Name, propertyInfo);
				dictionary.Add(propertyInfo, new List<PropertyInfo>());
			}

			var basePropertyNameList = GetBasePropertyList(type);

			var interfaces = type.GetInterfaces();
			for (var interfaceIndex = 0; interfaceIndex < interfaces.Length; interfaceIndex++)
			{
				var interfaceType = interfaces[interfaceIndex];
				var interfaceProperties = interfaceType.GetProperties();
				for (var interfacePropertyIndex = 0; interfacePropertyIndex < interfaceProperties.Length; interfacePropertyIndex++)
				{
					var interfaceProperty = interfaceProperties[interfacePropertyIndex];
					//try explicit first
					var explicitName = string.Format("{0}.{1}", interfaceType.FullName.Replace('+', '.'), interfaceProperty.Name);
					if (!basePropertyNameList.Contains(explicitName) && declaredPropertyDictionary.ContainsKey(explicitName))
					{
						dictionary[declaredPropertyDictionary[explicitName]].Add(interfaceProperty);
					}
					else if (!basePropertyNameList.Contains(interfaceProperty.Name) && declaredPropertyDictionary.ContainsKey(interfaceProperty.Name))
					{
						dictionary[declaredPropertyDictionary[interfaceProperty.Name]].Add(interfaceProperty);
					}
				}
			}
			return dictionary;
		}

		private static List<string> GetBasePropertyList(Type type)
		{
			var bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic;
			var list = new List<string>();
			var baseType = type.BaseType;
			while ((baseType != null) && baseType.Assembly.ContainsAttribute<IncludeAssemblyAttribute>())
			{
				var currentTypePropertyInfoList = baseType.GetProperties(bindingFlags);
				for (var index = 0; index < currentTypePropertyInfoList.Length; index++)
				{
					var propertyInfo = currentTypePropertyInfoList[index];
					if (!list.Contains(propertyInfo.Name))
					{
						list.Add(propertyInfo.Name);
					}
				}
				baseType = baseType.BaseType;
			}
			return list;
		}

    	#endregion

		#region process Fields

		private struct FieldLink
		{
			public FieldDescriptor FieldDescriptor;
			public FieldInfo FieldInfo;
		}
		private void ProcessFields(Type currentType)
		{
			var dictionary = new Dictionary<string, FieldLink>();
			var fieldInfos = currentType.GetFields(PropertyFlags);
			for (var index = 0; index < fieldInfos.Length; index++)
			{
				var fieldInfo = fieldInfos[index];
				if (!fieldInfo.FieldType.IsGenericParameter)
				{
					var fieldDescriptor = new FieldDescriptor(this, fieldInfo);
					var fieldLink = new FieldLink
					                	{
					                		FieldDescriptor = fieldDescriptor, 
											FieldInfo = fieldInfo
					                	};
					dictionary.Add(fieldDescriptor.Name, fieldLink);
				}
			}
			ProceessBaseTypeFields(dictionary, currentType);
			AddFieldsWithRules(dictionary);

		}
		private void AddFieldsWithRules(Dictionary<string, FieldLink> fields)
		{
			foreach (var propertyLink in fields.Values)
			{
				var fieldInfo = propertyLink.FieldInfo;
				var fieldDescriptor = propertyLink.FieldDescriptor;
				if (fieldDescriptor.Rules.Count > 0 || fieldInfo.ContainsAttribute<IncludeMemberAttribute>())
				{
					Fields.Add(fieldDescriptor);
				}
			}
		}
		private void ProceessBaseTypeFields(IDictionary<string, FieldLink> dictionary, Type currentType)
		{
			var baseType = currentType.BaseType;
			if (baseType != null)
			{
				if (AssemblyCache.CanReflectOverAssembly(currentType.Assembly, baseType.Assembly))
				{
					var baseTypeDescriptor = TypeCache.GetType(baseType);
					foreach (var baseFieldDescriptor in baseTypeDescriptor.Fields)
					{
						if (!baseFieldDescriptor.IsStatic)
						{
							var newFieldDescriptor = new FieldDescriptor(this, baseFieldDescriptor.fieldInfo);
							newFieldDescriptor.Rules.Merge(baseFieldDescriptor.Rules);
							var fieldLink = new FieldLink
							                	{
							                		FieldDescriptor = newFieldDescriptor,
							                		FieldInfo = baseFieldDescriptor.fieldInfo
							                	};
							dictionary.Add(newFieldDescriptor.Name, fieldLink);
						}
					}
				}
			}
		}

    	#endregion
	
		#endregion


		#region Properties

		/// <summary>
		/// Gets the <see cref="System.Type"/> for the <see cref="Type"/> this <see cref="TypeDescriptor"/> wraps.
		/// </summary>
		public Type RuntimeType
		{
			get;
			private set;
		}

		/// <summary>
		/// Get all the <see cref="PropertyDescriptor"/>s for this <see cref="TypeDescriptor"/>.
		/// </summary>
		public PropertyCollection Properties
		{
			get;
			private set;
		}


		/// <summary>
		/// Get all the <see cref="FieldDescriptor"/>s for this <see cref="TypeDescriptor"/>.
		/// </summary>
		public FieldCollection Fields
		{
			get;
			private set;
		}

		#endregion


		#region Methods
        
#if(!WindowsCE)
		/// <summary>
		/// Get or create a <see cref="PropertyDescriptor"/> for this <see cref="TypeDescriptor"/>
		/// </summary>
		/// <remarks>If the <see cref="PropertyDescriptor"/> exists in <see cref="Properties"/> it will be returned; otherwise a new <see cref="PropertyDescriptor"/> will be created, added to <see cref="Properties"/>, and returned.</remarks>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> representing the property to get or create.</param>
		/// <returns>The existing <see cref="PropertyDescriptor"/>, if it exists in <see cref="Properties"/>; otherwise a new <see cref="PropertyDescriptor"/>, that has been created and added to <see cref="Properties"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
		public PropertyDescriptor GetOrCreatePropertyDescriptor<TProperty>(Expression<Func<TProperty>> expression)
		{
			return GetOrCreatePropertyDescriptor(TypeExtensions.GetMemberName(expression));
		}
#endif
		/// <summary>
		/// Get or create a <see cref="PropertyDescriptor"/> for this <see cref="TypeDescriptor"/>
		/// </summary>
		/// <remarks>If the <see cref="PropertyDescriptor"/> exists in <see cref="Properties"/> it will be returned; otherwise a new <see cref="PropertyDescriptor"/> will be created, added to <see cref="Properties"/>, and returned.</remarks>
		/// <param name="name">The name of the <see cref="PropertyDescriptor"/> to get or create.</param>
		/// <returns>The existing <see cref="PropertyDescriptor"/>, if it exists in <see cref="Properties"/>; otherwise a new <see cref="PropertyDescriptor"/>, that has been created and added to <see cref="Properties"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is <see cref="string.Empty"/>.</exception>
		/// <exception cref="ArgumentException">Property with <paramref name="name"/> has no get method.</exception>
		public PropertyDescriptor GetOrCreatePropertyDescriptor(string name)
		{
			Guard.ArgumentNotNullOrEmptyString(name, "name");
			PropertyDescriptor propertyDescriptor;
			if (!Properties.TryGetValue(name, out propertyDescriptor))
			{
				//var typeFromHandle = Type.GetTypeFromHandle(RuntimeTypeHandle);
                var propertyInfo = RuntimeType.GetProperty(name, PropertyFlags);
				if (propertyInfo == null)
				{
                    var canReflectOverAssembly = AssemblyCache.CanReflectOverAssembly(RuntimeType.Assembly, RuntimeType.BaseType.Assembly);
                    if (RuntimeType.BaseType != null && canReflectOverAssembly)
					{
                        var baseTypeDescriptor = TypeCache.GetType(RuntimeType.BaseType);
						propertyDescriptor = baseTypeDescriptor.GetOrCreatePropertyDescriptor(name);
					}
					else
					{
                        throw new InvalidOperationException(string.Format("Could not find property '{0}' on type '{1}'", name, RuntimeType.ToUserFriendlyString()));
					}
				}
				else
				{
                    var getMethodInfo = propertyInfo.GetGetMethod(true);
                    if (getMethodInfo == null)
                    {
                        throw new ArgumentException("Property must have a get. Private gets are allowed.", "name");
                    }
                    propertyDescriptor = new PropertyDescriptor(this, propertyInfo, getMethodInfo);
				}
				Properties.InternalAdd(propertyDescriptor);
			}
			return propertyDescriptor;
		}

        
#if(!WindowsCE)
		/// <summary>
		/// Remove a <see cref="PropertyDescriptor"/> from <see cref="Properties"/>.
		/// </summary>
		/// <returns>true if the <see cref="PropertyDescriptor"/> is successfully found and removed; otherwise, false.</returns>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> representing the property to remove.</param>
		/// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
		public bool RemovePropertyDescriptor<TProperty>(Expression<Func<TProperty>> expression)
		{
			return RemovePropertyDescriptor(TypeExtensions.GetMemberName(expression));
		}
#endif

		/// <summary>
		/// Remove a <see cref="PropertyDescriptor"/> from <see cref="Properties"/>.
		/// </summary>
		/// <returns>true if the <see cref="PropertyDescriptor"/> is successfully found and removed; otherwise, false.</returns>
		/// <param name="name">The name of the <see cref="PropertyDescriptor"/> to remove.</param>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is <see cref="string.Empty"/>.</exception>
		public bool RemovePropertyDescriptor(string name)
		{
			Guard.ArgumentNotNullOrEmptyString(name, "name");
			return Properties.InternalRemove(name);
		}


		/// <summary>
		/// Remove a <see cref="PropertyDescriptor"/> from <see cref="Properties"/>.
		/// </summary>
		/// <returns>true if the <see cref="PropertyDescriptor"/> is successfully found and removed; otherwise, false.</returns>
		/// <param name="propertyDescriptor">The <see cref="PropertyDescriptor"/> to remove.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyDescriptor"/> is null.</exception>
		public bool RemovePropertyDescriptor(PropertyDescriptor propertyDescriptor)
		{
			return Properties.InternalRemove(propertyDescriptor);
		}

		
        
#if(!WindowsCE)
		/// <summary>
		/// Get or create a <see cref="FieldDescriptor"/> for this <see cref="TypeDescriptor"/>
		/// </summary>
		/// <remarks>If the <see cref="FieldDescriptor"/> exists in <see cref="Fields"/> it will be returned; otherwise a new <see cref="FieldDescriptor"/> will be created, added to <see cref="Fields"/>, and returned.</remarks>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> representing the field to get or create.</param>
		/// <returns>The existing <see cref="FieldDescriptor"/>, if it exists in <see cref="Fields"/>; otherwise a new <see cref="FieldDescriptor"/>, that has been created and added to <see cref="Fields"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
		public FieldDescriptor GetOrCreateFieldDescriptor<TProperty>(Expression<Func<TProperty>> expression)
		{
			return GetOrCreateFieldDescriptor(TypeExtensions.GetMemberName(expression));
		}
#endif
		/// <summary>
		/// Get or create a <see cref="FieldDescriptor"/> for this <see cref="TypeDescriptor"/>
		/// </summary>
		/// <remarks>If the <see cref="FieldDescriptor"/> exists in <see cref="Fields"/> it will be returned; otherwise a new <see cref="FieldDescriptor"/> will be created, added to <see cref="Fields"/>, and returned.</remarks>
		/// <param name="name">The name of the <see cref="FieldDescriptor"/> to get or create.</param>
		/// <returns>The existing <see cref="FieldDescriptor"/>, if it exists in <see cref="Fields"/>; otherwise a new <see cref="FieldDescriptor"/>, that has been created and added to <see cref="Fields"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is <see cref="string.Empty"/>.</exception>
		public FieldDescriptor GetOrCreateFieldDescriptor(string name)
		{
			Guard.ArgumentNotNullOrEmptyString(name, "name");
            FieldDescriptor fieldDescriptor;
            if (!Fields.TryGetValue(name, out fieldDescriptor))
            {
                //var typeFromHandle = Type.GetTypeFromHandle(RuntimeType);
                var fieldInfo = RuntimeType.GetField(name, PropertyFlags);
                if (fieldInfo == null)
                {
                    var canReflectOverAssembly = AssemblyCache.CanReflectOverAssembly(RuntimeType.Assembly, RuntimeType.BaseType.Assembly);
                    if (RuntimeType.BaseType != null && canReflectOverAssembly)
                    {
                        var baseTypeDescriptor = TypeCache.GetType(RuntimeType.BaseType);
                        fieldDescriptor = baseTypeDescriptor.GetOrCreateFieldDescriptor(name);
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("Could not find field '{0}' on type '{1}'", name, RuntimeType.ToUserFriendlyString()));
                    }
                }
                else
                {
                    fieldDescriptor = new FieldDescriptor(this, fieldInfo);
                }
                Fields.InternalAdd(fieldDescriptor);
            }
            return fieldDescriptor;
		}

		
#if(!WindowsCE)
		/// <summary>
		/// Remove a <see cref="FieldDescriptor"/> from <see cref="Fields"/>.
		/// </summary>
		/// <returns>true if the <see cref="FieldDescriptor"/> is successfully found and removed; otherwise, false.</returns>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> representing the field to remove.</param>
		/// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
		public bool RemoveFieldDescriptor<TField>(Expression<Func<TField>> expression)
		{
			return RemoveFieldDescriptor(TypeExtensions.GetMemberName(expression));
		}
#endif
		/// <summary>
		/// Remove a <see cref="FieldDescriptor"/> from <see cref="Fields"/>.
		/// </summary>
		/// <returns>true if the <see cref="FieldDescriptor"/> is successfully found and removed; otherwise, false.</returns>
		/// <param name="name">The name of the <see cref="FieldDescriptor"/> to remove.</param>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is <see cref="string.Empty"/>.</exception>
		public bool RemoveFieldDescriptor(string name)
		{
			Guard.ArgumentNotNullOrEmptyString(name, "name");
			return Fields.InternalRemove(name);
		}


		/// <summary>
		/// Remove a <see cref="FieldDescriptor"/> from <see cref="Fields"/>.
		/// </summary>
		/// <returns>true if the <see cref="FieldDescriptor"/> is successfully found and removed; otherwise, false.</returns>
		/// <param name="fieldDescriptor">The <see cref="FieldDescriptor"/> to remove.</param>
		/// <exception cref="ArgumentNullException"><paramref name="fieldDescriptor"/> is null.</exception>
		public bool RemoveFieldDescriptor(FieldDescriptor fieldDescriptor)
		{
			return Fields.InternalRemove(fieldDescriptor);
		}
		/// <summary>
		/// Gets the total number of rules, for all sets, for this <see cref="FieldDescriptor"/>
		/// </summary>
		/// <returns></returns>
		public int GetRuleCount()
		{
			return Properties.Select(x => x.Rules.Count).Sum();
		}

		#endregion
	}
}