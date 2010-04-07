using System;
using System.Collections.Generic;
using System.Xml;
using ValidationFramework.Reflection;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// An object representation of the validation configuration for a method.
    /// </summary>
    /// <exclude/>
    /// <seealso cref="ConfigurationService"/>
    public sealed class MethodData
    {

        public MethodData()
        {
            Parameters = new List<ParameterData>();
            OverloadTypes = new List<TypeData>();

        }
        #region Properties

        /// <summary>
        /// Gets or sets name of the method this <see cref="MethodData"/> represents.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a <see cref="List{T}"/> of <see cref="ParameterData"/>s that represent the parameters to be validate.
        /// </summary>
        /// <remarks>The <see cref="ParameterData"/>s will be converted to <see cref="ParameterDescriptor"/>s.</remarks>
        public List<ParameterData> Parameters
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets a <see cref="List{T}"/> of <see cref="TypeData"/>s that are used to uniquely identify overloads of a method.
        /// </summary>
        public List<TypeData> OverloadTypes
        {
            get;
            set;
        }

        #endregion

        internal static MethodData Read(XmlReader xmlReader)
        {
            var methodData = new MethodData();
            xmlReader.MoveToFirstAttribute();
            do
            {
                if (xmlReader.Name.Length > 0)
                {
                    switch (xmlReader.Name)
                    {
                        case ("name"):
                            {
                                methodData.Name = xmlReader.Value;
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
                        case ("overloadType"):
                            {
                                var typeData = TypeData.Read(xmlReader);
                                methodData.OverloadTypes.Add(typeData);
                                break;
                            }
                        case ("parameter"):
                            {
                                var parameterData = ParameterData.Read(xmlReader);
                                methodData.Parameters.Add(parameterData);
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
            return methodData;
        }
    }
}