using System;
using System.Collections.Generic;
using System.Xml;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// An object representation of the validation configuration for a parameter.
    /// </summary>
    /// <seealso cref="ConfigurationService"/>
    /// <exclude/>
    public sealed class ParameterData
    {
        /// <inheritdoc/>
        public ParameterData()
        {
            RuleDatas = new List<RuleData>();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a <see cref="List{T}"/> of <see cref="RuleData"/> that represents the <see cref="Rule"/>s for this <see cref="ParameterData"/>.
        /// </summary>
        public List<RuleData> RuleDatas
        {
            get;
            set;
        }

        #endregion

        internal static ParameterData Read(XmlReader xmlReader)
        {
            var parameterData = new ParameterData();
            xmlReader.MoveToFirstAttribute();
            do
            {
                if (xmlReader.Name.Length > 0)
                {
                    switch (xmlReader.Name)
                    {
                        case ("name"):
                            {
                                parameterData.Name = xmlReader.Value;
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
                                parameterData.RuleDatas.Add(ruleData);
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
            return parameterData;
        }
    }
}