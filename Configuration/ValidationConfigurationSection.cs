using System;
using System.Configuration;

namespace ValidationFramework.Configuration
{
    //http://msdn2.microsoft.com/en-us/library/2tw134k3.aspx
    //http://blogs.conchango.com/pauloreichert/archive/2005/05/31/1514.aspx
    /// <summary>
    /// Defines a custom <see cref="ConfigurationSection"/> for parsing validation settings. 
    /// </summary>
    public class ValidationConfigurationSection : ConfigurationSection
    {

        #region Properties

        /// <summary>
        /// Gets or sets the name of the <see cref="Type"/> to use when creating <see cref="ConfigurationService.ErrorMessageProvider"/>.
        /// </summary>
        [ConfigurationProperty("errorMessageProvider", IsRequired = false)]
        public ErrorMessageProviderElement ErrorMessageProvider
        {
            get
            {
                return (ErrorMessageProviderElement) this["errorMessageProvider"];
            }
            set
            {
                this["errorMessageProvider"] = value;
            }
        }


        /// <summary>
        /// Gets a list of urls to add as mapping documents
        /// </summary>
        /// <remarks>
        /// For each <see cref="MappingDocumentElement"/> in this collection <see cref="ConfigurationService.AddUrl(string)"/> will be called.
        /// </remarks>
        [ConfigurationProperty("mappingDocuments", IsRequired = false)]
        public MappingDocumentsCollection MappingDocuments
        {
            get
            {
                return (MappingDocumentsCollection) base["mappingDocuments"];
            }
        }

        #endregion

    }
}