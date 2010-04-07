using System;

namespace ValidationFramework.Reflection
{
    /// <summary>
    /// A <see cref="AutoKeyDictionary{TKey,TItem}"/> of <see cref="ParameterDescriptor"/>s.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public sealed class ParameterCollection : AutoKeyDictionary<string, ParameterDescriptor>
    {
      
        #region Constructors

        /// <summary>
        /// Initialize a new instance of the <see cref="ParameterCollection"/> class.
        /// </summary>
        /// <param name="methodDescriptor">The <see cref="Reflection.MethodDescriptor"/> for this <see cref="ParameterCollection"/>.</param>
        internal ParameterCollection(MethodDescriptor methodDescriptor)
        {
            MethodDescriptor = methodDescriptor;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the <see cref="Reflection.MethodDescriptor"/> for this <see cref="ParameterCollection"/>.
        /// </summary>
        public MethodDescriptor MethodDescriptor
        {
        	get;
        	private set;
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        protected override string GetKeyForItem(ParameterDescriptor item)
        {
            Guard.ArgumentNotNull(item, "item");
            return item.Name;
        }

        /// <summary>
        /// Gets the <see cref="ParameterDescriptor"/> at the specified index.
        /// </summary>
        /// <param name="position">The zero-based position of the <see cref="ParameterDescriptor"/> to get.</param>
        /// <returns>The <see cref="ParameterDescriptor"/> at the specified position.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="position"/> does not exists in this <see cref="ParameterCollection"/>.</exception>
        public ParameterDescriptor this[int position]
        {
            get
            {
                foreach (var parameterDescriptor in this)
                {
                    if (parameterDescriptor.Position == position)
                    {
                        return parameterDescriptor;
                    }
                }
                throw new InvalidOperationException("Position does not exist.");
            }
        }
        #endregion
    }
}