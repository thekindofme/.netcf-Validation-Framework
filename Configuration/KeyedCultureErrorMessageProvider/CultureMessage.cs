using System.Xml.Serialization;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// An object representation of the error message for a culture.
    /// </summary>
    /// <seealso cref="KeyedCultureErrorMessageProvider"/>
    /// <exclude/>
    public class CultureMessage
    {


        #region Properties

        /// <summary>
        /// Gets or sets the culture identifier for this <see cref="CultureMessage"/>.
        /// </summary>
        [XmlAttribute("cultureId")]
        public string CultureId
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the text for this <see cref="CultureMessage"/>.
        /// </summary>
        [XmlText]
        public string Text
        {
            get;
            set;
        }

        #endregion
    }
}