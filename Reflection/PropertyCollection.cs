#if (!SILVERLIGHT)
using System;
#endif

namespace ValidationFramework.Reflection
{
    /// <summary>
    /// A <see cref="AutoKeyDictionary{TKey,TItem}"/>  of <see cref="PropertyDescriptor"/>s.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public sealed class PropertyCollection : AutoKeyDictionary<string, PropertyDescriptor>
    {
        private readonly TypeDescriptor typeDescriptor;


        #region Constructors

        /// <summary>
        /// Initialize a new instance of the <see cref="PropertyCollection"/> class.
        /// </summary>
        /// <param name="typeDescriptor">The <see cref="Reflection.TypeDescriptor"/> for this <see cref="PropertyCollection"/>.</param>
        internal PropertyCollection(TypeDescriptor typeDescriptor)
        {
            this.typeDescriptor = typeDescriptor;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the <see cref="Reflection.TypeDescriptor"/> for this <see cref="PropertyCollection"/>.
        /// </summary>
        public TypeDescriptor TypeDescriptor
        {
            get
            {
                return typeDescriptor;
            }
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        protected override string GetKeyForItem(PropertyDescriptor item)
        {
            Guard.ArgumentNotNull(item, "item");
            return item.Name;
        }


        #endregion

    }
}