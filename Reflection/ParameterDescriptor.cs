using System;
using System.Reflection;
using ValidationFramework.Extensions;

namespace ValidationFramework.Reflection
{
    /// <summary>
    /// A light-weight wrapper for <see cref="ParameterInfo"/>.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class ParameterDescriptor : InfoDescriptor
    {


        #region Constructors

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <exclude/>
        protected ParameterDescriptor(Type runtimeType, string name)
            : base(runtimeType, name)
        {
        }


        /// <summary>
        /// Initialize a new instance of the <see cref="ParameterDescriptor"/> class.
        /// </summary>
        /// <param name="methodDescriptor">The parent <see cref="MethodDescriptor"/>.</param>
        /// <param name="parameterInfo">The <see cref="ParameterInfo"/> to wrap.</param>
        internal ParameterDescriptor(MethodDescriptor methodDescriptor, ParameterInfo parameterInfo)
            : base(parameterInfo.ParameterType, parameterInfo.Name)
        {
            if (parameterInfo.IsOutParam())
            {
                var memberInfo = parameterInfo.Member;
                var declaringType = memberInfo.DeclaringType;
                throw new ArgumentException(string.Format("Cannot apply rules to an 'out' parameter. Type = '{0}'. Method = '{1}'. Parameter = '{2}'", declaringType.ToUserFriendlyString(), memberInfo.Name, parameterInfo.Name), "parameterInfo");
            }
            Position = parameterInfo.Position;
            Method = methodDescriptor;
			AddRulesForParameterInfo(parameterInfo);
        }

    	internal void AddRulesForParameterInfo(ParameterInfo parameterInfo)
    	{
    		var parameterAttributes = parameterInfo.GetCustomAttributes(RuleAttributeType, true);
    		for (var parameterRuleIndex = 0; parameterRuleIndex < parameterAttributes.Length; parameterRuleIndex++)
    		{
    			var parameterRuleAttribute = (IRuleAttribute)parameterAttributes[parameterRuleIndex];
    			var parameterRule = parameterRuleAttribute.CreateRule(this);
                Rules.Add(parameterRule);
    		}
    	}

    	#endregion


        #region Properties

        /// <summary>
        /// The parent <see cref="MethodDescriptor"/>.
        /// </summary>
        public MethodDescriptor Method
        {
        	get;
        	private set;
        }


        /// <summary>
        /// The position of the <see cref="ParameterDescriptor"/> in the methods signature.
        /// </summary>
        public int Position
        {
        	get;
        	private set;
        }

        #endregion

        #region Methods
		/// <inheritdoc />
		/// <exception cref="NotImplementedException">Always.</exception>
        public override object GetValue(object target)
      {
        throw new NotImplementedException();
      }

        #endregion
    }
}