using System;
using System.Collections.Generic;
using System.Xml;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// An object representation of the validation configuration for a property.
    /// </summary>
    /// <seealso cref="ConfigurationService"/>
    /// <exclude/>
    public sealed class PropertyData
    {
        /// <inheritdoc/>
        public PropertyData()
        {
            RuleDatas = new List<RuleData>();
        }
        #region Properties

        /// <summary>
        /// Gets or sets a <see cref="List{T}"/> of <see cref="RuleData"/> that represents the <see cref="Rule"/>s for this <see cref="PropertyData"/>.
        /// </summary>
        public List<RuleData> RuleDatas
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        #endregion

        internal static PropertyData Read(XmlReader xmlReader)
        {

            var propertyData = new PropertyData();
            xmlReader.MoveToFirstAttribute();
            do
            {
                if (xmlReader.Name.Length > 0)
                {
                    switch (xmlReader.Name)
                    {
                        case ("name"):
                            {
                                propertyData.Name = xmlReader.Value;
                                break;
                            }
                        default:
                            {
                                throw new Exception(string.Format("Un-expected element while parsing xml. Element name='{0}'", xmlReader.Name));
                            }
                    }
                }
            } while (xmlReader.MoveToNextAttribute());
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    break;
                }
                if (xmlReader.IsStartElement())
                {
                    switch (xmlReader.Name)
                    {
                        case ("rule"):
                            {
                                var ruleData = RuleData.Read(xmlReader);
                                propertyData.RuleDatas.Add(ruleData);
                                break;
                            }
                        default:
                            {
                                throw new Exception(string.Format("Un-expected element while parsing xml. Element name='{0}'", xmlReader.Name));
                            }
                            }
                }
            }
            xmlReader.Read();
            return propertyData;
        }
    }
}