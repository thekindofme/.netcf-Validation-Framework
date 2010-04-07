#if (!SILVERLIGHT)
using System.Configuration;
#endif

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// A custom <see cref="ConfigurationElement"/> to hold the urls of mapping documents.
    /// </summary>
    public sealed class MappingDocumentElement : ConfigurationElement
    {

        #region Properties

        /// <summary>
        /// Gets or sets the url for the <see cref="MappingDocumentElement"/>.
        /// </summary>   
        [ConfigurationProperty("url", IsRequired = true, IsKey = true)]
        public string Url
        {
            get
            {
                return (string) base["url"];
            }
            set
            {
                base["url"] = value;
            }
        }

        #endregion

    }
}