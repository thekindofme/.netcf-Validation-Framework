using System;
using System.Reflection;

namespace ValidationFramework.Reflection
{
    /// <summary>
    /// A light-weight wrapper for <see cref="PropertyInfo"/>.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class PropertyDescriptor : InfoDescriptor
    {
        #region Fields
#if(WindowsCE || SILVERLIGHT)
		private readonly MethodInfo getMethodInfo;
#else
        private readonly FastInvokeHandler invoker;
#endif
        #endregion


        #region Constructors

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <exclude/>
        protected PropertyDescriptor(Type runtimeType, string name)
            : base(runtimeType, name)
        {
        }


        /// <summary>
        /// Initialize a new instance of <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> to wrap.</param>
        /// <param name="typeDescriptor">The <see cref="Reflection.TypeDescriptor"/> this <see cref="PropertyDescriptor"/> belongs to.</param>
        /// <param name="getMethodInfo">The get <see cref="MethodInfo"/> for the <see cref="PropertyInfo"/> being wrapped.</param>
        /// <exception cref="NullReferenceException"><paramref name="propertyInfo"/> is null.</exception>
        internal PropertyDescriptor(TypeDescriptor typeDescriptor, PropertyInfo propertyInfo, MethodInfo getMethodInfo)
            : base(getMethodInfo.ReturnType, propertyInfo.Name)
        {
            if (propertyInfo.PropertyType.IsGenericParameter)
            {
                throw new ArgumentException("propertyInfo cannot be a genetic type.", "propertyInfo");
            }
            TypeDescriptor = typeDescriptor;
            IsStatic = getMethodInfo.IsStatic;
            IsVirtual = getMethodInfo.IsVirtual;
#if(WindowsCE || SILVERLIGHT)
            this.getMethodInfo = getMethodInfo;
#else
            invoker = MethodInvokerCreator.GetMethodInvoker(getMethodInfo);
#endif
			AddRulesForPropertyInfo(propertyInfo);
        }


        /// <summary>
        /// Gets a value indicating whether the property is virtual.
        /// </summary>
        public bool IsVirtual { get; private set; }

        internal void AddRulesForPropertyInfo(PropertyInfo propertyInfo)
        {
            var attributes = (IRuleAttribute[])propertyInfo.GetCustomAttributes(RuleAttributeType, true);
            for (var index = 0; index < attributes.Length; index++)
            {
                var propertyRuleAttribute = attributes[index];
                //Make sure each attribute is "aware" of the propertyInfo it's validating
                var rule = propertyRuleAttribute.CreateRule(this);
                Rules.Add(rule);
            }
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets a value indicating whether the <see cref="PropertyDescriptor"/> is static. 
        /// </summary>
		public bool IsStatic
		{
			get;
			private set;
		}

        /// <summary>
        /// Gets the <see cref="Reflection.TypeDescriptor"/> for this <see cref="PropertyDescriptor"/>.
        /// </summary>
        public TypeDescriptor TypeDescriptor
        {
			get;
			private set;
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        public override object GetValue(object target)
		{
#if(WindowsCE || SILVERLIGHT)
			return getMethodInfo.Invoke(target, null);
#else
            return invoker(target);
#endif
        }

        #endregion
    }
}