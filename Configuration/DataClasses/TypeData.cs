using System;
using System.Xml;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// An object representation of the validation configuration for an overload type.
    /// </summary>
    /// <remarks>Used to identify methods that have overloads.</remarks>
    /// <exclude/>
    /// <seealso cref="ConfigurationService"/>
    public sealed class TypeData
    {

        #region Properties

        /// <summary>
        /// Gets or sets the name of the <see cref="Type"/> that this <see cref="TypeData"/> represents.
        /// </summary>
        public string TypeName
        {
            get;
            set;
        }

        #endregion

        internal static TypeData Read(XmlReader xmlReader)
        {
            var typeData = new TypeData();
            xmlReader.MoveToFirstAttribute();
            do
            {
                if (xmlReader.Name.Length > 0)
                {
                    switch (xmlReader.Name)
                    {
                        case ("typeName"):
                            {
                                typeData.TypeName = xmlReader.Value;
                                break;
                            }
                        default:
                            {
                                throw new Exception(string.Format("Un-expected element while parsing xml. Element name='{0}'", xmlReader.Name));
                            }
                    }
                }
            } while (xmlReader.MoveToNextAttribute());
            return typeData;
        }
    }
}