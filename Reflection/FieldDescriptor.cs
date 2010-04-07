using System;
using System.Reflection;

namespace ValidationFramework.Reflection
{
    /// <summary>
    /// A light-weight wrapper for <see cref="FieldInfo"/>.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class FieldDescriptor : InfoDescriptor
    {
        #region Fields

        //private readonly FieldGetHandler invoker;
        internal readonly FieldInfo fieldInfo;

        #endregion


        #region Constructors

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <exclude/>
        protected FieldDescriptor(Type runtimeType, string name)
            : base(runtimeType, name)
        {
        }


        /// <summary>
        /// Initialize a new instance of <see cref="FieldDescriptor"/>.
        /// </summary>
        /// <param name="fieldInfo">The <see cref="FieldInfo"/> to wrap.</param>
        /// <param name="typeDescriptor">The <see cref="Reflection.TypeDescriptor"/> this <see cref="FieldDescriptor"/> belongs to.</param>
        /// <exception cref="NullReferenceException"><paramref name="fieldInfo"/> is null.</exception>
        internal FieldDescriptor(TypeDescriptor typeDescriptor, FieldInfo fieldInfo)
            : base(fieldInfo.FieldType, fieldInfo.Name)
        {
            if (fieldInfo.FieldType.IsGenericParameter)
            {
                throw new ArgumentException("fieldInfo cannot be a genetic type.", "fieldInfo");
            }
            //TODO: Use Dynamic method instead
            this.fieldInfo = fieldInfo;
            TypeDescriptor = typeDescriptor;
            IsStatic = fieldInfo.IsStatic;
//            invoker = MethodInvokerCreator.GetFieldGetInvoker(Type.GetTypeFromHandle(typeDescriptor.RuntimeTypeHandle), fieldInfo);
			var attributes = (IRuleAttribute[])fieldInfo.GetCustomAttributes(RuleAttributeType, true);
            for (var index = 0; index < attributes.Length; index++)
            {
				var fieldRuleAttribute = attributes[index];
                //Make sure each attribute is "aware" of the fieldInfo it's validating
                var rule = fieldRuleAttribute.CreateRule(this);
                Rules.Add(rule);
            }
        }




        #endregion


        #region Properties

        /// <summary>
        /// Gets a value indicating whether the <see cref="FieldDescriptor"/> is static. 
        /// </summary>
		public bool IsStatic
		{
			get; 
			private set;
		}

        /// <summary>
        /// Gets the <see cref="Reflection.TypeDescriptor"/> for this <see cref="FieldDescriptor"/>.
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
             return fieldInfo.GetValue(target);
              //return invoker(target);
          }

        #endregion
    }
}