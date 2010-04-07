using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// An object representation of the how cultures map to an error message.
    /// </summary>
    /// <seealso cref="KeyedCultureErrorMessageProvider"/>
    /// <exclude/>
    public class MessageGroup
    {
        #region Fields

        private Dictionary<string, CultureMessage> cultureDictionary;
        private CultureMessage[] cultureMessages;

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the key for this <see cref="MessageGroup"/>.
        /// </summary>
        [XmlAttribute("key")]
        public string Key
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets an array of <see cref="CultureMessage"/>s for this <see cref="MessageGroup"/>.
        /// </summary>
        [XmlElement("cultureMessage", Type = typeof (CultureMessage))]
        public CultureMessage[] CultureMessages
        {
            get
            {
                return cultureMessages;
            }
            set
            {
                cultureMessages = value;
                cultureDictionary = new Dictionary<string, CultureMessage>(StringComparer.OrdinalIgnoreCase);
                for (var index = 0; index < cultureMessages.Length; index++)
                {
                    var cultureMessage = cultureMessages[index];
                    CultureDictionary.Add(cultureMessage.CultureId, cultureMessage);
                }
            }
        }


        internal Dictionary<string, CultureMessage> CultureDictionary
        {
            get
            {
                return cultureDictionary;
            }
        }

        #endregion
    }
}