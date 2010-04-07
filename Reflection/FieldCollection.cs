#if (!SILVERLIGHT)
using System;
#endif

namespace ValidationFramework.Reflection
{
    /// <summary>
    /// A <see cref="AutoKeyDictionary{TKey,TItem}"/>  of <see cref="FieldDescriptor"/>s.
	/// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public sealed class FieldCollection : AutoKeyDictionary<string, FieldDescriptor>
    {
        private readonly TypeDescriptor typeDescriptor;


        #region Constructors

        /// <summary>
        /// Initialize a new instance of the <see cref="FieldCollection"/> class.
        /// </summary>
        /// <param name="typeDescriptor">The <see cref="Reflection.TypeDescriptor"/> for this <see cref="FieldCollection"/>.</param>
      internal FieldCollection(TypeDescriptor typeDescriptor)
        {
            this.typeDescriptor = typeDescriptor;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the <see cref="Reflection.TypeDescriptor"/> for this <see cref="FieldCollection"/>.
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
      protected override string GetKeyForItem(FieldDescriptor item)
        {
            Guard.ArgumentNotNull(item, "item");
            return item.Name;
        }

        #endregion
    }
}