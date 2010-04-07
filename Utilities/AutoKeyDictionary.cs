using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#if (!WindowsCE && !SILVERLIGHT)
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
#endif

namespace ValidationFramework
{
    /// <summary>
    /// Provides the abstract base class for a dictionary that generated its own keys.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the collection.</typeparam>
    /// <typeparam name="TItem">The type of items in the collection.</typeparam>
#if (WindowsCE)
    [Serializable, ComVisible(false)]
    public abstract class AutoKeyDictionary<TKey, TItem> : ICollection<TItem>
#endif
#if (SILVERLIGHT)
    [ComVisible(false)]
    public abstract class AutoKeyDictionary<TKey, TItem> : ICollection<TItem>
#endif
#if (!WindowsCE && !SILVERLIGHT)
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
    public abstract class AutoKeyDictionary<TKey, TItem> : ICollection<TItem>, ISerializable, IDeserializationCallback
#endif
    {
        #region Fields

        private readonly Dictionary<TKey, TItem> dictionary;
      
        #endregion


        #region Constructors

        /// <inheritdoc />
        protected AutoKeyDictionary()
            : this(null, 0)
        {
        }

        /// <param name="comparer">The implementation of the <see cref="IEqualityComparer{T}"/> generic interface to use when comparing keys, or null to use the default equality comparer for the type of the key, obtained from <see cref="EqualityComparer{T}.Default"></see>.</param>
        protected AutoKeyDictionary(IEqualityComparer<TKey> comparer)
            : this(comparer, 0)
        {
        }


        /// <param name="comparer">The implementation of the <see cref="IEqualityComparer{T}"/> generic interface to use when comparing keys, or null to use the default equality comparer for the type of the key, obtained from <see cref="EqualityComparer{T}.Default"/>.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="AutoKeyDictionary{TKey,TItem}"/> can contain.</param>
        protected AutoKeyDictionary(IEqualityComparer<TKey> comparer, int capacity)
        {
            dictionary = new Dictionary<TKey, TItem>(capacity, comparer);
        }

        #endregion


        #region Methods


        #region ICollection<TItem> Members

		/// <inheritdoc />
        public virtual void Add(TItem item)
        {
            Guard.ArgumentNotNull(item, "item");
            ValidateReadOnly();
            dictionary.Add(GetKeyForItem(item), item);
        }


		/// <inheritdoc />
        public void Clear()
        {
            ValidateReadOnly();
            dictionary.Clear();
        }


		/// <inheritdoc />
        public bool Contains(TItem item)
        {
            Guard.ArgumentNotNull(item, "item");
            var key = GetKeyForItem(item);
            return dictionary.ContainsKey(key);
        }


		/// <inheritdoc />
        public void CopyTo(TItem[] array, int arrayIndex)
        {
            dictionary.Values.CopyTo(array, arrayIndex);
        }


		/// <inheritdoc />
        public bool Remove(TItem item)
        {
            Guard.ArgumentNotNull(item, "item");
            ValidateReadOnly();
            var key = GetKeyForItem(item);
            return dictionary.Remove(key);
        }

        #endregion


        #region IEnumerable<TItem> Members
		/// <inheritdoc />
      public  IEnumerator<TItem> GetEnumerator()
        {
            return dictionary.Values.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.Values.GetEnumerator();
        }

        #endregion


        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <returns>The key for the specified element.</returns>
        /// <param name="item">The element from which to extract the key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        protected abstract TKey GetKeyForItem(TItem item);


        private void ValidateReadOnly()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("Dictionary is read-only.");
            }
        }


        internal void InternalAdd(TItem item)
        {
            Guard.ArgumentNotNull(item, "item");
            dictionary.Add(GetKeyForItem(item), item);
        }


        /// <summary>
        /// Determines whether the <see cref="AutoKeyDictionary{TKey,TItem}"/> contains the specified key.
        /// </summary>
        /// <returns>true if the <see cref="AutoKeyDictionary{TKey,TItem}"/> contains an element with the specified key; otherwise, false.</returns>
        /// <param name="key">The key to locate in the <see cref="AutoKeyDictionary{TKey,TItem}"/>.</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="key"/> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }


        /// <summary>
        /// Removes the element with the specified key from the <see cref="AutoKeyDictionary{TKey,TItem}"/>. 
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="AutoKeyDictionary{TKey,TItem}"/> is read-only.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <seealso cref="IsReadOnly"/> 
        /// <seealso cref="SetToReadOnly"/>
        public bool RemoveKey(TKey key)
        {
            ValidateReadOnly();
            return dictionary.Remove(key);
        }


        /// <summary>
        /// Removes the element with the specified key from the <see cref="AutoKeyDictionary{TKey,TItem}"/>. 
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="AutoKeyDictionary{TKey,TItem}"/> is read-only.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <seealso cref="IsReadOnly"/> 
        /// <seealso cref="SetToReadOnly"/>
        internal bool InternalRemove(TKey key)
        {
            return dictionary.Remove(key);
        }


        /// <summary>
        /// Removes the element from the <see cref="AutoKeyDictionary{TKey,TItem}"/>. 
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="AutoKeyDictionary{TKey,TItem}"/> is read-only. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        /// <seealso cref="IsReadOnly"/> 
        /// <seealso cref="SetToReadOnly"/>
        internal bool InternalRemove(TItem item)
        {
            Guard.ArgumentNotNull(item, "item");
            var key = GetKeyForItem(item);
            return dictionary.Remove(key);
        }


        /// <summary>
        /// Set the <see cref="AutoKeyDictionary{TKey,TItem}"/> to be read-only
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public void SetToReadOnly()
        {
            IsReadOnly = true;
        }


        /// <summary>
        /// Gets the item associated with the specified key. 
        /// </summary>
        /// <param name="key">The key of the item to get.</param>
        /// <param name="item">When this method returns, contains the item associated with the specified <paramref name="key"/>, if the key is found; otherwise, the default item for the type of the item parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the <see cref="AutoKeyDictionary{TKey,TItem}"/> contains an element with the specified <paramref name="key"/>; otherwise, false. </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference </exception>
        public bool TryGetValue(TKey key, out TItem item)
        {
            return dictionary.TryGetValue(key, out item);
        }


#if(!WindowsCE && !SILVERLIGHT)
        #region ISerializable, IDeserializationCallback


        #region IDeserializationCallback Members
		/// <inheritdoc />
       public void OnDeserialization(object sender)
        {
            dictionary.OnDeserialization(sender);
        }

        #endregion
        

        #region ISerializable Members

	   /// <inheritdoc />
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            dictionary.GetObjectData(info, context);
        }

        #endregion

        #endregion
#endif


        #endregion


        #region Properties

        /// <summary>Gets the <see cref="IEqualityComparer{T}"/> that is used to determine equality of keys for the dictionary. </summary>
        /// <returns>The <see cref="IEqualityComparer{T}"/> generic interface implementation that is used to determine equality of keys for the current <see cref="AutoKeyDictionary{TKey,TItem}"/> and to provide hash values for the keys.</returns>
        public IEqualityComparer<TKey> Comparer
        {
            get
            {
                return dictionary.Comparer;
            }
        }


        /// <summary>Gets the value associated with the specified key.</summary>
        /// <returns>The value associated with the specified key. If the specified <paramref name="key"/> is not found, a get operation throws a <see cref="KeyNotFoundException"/>, and a set operation creates a new element with the specified key.</returns>
        /// <param name="key">The key of the value to get or set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">An item with the <paramref name="key"/> does not exist in the collection.</exception>
        public TItem this[TKey key]
        {
            get
            {
                return dictionary[key];
            }
        }


        /// <summary>
        /// Gets a collection containing the keys in the <see cref="AutoKeyDictionary{TKey,TItem}"/>. 
        /// </summary>
        /// <remarks>
        /// The order of the keys in the <see cref="Dictionary{TKey,TValue}.KeyCollection"/> is unspecified, but it is the same order as the associated values in the <see cref="Dictionary{TKey,TValue}.ValueCollection"/> returned by the Values property.
        /// The returned <see cref="Dictionary{TKey,TValue}.KeyCollection"/> is not a static copy; instead, the <see cref="Dictionary{TKey,TValue}.KeyCollection"/> refers back to the keys in the original <see cref="AutoKeyDictionary{TKey,TItem}"/>. Therefore, changes to the <see cref="AutoKeyDictionary{TKey,TItem}"/> continue to be reflected in the <see cref="Dictionary{TKey,TValue}.KeyCollection"/>.
        /// Getting the value of this property is an O(1) operation.
        /// </remarks>
        public Dictionary<TKey, TItem>.KeyCollection Keys
        {
            get
            {
                return dictionary.Keys;
            }
        }

        /// <summary>
        /// Gets a collection containing the values in the <see cref="AutoKeyDictionary{TKey,TItem}"/>. 
        /// </summary>
        /// <remarks>
        /// The order of the values in the <see cref="Dictionary{TKey,TValue}.ValueCollection"/> is unspecified, but it is the same order as the associated keys in the <see cref="Dictionary{TKey,TValue}.KeyCollection"/> returned by the Keys property.
        /// The returned <see cref="Dictionary{TKey,TValue}.ValueCollection"/> is not a static copy; instead, the <see cref="Dictionary{TKey,TValue}.ValueCollection"/> refers back to the values in the original <see cref="AutoKeyDictionary{TKey,TItem}"/>. Therefore, changes to the <see cref="AutoKeyDictionary{TKey,TItem}"/> continue to be reflected in the <see cref="Dictionary{TKey,TValue}.ValueCollection"/>.
        /// Getting the value of this property is an O(1) operation.
        /// </remarks>
        public Dictionary<TKey, TItem>.ValueCollection Values
        {
            get
            {
                return dictionary.Values;
            }
        }

		/// <inheritdoc />
        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

		/// <inheritdoc />
        public bool IsReadOnly
        {
			get;
			private set;
        }

        #endregion
    }
}