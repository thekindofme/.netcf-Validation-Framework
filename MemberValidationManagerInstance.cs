using System;
using System.Collections.Generic;
#if(!WindowsCE)
using System.Linq.Expressions;
#endif
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
	/// <summary>
	/// Base class for field and property validation.
	/// </summary>
	/// <seealso cref="PropertyValidationManager"/>
	/// <seealso cref="FieldValidationManager"/>
	public partial class MemberValidationManager
	{
		#region Fields

		/// <summary>
		/// A <see cref="Dictionary{TKey,TValue}"/> containing all <see cref="Rule"/>s and <see cref="ValidationError"/>s.
		/// </summary>
		/// <remarks>
		/// This may change in the future.
		/// </remarks>
		protected readonly Dictionary<Rule, ValidationError> errorDictionary;


	    private readonly Dictionary<string, KeyValuePair<InfoDescriptor, List<Rule>>> dictionary;
		#endregion


		#region Constructors

	    public MemberValidationManager(IEnumerable<KeyValuePair<InfoDescriptor, List<Rule>>> ruleDictionary)
		{
            dictionary = new Dictionary<string, KeyValuePair<InfoDescriptor, List<Rule>>>();
			errorDictionary = new Dictionary<Rule, ValidationError>();
		    foreach (var keyValuePair in ruleDictionary)
		    {
		        dictionary.Add(keyValuePair.Key.Name, new KeyValuePair<InfoDescriptor, List<Rule>>(keyValuePair.Key, new List<Rule>(keyValuePair.Value)));
		    }
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets a value indicating if all members are valid.
		/// </summary>
		/// <remarks>Calling this property does not perform a validation it only checks the current state. To perform a full validation call 
		/// <see cref="MemberValidationManager{T}.ValidateAll()"/>.</remarks>
		public bool IsValid
		{
			get
			{
				return (errorDictionary.Count == 0);
			}
		}

		/// <summary>
		/// Gets the <see cref="Reflection.TypeDescriptor"/> for the <see cref="Type"/> that this <see cref="MemberValidationManager"/> is validating.
		/// </summary>
		public TypeDescriptor TypeDescriptor
		{
			get
			{
				//get this from the cache each time. this will allow the removal of types from the cache in the future to save on memory.
				return TypeCache.GetType(TargetType);
			}
		}


		/// <summary>
		/// Gets the instance of the object that this <see cref="MemberValidationManager"/> is handling.
		/// </summary>
		/// <remarks>Will return a null for static types.</remarks>
		public Object Target
		{
			get;
			set;
		}


		/// <summary>
		/// Gets a <see cref="IList{T}"/> containing all <see cref="ValidationError"/> in error.
		/// </summary>
		/// <remarks>This is a copy of the actual list of <see cref="ValidationError"/>s.</remarks>
		public IList<ValidationError> ValidatorResultsInError
		{
			get
			{
				return new List<ValidationError>(errorDictionary.Values);
			}
		}


		/// <summary>
		/// Gets the <see cref="Type"/> for the <see cref="Type"/> being validated.
		/// </summary>
		public Type TargetType
		{
			get;
			set;
		}


		private string ruleSet;

		/// <summary>
		/// Gets the rule set to validate.
		/// </summary>
		/// <remarks>
		/// Only <see cref="Rule"/>s where RuleSet equals this value.
		/// Case insensitive so this will always return a upper case string no matter what is passed into the constructor.
		/// </remarks>
		/// <exception cref="ArgumentException"><paramref name="value"/> is a <see cref="string.Empty"/>.</exception>
		public string RuleSet
		{
			get
			{
				return ruleSet;
			}
			set
			{
				Guard.ArgumentNotEmptyString(ruleSet, "ruleSet");
				ruleSet = value.ToUpperIgnoreNull();
			}
		}

		/// <summary>
		/// An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. The default is null.
		/// </summary>
		public object Context
		{
			get;
			set;
		}


		#endregion


		#region Methods

		/// <summary>
		/// Get a <see cref="IList{T}"/> of all <see cref="ValidationError"/>s for a given member.
		/// </summary>
		/// <remarks>Use <see cref="ResultFormatter"/> for some basic formatting conversions of <see cref="ValidationError"/>s.</remarks>
		/// <param name="memberName">Member to retrieve error message for. Case sensitive.</param>
		/// <returns>All <see cref="ValidationError"/>s for a given member.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="memberName"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="memberName"/> is <see cref="string.Empty"/>.</exception>
		/// <exception cref="ArgumentException">No <see cref="InfoDescriptor"/> could be found named <paramref name="memberName"/>.</exception>
        public IList<ValidationError> GetResultsInError(string memberName)
		{
		    Guard.ArgumentNotNullOrEmptyString(memberName, "memberName");
		    var errors = new List<ValidationError>();
		    foreach (var validator in errorDictionary.Values)
		    {
		        if (validator.InfoDescriptor.Name == memberName)
		        {
		            errors.Add(validator);
		        }
		    }
		    return errors;
		    //else
		    //{
		    //    throw new ArgumentException(string.Format("A member named '{0}' could not be found containing rules.", memberName), "memberName");
		    //}
		}

#if (!WindowsCE)
		/// <summary>
		/// Get a <see cref="IList{T}"/> of all <see cref="ValidationError"/>s for a given member.
		/// </summary>
		/// <remarks>Use <see cref="ResultFormatter"/> for some basic formatting conversions of <see cref="ValidationError"/>s.</remarks>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> to retrieve error message for. </param>
		/// <returns>All <see cref="ValidationError"/>s for a given member.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
		/// <exception cref="ArgumentException">No <see cref="InfoDescriptor"/> could be found based on <paramref name="expression"/>.</exception>
		public IList<ValidationError> GetResultsInError<TMember>(Expression<Func<TMember>> expression)
		{
			return GetResultsInError(TypeExtensions.GetMemberName(expression));
		}
#endif

		/// <summary>
		/// Get a <see cref="IList{T}"/> of all <see cref="ValidationError"/>s for a list of properties.
		/// </summary>
		/// <remarks>Use <see cref="ResultFormatter"/> for some basic formatting conversions of <see cref="ValidationError"/>s.</remarks>
		/// <param name="memberNames">The names of the properties to retrieve error messages for. Case sensitive.</param>
		/// <returns>All <see cref="ValidationError"/>s for the list of properties.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="memberNames"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Length of <paramref name="memberNames"/> is 0.</exception>
		/// <exception cref="ArgumentException">No <see cref="InfoDescriptor"/> could be found any single member <paramref name="memberNames"/>.</exception>
		public IList<ValidationError> GetResultsInErrorForList(params string[] memberNames)
		{
			Guard.ArgumentNotNull(memberNames, "memberNames");

			if (memberNames.Length == 0)
			{
				throw new ArgumentOutOfRangeException("memberNames");
			}

			var validationErrors = new List<ValidationError>();
			foreach (var memberName in memberNames)
			{
				validationErrors.AddRange(GetResultsInError(memberName));
			}
			return validationErrors;
		}


	    /// <summary>
		/// Populates <see cref="errorDictionary"/> with <see cref="ValidationError"/> for the member defined in <paramref name="infoDescriptor"/>.
		/// </summary>
		/// <param name="infoDescriptor">The <see cref="InfoDescriptor"/> for the member to validate.</param>
		/// <param name="memberValue">The value of the member to validate.</param>
		/// <returns><see langword="true"/> if a <see cref="Rule"/> is found; otherwise <see langword="false"/>.</returns>
		protected void CheckValidMember(InfoDescriptor infoDescriptor, object memberValue)
		{
			var rules= infoDescriptor.Rules.GetRulesForRuleSet(RuleSet);

				for (var index = 0; index < rules.Count; index++)
				{
                    var rule = rules[index];
                    var validationError = rule.BaseValidate(Target, memberValue, Context, infoDescriptor);
					errorDictionary.Remove(rule);
					if (validationError != null)
					{
						errorDictionary.Add(rule, validationError);
					}
				}
		}

		internal void InternalThrowException(InfoDescriptor infoDescriptor, object value, object context, bool throwException)
		{
		    var rules = infoDescriptor.Rules.GetRulesForRuleSet(RuleSet);

		    for (var index = 0; index < rules.Count; index++)
		    {
		        var rule = rules[index];
		        var validationError = rule.BaseValidate(Target, value, context, null);
		        if (validationError != null)
		        {
		            throw new ArgumentException(validationError.ErrorMessage, "value");
		        }
		    }
		}

	    #region Validate

		/// <summary>
		/// Validates all properties.
		/// </summary>
		/// <exception cref="InvalidOperationException">No <see cref="Rule"/>s can be found.</exception>
		public void ValidateAll()
		{
			errorDictionary.Clear();
            foreach (var keyValuePair in dictionary.Values)
            {
                var memberValue = keyValuePair.Key.GetValue(Target);
                CheckValidMember(keyValuePair.Key, memberValue);
            }
		}


		/// <summary>
		/// Validates the specified member.
		/// </summary>
		/// <param name="memberName">Member to validate. Case sensitive.</param>
		/// <exception cref="ArgumentNullException"><paramref name="memberName"/> is a null reference.</exception>
		/// <exception cref="ArgumentException"><paramref name="memberName"/> is <see cref="string.Empty"/>.</exception>
		/// <exception cref="ArgumentException">No <see cref="InfoDescriptor"/> could be found named <paramref name="memberName"/>.</exception>
		/// <exception cref="ArgumentException">No <see cref="Rule"/>s could be found on the <see cref="InfoDescriptor"/>,for <paramref name="memberName"/>, that have the RuleSet equal to <see cref="MemberValidationManager.RuleSet"/>.</exception>
        public void Validate(string memberName)
		{
		    Guard.ArgumentNotNullOrEmptyString(memberName, "memberName");
		    KeyValuePair<InfoDescriptor, List<Rule>> keyValuePair;
		    if (dictionary.TryGetValue(memberName, out keyValuePair))
		    {
		        var infoDescriptor = keyValuePair.Key;
		        var memberValue = infoDescriptor.GetValue(Target);
		        CheckValidMember(infoDescriptor, memberValue);
		    }
		    //TODO: 
		    //throw new ArgumentException(string.Format("A member named '{0}' could not be found containing rules.", memberName), "memberName");
		}

#if (!WindowsCE)
			/// <summary>
		/// Validates the specified member.
		/// </summary>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> to validate. Case sensitive.</param>
		/// <exception cref="ArgumentNullException"><paramref name="expression"/> is a null reference.</exception>
		/// <exception cref="ArgumentException">No <see cref="InfoDescriptor"/> could be found named <paramref name="expression"/>.</exception>
		/// <exception cref="ArgumentException">No <see cref="Rule"/>s could be found on the <see cref="InfoDescriptor"/>,for <paramref name="expression"/>, that have the RuleSet equal to <see cref="MemberValidationManager.RuleSet"/>.</exception>
        public void Validate<TMember>(Expression<Func<TMember>> expression)
			{
			    Validate(TypeExtensions.GetMemberName(expression));
			}

#endif

		#endregion



		/// <summary>
		/// Performs validation when a member is being set.
		/// </summary>
		/// <remarks>
		/// <para>Should be called before the field (representing this member) is set.</para>
		/// </remarks>
		/// <param name="memberName">Member to validate. Case sensitive.</param>
		/// <param name="memberValue">The value of the member being validated.</param>
		/// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
		/// <exception cref="ArgumentNullException"><paramref name="memberName"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="memberName"/> is <see cref="string.Empty"/>.</exception>
		/// <exception cref="ArgumentException">No <see cref="InfoDescriptor"/> could be found named <paramref name="memberName"/>.</exception>
		public void ThrowException(object memberValue, string memberName, object context)
		{
			Guard.ArgumentNotNullOrEmptyString(memberName, "memberName");
		    KeyValuePair<InfoDescriptor, List<Rule>> keyValuePair;
		    if (dictionary.TryGetValue(memberName, out keyValuePair))
		    {
                InternalThrowException(keyValuePair.Key, memberValue, context, true);   
		    }
            //TODO:
            //else
            //{
            //    throw new ArgumentException(string.Format("A member named '{0}' could not be found containing rules.", memberName), "memberName");
            //}
		}
#if (!WindowsCE)
		/// <summary>
		/// Performs validation when a member is being set.
		/// </summary>
		/// <remarks>
		/// <para>Should be called before the field (representing this member) is set.</para>
		/// </para> 
		/// </remarks>
		/// <param name="expression">The <see cref="Expression{TDelegate}"/> to validate.</param>
		/// <param name="memberValue">The value of the member being validated.</param>
		/// <param name="context">An <see cref="object"/> to pass as the context parameter when calling <see cref="Rule.Validate"/>. Use a null for a non value.</param>
		/// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
		/// <exception cref="ArgumentException">No <see cref="InfoDescriptor"/> could be found based on <paramref name="expression"/>.</exception>
		public void ThrowException<TMember>(object memberValue, Expression<Func<TMember>> expression, object context)
		{
			ThrowException(memberValue, TypeExtensions.GetMemberName(expression), context);
		}
#endif

		#endregion
	}
}