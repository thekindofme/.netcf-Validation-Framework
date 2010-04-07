using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// An <see cref="IErrorMessageProvider"/> that uses the current culture to determine error messages.
    /// </summary>
    [XmlRoot("keyedCultureErrorMessageProvider")]
    public class KeyedCultureErrorMessageProvider : IErrorMessageProvider
    {
        #region Fields

        private Dictionary<string, MessageGroup> messageDictionary;
        private MessageGroup[] messageGroups;

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the array of <see cref="MessageGroup"/>s for the <see cref="KeyedCultureErrorMessageProvider"/>.
        /// </summary>
        [XmlElement("messageGroup", Type = typeof (MessageGroup))]
        public MessageGroup[] MessageGroups
        {
            get
            {
                return messageGroups;
            }
            set
            {
                messageGroups = value;
                messageDictionary = new Dictionary<string, MessageGroup>(StringComparer.OrdinalIgnoreCase);
                for (var index = 0; index < messageGroups.Length; index++)
                {
                    var messageGroup = messageGroups[index];
                    messageDictionary.Add(messageGroup.Key, messageGroup);
                }
            }
        }

        #endregion


        #region Methods

		/// <inheritdoc />
        public string RetrieveErrorMessage(Rule rule, object targetObjectValue, object targetMemberValue, object context)
        {
            var cultureInfo = CultureInfo.CurrentCulture;

            MessageGroup messageGroup;
            if (messageDictionary.TryGetValue(rule.ErrorMessage, out messageGroup))
            {
                CultureMessage currentCultureMessage;
                CultureMessage twoLetterISOLanguageName;
                if (messageGroup.CultureDictionary.TryGetValue(cultureInfo.TwoLetterISOLanguageName, out twoLetterISOLanguageName))
                {
                    return twoLetterISOLanguageName.Text;
                }
                else if (messageGroup.CultureDictionary.TryGetValue(cultureInfo.Name, out currentCultureMessage))
                {
                    return currentCultureMessage.Text;
                }
            }
            return null;
        }

        #endregion
    }
}