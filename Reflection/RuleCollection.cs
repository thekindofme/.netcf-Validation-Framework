using System;
using System.Collections;
using System.Collections.Generic;

namespace ValidationFramework.Reflection
{
    /// <summary>
    /// A <see cref="AutoKeyDictionary{TKey,TItem}"/>  of <see cref="Rule"/>s.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public sealed class RuleCollection  : IEnumerable<Rule>
    {

    	private readonly List<Rule> ruleList;
    	private readonly List<Rule> rulesWithoutRuleSetList;
    	private readonly Dictionary<string, IList<Rule>> ruleDictionary;

    	#region Constructors

        /// <summary>
        /// Initialize a new instance of the <see cref="RuleCollection"/> class.
        /// </summary>
        /// <param name="infoDescriptor">The <see cref="InfoDescriptor"/> this <see cref="RuleCollection"/> belongs to.</param>
        internal RuleCollection(InfoDescriptor infoDescriptor)
        {
            InfoDescriptor = infoDescriptor;
			ruleDictionary = new Dictionary<string, IList<Rule>>();
            ruleList = new List<Rule>();
            rulesWithoutRuleSetList = new List<Rule>();
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the <see cref="InfoDescriptor"/> this <see cref="RuleCollection"/> belongs to.
        /// </summary>
        public InfoDescriptor InfoDescriptor
        {
        	get;
        	private set;
        }

        public int Count
        {
            get
            {
                return ruleList.Count;
            }
        }
        #endregion


        #region Methods
		/// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception> 
        /// <exception cref="ArgumentException">An element with the same key already exists in the <see cref="AutoKeyDictionary{TKey,TItem}"/>.</exception>
		public void Add(Rule rule)
		{
			Guard.ArgumentNotNull(rule, "rule");

			CheckForDuplicate(rule);
		    if (rule.RuleSet == null)
		    {
				rulesWithoutRuleSetList.Add(rule);
		    }
		    else
		    {
		        IList<Rule> rules;
				if (!ruleDictionary.TryGetValue(rule.RuleSet, out rules))
		        {
		            rules = new List<Rule>();
					ruleDictionary.Add(rule.RuleSet, rules);
		        }
				rules.Add(rule);
		    }
			ruleList.Add(rule);
		}

        internal void Merge(RuleCollection ruleCollection)
        {
            foreach (var keyValuePair in ruleDictionary)
            {
                var ruleSet = keyValuePair.Key;
                foreach (var rule in keyValuePair.Value)
                {
                    if (!IsDuplicate(rule))
                    {
                        Add(rule);
                    }
                }
            }

            foreach (var rule in ruleCollection.ruleList)
            {
                if (!IsDuplicate(rule))
                {
                    Add(rule);
                }
            }
        }

		internal IList<Rule>  GetRulesForRuleSet(string ruleSet)
		{
			if (ruleSet == null)
			{
				if (ruleList.Count != 0)
				{
                    return new List<Rule>(ruleList);
				}
			}
			else
			{
				ruleSet = ruleSet.ToUpper();
			    IList<Rule> tempRules;
                if (ruleDictionary.TryGetValue(ruleSet, out tempRules))
                {
                    return tempRules;
                }
			}
            return new List<Rule>();
		}

        public string GetRuleSet(Rule rule)
        {
            foreach (var keyValuePair in ruleDictionary)
            {
                foreach (var rule1 in keyValuePair.Value)
                {
                    if (rule1 == rule)
                    {
                        return keyValuePair.Key;
                    }
                }
            }
            foreach (var rule1 in ruleList)
            {
                if (rule1 == rule)
                {
                    return null;
                }
            }
                throw new ArgumentException("Rule not in RuleCollection.");
        }

        /// <inheritdoc />
		public void Remove(Rule rule)
		{
            var ruleSet = GetRuleSet(rule);
            if (ruleSet == null)
            {
                rulesWithoutRuleSetList.Remove(rule);
            }
            else
            {
                ruleDictionary[ruleSet].Remove(rule);
            }
            ruleList.Remove(rule);
		}

		/// <inheritdoc />
		public void Clear()
		{
            rulesWithoutRuleSetList.Clear();
			ruleDictionary.Clear();
			ruleList.Clear();
		}


        private void CheckForDuplicate(Rule rule)
        {
            if (IsDuplicate(rule))
            {
                throw new ArgumentException(string.Format("An equivalent rule already exists in collection. RuleInterpretation : '{0}'", rule.RuleInterpretation), "rule");
            }
         rule.CheckType(InfoDescriptor);
         rule.SetDefaultErrorMessage(InfoDescriptor);
        }



        private bool IsDuplicate(Rule rule)
        {
            //Store temp values for the new ruleSet and Type to save them being accessed for each iteration of the loop.
            var ruleType = rule.GetType();
            IList<Rule> rules;
			if (rule.RuleSet == null)
            {
                rules = rulesWithoutRuleSetList;
            }
            else
            {
				if (!ruleDictionary.TryGetValue(rule.RuleSet, out rules))
                {
                    return false;
                }
            }
            for (var ruleIndex = 0; ruleIndex < rules.Count; ruleIndex++)
            {
                var existingRule = rules[ruleIndex];
                if ((existingRule == rule) || (existingRule.GetType() == ruleType))
                {
                    if (existingRule.IsEquivalent(rule))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        public Rule this[int index]
        {
            get
            {
                return ruleList[index];
            }
        }
        /// <inheritdoc/>
        public IEnumerator<Rule> GetEnumerator()
        {
            return ruleList.GetEnumerator();
        }
        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}