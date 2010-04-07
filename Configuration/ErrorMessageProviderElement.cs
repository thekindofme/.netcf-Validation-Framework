using System;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// Represents a <see cref="ConfigurationElement"/> used for deserializing a <see cref="IErrorMessageProvider"/> from a config file.
    /// </summary>
    /// <remarks>
    /// The <see cref="TypeName"/> and the <see cref="InnerXml"/> will, in conjunction with a <see cref="XmlSerializer"/>, to instantiate a <see cref="IErrorMessageProvider"/>.
    /// </remarks>
    public sealed class ErrorMessageProviderElement : ConfigurationElement
    {

        #region Methods

		/// <inheritdoc />
        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            InnerXml = reader.ReadOuterXml();
            return true;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the name of the <see cref="Type"/> that this <see cref="ErrorMessageProviderElement"/> will deserialize.
        /// </summary>
        [ConfigurationProperty("typeName", IsRequired = true)]
        public string TypeName
        {
            get
            {
                return (string) base["typeName"];
            }
            set
            {
                base["typeName"] = value;
            }
        }


        /// <summary>
        /// Gets or set the inner XML for this <see cref="ErrorMessageProviderElement"/>.
        /// </summary>
        public string InnerXml
        {
            get;
            set;
        }

        #endregion
    }
}