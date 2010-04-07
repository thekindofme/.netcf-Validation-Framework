using System;
using System.Collections.Generic;
using System.Xml;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// An object representation of the validation configuration for a group classes.
    /// </summary>
    /// <seealso cref="ConfigurationService"/>
    /// <exclude/>
    public sealed class ValidationMappingData
    {
        /// <inheritdoc/>
        public ValidationMappingData()
        {
            ClassDatas = new List<ClassData>();
        }
        #region Properties

        /// <summary>
        /// Gets or sets a <see cref="List{T}"/> of <see cref="ClassData"/>s that represent the classes to be validate.
        /// </summary>
        public List<ClassData> ClassDatas
        {
            get;
            set;
        }

        #endregion

        internal static ValidationMappingData Read(XmlReader xmlReader)
        {
            var validationMappingData = new ValidationMappingData();
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if (xmlReader.Name == "class")
                    {
                        var classData = ClassData.Read(xmlReader);
                        validationMappingData.ClassDatas.Add(classData);
                    }
                    else
                    {
                        throw new Exception(string.Format("Un-expected element while parsing xml. Element name='{0}'", xmlReader.Name));
                            }
                }
            }
            return validationMappingData;
        }
    }
}