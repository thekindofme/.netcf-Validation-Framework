using System.Configuration;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A custom <see cref="ConfigurationElementCollection"/> to contain <see cref="MappingDocumentElement"/>s.
    /// </summary>
    public sealed class MappingDocumentsCollection : ConfigurationElementCollection
    {
        #region Methods

		/// <inheritdoc />
        protected override ConfigurationElement CreateNewElement()
        {
            return new MappingDocumentElement();
        }


		/// <inheritdoc />
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MappingDocumentElement) element).Url;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Get the <see cref="MappingDocumentElement"/> at the specified index location.
        /// </summary>
        /// <param name="index">The index location of the <see cref="MappingDocumentElement"/> to return.</param>
        /// <returns>The <see cref="MappingDocumentElement"/> at the specified index.</returns>
        public MappingDocumentElement this[int index]
        {
            get
            {
                return (MappingDocumentElement) BaseGet(index);
            }
        }


		/// <inheritdoc />
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }


		/// <inheritdoc />
        protected override string ElementName
        {
            get
            {
                return "mappingDocument";
            }
        }

        #endregion
    }
}