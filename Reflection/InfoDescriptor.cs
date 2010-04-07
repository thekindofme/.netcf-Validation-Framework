using System;

namespace ValidationFramework.Reflection
{
    /// <summary>
    /// A base class for reflected infos (<see cref="System.Reflection.ParameterInfo"/> and <see cref="System.Reflection.PropertyInfo"/>) that are to be cached for performance reasons.
    /// </summary>
    /// <seealso cref="PropertyDescriptor"/>
	/// <seealso cref="ParameterDescriptor"/>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public abstract class InfoDescriptor
    {

		#region Fields

		/// <summary>
		/// A static instance of typeof(IRuleAttribute).
		/// </summary>
		public static readonly Type RuleAttributeType = typeof(IRuleAttribute);

		#endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance of <see cref="InfoDescriptor"/>. Exposed for testing purposes.
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="name">The name of the <see cref="InfoDescriptor"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is a <see cref="string.Empty"/>.</exception>
        /// <exclude/>
        protected InfoDescriptor(Type runtimeType, string name)
        {
            Guard.ArgumentNotNullOrEmptyString(name, "name");
            RuntimeType = runtimeType;
            Name = name;
            Rules = new RuleCollection(this);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the name of the <see cref="InfoDescriptor"/>.
        /// </summary>
		public string Name
		{
			get; 
			private set;
		}

        /// <summary>
        /// Gets the <see cref="RuntimeType"/> for the <see cref="InfoDescriptor"/>.
        /// </summary>
        /// <remarks>For a <see cref="PropertyDescriptor"/> this will be the <see cref="RuntimeType"/> for the return <see cref="Type"/> of the get. For a <see cref="ParameterDescriptor"/> this will be the <see cref="RuntimeType"/> for the <see cref="Type"/> of the <see langword="parameter"/>.</remarks>
        public Type RuntimeType
		{
			get;
			private set;
        }

        /// <summary>
        /// Gets a <see cref="RuleCollection"/> containing all <see cref="Rule"/>s for the <see cref="InfoDescriptor"/>.
        /// </summary>
        public RuleCollection Rules
		{
			get;
			private set;
        }

        #endregion


        #region Methods

      /// <summary>
      /// Get the value for this <see cref="InfoDescriptor"/>.
      /// </summary>
      /// <param name="target">The object on which to extract the field value. If a <see cref="InfoDescriptor"/> is static, this argument is ignored.</param>
        /// <returns>The value for the <see cref="InfoDescriptor"/>.</returns>
      public abstract object GetValue(object target);
        #endregion
      }
}