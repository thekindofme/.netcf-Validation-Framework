using System;
using System.Collections.Generic;
using System.Xml;
using ValidationFramework.Reflection;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// An object representation of the validation configuration for a class.
    /// </summary>
    /// <exclude/>
    /// <seealso cref="ConfigurationService"/>
    public sealed class ClassData
    {
        /// <inheritdoc/>
        public ClassData()
        {
            Properties = new List<PropertyData>();
            Fields= new List<FieldData>();
            Methods= new List<MethodData>();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the name of the <see cref="Type"/> that this <see cref="ClassData"/> represents.
        /// </summary>
        public string TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an array of <see cref="PropertyData"/>s that represent the properties to be validate.
        /// </summary>
        /// <remarks>The <see cref="PropertyData"/>s will be converted to <see cref="PropertyDescriptor"/>s.</remarks>
        public List<PropertyData> Properties
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an array of <see cref="FieldData"/>s that represent the properties to be validate.
        /// </summary>
        /// <remarks>The <see cref="FieldData"/>s will be converted to <see cref="FieldDescriptor"/>s.</remarks>
        public List<FieldData> Fields
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an array of <see cref="MethodData"/>s that represent the methods to be validate.
        /// </summary>
        /// <remarks>The <see cref="MethodData"/>s will be converted to <see cref="MethodDescriptor"/>s.</remarks>
        public List<MethodData> Methods
        {
            get;
            set;
        }

        #endregion

        internal static ClassData Read(XmlReader xmlReader)
        {
            var classData = new ClassData();
            xmlReader.MoveToFirstAttribute();
            do
            {
                if (xmlReader.Name.Length > 0)
                {
                    switch (xmlReader.Name)
                    {
                        case ("typeName"):
                            {
                                classData.TypeName = xmlReader.Value;
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
                    case("method"):
                        {
                            var methodData = MethodData.Read(xmlReader);
                            classData.Methods.Add(methodData);
                            break;
                        }
                    case("property"):
                        {
                            var propertyData = PropertyData.Read(xmlReader);
                            classData.Properties.Add(propertyData);
                            break;
                        }
                    case("field"):
                        {
                            var fieldData = FieldData.Read(xmlReader);
                            classData.Fields.Add(fieldData);
                            break;
                        }
                    default:
                        {
                            throw new Exception(string.Format("Un-expected element while parsing xml. Element name='{0}'", xmlReader.Name));
                            }

                }}
            }
            xmlReader.Read();
                return classData;
        }
    }
}