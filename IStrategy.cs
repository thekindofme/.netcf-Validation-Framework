namespace ValidationFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTarget">The type hat the <see cref="IStrategy{TTarget}"/> will act upon.</typeparam>
	public interface IStrategy<TTarget>
	{
        /// <summary>
        /// Check if the <see cref="IStrategy{TTarget}"/> can handle a target.
        /// </summary>
        /// <param name="target">The target to check.</param>
        /// <returns><see langword="true"/> is the <see cref="IStrategy{TTarget}"/> can handle <paramref name="target"/>; otherwise <see langword="false"/>.</returns>
		bool CanHandleTarget(TTarget target);
	}
}