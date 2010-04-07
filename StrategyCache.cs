using System;
using System.Collections.Generic;

namespace ValidationFramework
{
	/// <summary>
	/// An implementation of the strategy pattern "http://en.wikipedia.org/wiki/Strategy_Pattern.
	/// </summary>
    /// <typeparam name="TStrategy">The type of the strategy.</typeparam>
	/// <typeparam name="TTarget">The type that a strategy will handle.</typeparam>
    public class StrategyCache<TStrategy, TTarget> where TStrategy : IStrategy<TTarget>
	{
		#region Fields

        internal readonly List<TStrategy> displayStrategies;
        internal readonly Dictionary<RuntimeTypeHandle, TStrategy> displayStrategiesHash;

		#endregion

        /// <inheritdoc/>
		public StrategyCache()
        {
            displayStrategies = new List<TStrategy>();
            displayStrategiesHash = new Dictionary<RuntimeTypeHandle, TStrategy>();
        }

	
		/// <summary>
		/// Adds a strategy.
		/// </summary>
		/// <param name="strategy">The strategy to add.</param>
        public void AddStrategy(TStrategy strategy)
		{
			Guard.ArgumentNotNull(strategy, "strategy");
            displayStrategiesHash.Add(strategy.GetType().TypeHandle, strategy);
			displayStrategies.Insert(0, strategy);
		}

		/// <summary>
		/// Removes a strategy that was added using the <see cref="AddStrategy"/>.
		/// </summary>
		/// <param name="strategy">The strategy to remove.</param>
        public void RemoveStrategy(TStrategy strategy)
		{
			Guard.ArgumentNotNull(strategy, "strategy");
			displayStrategiesHash.Remove(strategy.GetType().TypeHandle);
			displayStrategies.Remove(strategy);
		}

		/// <summary>
        /// Find the <typeparamref name="TStrategy"/> to handle <paramref name="target"/>.
		/// </summary>
		/// <param name="target">The target object to find the strategy for.</param>
		/// <returns>An instance of the strategy.</returns>
        public TStrategy FindStrategy(TTarget target)
		{
			for (var i = displayStrategies.Count - 1; i >= 0; i--)
			{
				var errorDisplayStrategy = displayStrategies[i];
                if (errorDisplayStrategy.CanHandleTarget(target))
				{
					return errorDisplayStrategy;
				}
			}
			//TODO: throw exception here??
            return default(TStrategy);
		}

		/// <summary>
        /// Clear all <typeparamref name="TStrategy"/> instances from the <see cref="StrategyCache{TStategy,TTarget}"/>.
		/// </summary>
	    public void Clear()
	    {
	        displayStrategies.Clear();
            displayStrategiesHash.Clear();
	    }

		/// <summary>
        /// Check if a <typeparamref name="TStrategy"/> exists inside the <see cref="StrategyCache{TStrategy,TTarget}"/>.s
		/// </summary>
        /// <param name="strategy">The <typeparamref name="TStrategy"/> to check for.</param>
        /// <returns><see langword="true"/> if <paramref name="strategy"/> exists in the <see cref="StrategyCache{TStrategy,TTarget}"/>; otherwise <see langword="false"/>.</returns>
        public bool Contains(TStrategy strategy)
	    {
            return displayStrategiesHash.ContainsKey(strategy.GetType().TypeHandle);
	    }
	}
}
