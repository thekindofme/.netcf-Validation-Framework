using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ValidationFramework.Configuration
{
    //TODO: should this be a struct???
    /// <summary>
    /// An object representation of the validation configuration.
    /// </summary>
    /// <remarks>Will be converted to a <see cref="Rule"/> by the <see cref="Type"/> refined by <see cref="RuleData.TypeName"/>.</remarks>
    /// <seealso cref="ConfigurationService"/>
    public sealed class RuleData
    {
        /// <inheritdoc />
        public RuleData()
        {
            XmlAttributes = new Dictionary<string, string>();
        } 
        #region Properties

        /// <summary>
        /// Gets or sets the name of the <see cref="Type"/> that this <see cref="RuleData"/> represents.
        /// </summary>
        /// <remarks>This <see cref="Type"/> must implement <see cref="IRuleConfigReader"/>.</remarks>
        public string TypeName
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the error message for this <see cref="RuleData"/>.
        /// </summary>
        public string ErrorMessage
        {
            get;
            set;
        }

        public short Severity
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets a <see cref="bool"/> indicating is <see cref="ConfigurationService.ErrorMessageProvider"/> should be used.
        /// </summary>
        public bool UseErrorMessageProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an array of leftover <see cref="XmlAttribute"/>s after xml has been deserialized from xml.
        /// </summary>
        public Dictionary<string,string> XmlAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an array of leftover <see cref="XmlElement"/>s after xml has been deserialized from xml.
        /// </summary>
        public string InnerXml
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rule set for this <see cref="RuleData"/>.
        /// </summary>
        public string RuleSet
        {
            get;
            set;
        }

        #endregion


        #region Methods


		/// <summary>
		/// Attempt to get a value from a <see cref="IDictionary{TKey,TValue}"/>. It the item does not exist <paramref name="defaultValue"/> is returned.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> to try to convert to.</typeparam>
		/// <param name="attributes">The <see cref="IDictionary{TKey,TValue}"/> to extract the value from.</param>
		/// <param name="key">The key to use or the extraction</param>
		/// <param name="defaultValue">The default value if <paramref name="key"/> is not found.</param>
		/// <returns>The value from <paramref name="attributes"/> if <paramref name="key"/> exists; otherwise <paramref name="defaultValue"/>.</returns>
		public static T TryGetValue<T>(IDictionary<string, string> attributes, string key, T defaultValue)
		{
			string stringValue;
			if (attributes.TryGetValue(key, out stringValue))
			{
				return (T) Convert.ChangeType(stringValue, typeof(T), CultureInfo.InvariantCulture);
			}
			else
			{
				return defaultValue;
			}
		}

        internal static RuleData Read(XmlReader xmlReader)
        {
            var ruleData = new RuleData();
            xmlReader.MoveToFirstAttribute();
            do
            {
                if (xmlReader.Name.Length > 0)
                {
                    switch (xmlReader.Name)
                    {
                        case ("ruleSet"):
                            {
                                ruleData.RuleSet = xmlReader.Value;
                                break;
                            }
                        case ("errorMessage"):
                            {
                                ruleData.ErrorMessage = xmlReader.Value;
                                break;
                            }
                        case ("useErrorMessageProvider"):
                            {
                                ruleData.UseErrorMessageProvider = xmlReader.ReadContentAsBoolean();
                                break;
                            }
                        case ("typeName"):
                            {
                                ruleData.TypeName = xmlReader.Value;
                                break;
                            }
                        default:
                            {
                                ruleData.XmlAttributes.Add(xmlReader.Name, xmlReader.Value);
                                break;
                            }
                    }
                }
            } while (xmlReader.MoveToNextAttribute());
            xmlReader.MoveToElement();
            ruleData.InnerXml = xmlReader.ReadInnerXml();
            return ruleData;
        }

        #endregion
    }
}