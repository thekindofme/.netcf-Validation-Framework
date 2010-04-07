using System;
using System.Collections.Generic;
#if (!WindowsCE && !SILVERLIGHT)
using System.Configuration;
using System.Xml.Serialization;
#endif
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;
using ValidationFramework.Extensions;
using ValidationFramework.Reflection;

namespace ValidationFramework.Configuration
{
    /// <summary>
    /// Allow the addition of <see cref="Rule"/>s via xml.
    /// </summary>
    /// <example>
    /// <code source="Examples\ExampleLibraryCSharp\Configuration\ConfigurationServiceExample.cs" title="The following example shows how to programmatically add Rules via an embedded resource." lang="cs"/>
    /// <code source="Examples\ExampleLibraryCSharp\Configuration\Person.validation.xml" title="This C# example assumes there is an embedded resource, named 'ValidatableClass.validation.xml' in the current Assembly, containing the following xml." lang="xml"/>
    /// <code source="Examples\ExampleLibraryVB\Configuration\ConfigurationServiceExample.vb" title="The following example shows how to programmatically add Rules via an embedded resource." lang="vbnet"/>
    /// <code source="Examples\ExampleLibraryVB\Configuration\Person.validation.xml" title="This VB example assumes there is an embedded resource, named 'ValidatableClass.validation.xml' in the current Assembly, containing the following xml." lang="xml"/>
    /// </example>
    public static class ConfigurationService
    {
        #region Fields
		
		private static readonly object initializedLock = new object();
        private static bool initialized;

        #endregion



        #region Methods
#if (!WindowsCE && !SILVERLIGHT)
        /// <summary>
        /// Initializes the <see cref="ConfigurationService"/>.
        /// </summary>
        /// <remarks>
		/// Retrieves <see cref="ValidationConfigurationSection"/>from the current application's default configuration. 
		/// For that <see cref="ValidationConfigurationSection"/> it does the following.
		/// <list type="bullet">
		/// <item>
		/// Configures <see cref="ErrorMessageProvider"/> base on <see cref="ValidationConfigurationSection.ErrorMessageProvider"/>.
		/// </item>
		/// <item>
		/// Goes through each <see cref="ValidationConfigurationSection.MappingDocuments"/> and calls <see cref="AddUrl(string)"/> for each <see cref="MappingDocumentElement.Url"/>.
		/// </item>
		/// </list>
		/// Calling this only performs these action on the first call. Each successive call will be ignored.
		/// If this method is called simultaneous on two different threads one thread will obtain a lock and the other thread will have to wait. 
		/// </remarks>
		/// <exception cref="ArgumentNullException">Any <see cref="MappingDocumentElement.Url"/> is null.</exception>
		/// <exception cref="ArgumentException">Any <see cref="MappingDocumentElement.Url"/>  is a <see cref="string.Empty"/>.</exception>
		/// <exception cref="FileNotFoundException">Any <see cref="MappingDocumentElement.Url"/>  cannot be found.</exception>
		/// <exception cref="WebException">The remote filename, defined by Any <see cref="MappingDocumentElement.Url"/>, cannot be resolved.-or-An error occurred while processing the request.</exception>
		/// <exception cref="DirectoryNotFoundException">Part of the filename or directory cannot be found.</exception>
		/// <exception cref="UriFormatException">Any <see cref="MappingDocumentElement.Url"/> is not a valid URI.</exception>
		public static void Initialize()
        {
        	if (!initialized)
        	{
        		lock (initializedLock)
        		{
        			if (!initialized)
        			{
        				var section = ConfigurationManager.GetSection("validationFrameworkConfiguration");
        				if (section != null)
        				{
        					var validationConfigurationSection = (ValidationConfigurationSection) section;
        					var errorMessageProviderElement = validationConfigurationSection.ErrorMessageProvider;
        					if (errorMessageProviderElement != null)
        					{
        						var type = Type.GetType(errorMessageProviderElement.TypeName, true);
        						var errorMessageProviderXmlSerializer = new XmlSerializer(type);

        						ErrorMessageProvider = (IErrorMessageProvider) errorMessageProviderXmlSerializer.Deserialize(new StringReader(errorMessageProviderElement.InnerXml));
        					}

        					if (validationConfigurationSection.MappingDocuments != null)
        					{
        						foreach (MappingDocumentElement mappingDocument in validationConfigurationSection.MappingDocuments)
        						{
        							AddUrl(mappingDocument.Url);
        						}
        					}
        				}
        				initialized = true;
        			}
        		}
        	}
        }

#endif
    	/// <summary>
        /// Add validation from a particular XML file.
        /// </summary>
        /// <param name="xmlFileInfo">The <see cref="FileInfo"/> to the XML data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="xmlFileInfo"/> is null.</exception>
        /// <exception cref="FileNotFoundException">The file represented by <paramref name="xmlFileInfo"/> cannot be found.</exception>
        /// <exception cref="WebException">The remote filename cannot be resolved.-or-An error occurred while processing the request.</exception>
        /// <exception cref="DirectoryNotFoundException">Part of the filename or directory cannot be found.</exception>
        public static void AddXmlFile(FileInfo xmlFileInfo)
        {
            Guard.ArgumentNotNull(xmlFileInfo, "xmlFileInfo");
            AddXmlFile(xmlFileInfo.FullName);
        }


        /// <summary>
        /// Add validation from a particular XML file.
        /// </summary>
        /// <param name="xmlFileUrl">The URL for the file containing the XML data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="xmlFileUrl"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="xmlFileUrl"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="FileNotFoundException"><paramref name="xmlFileUrl"/> cannot be found.</exception>
        /// <exception cref="WebException">The remote filename cannot be resolved.-or-An error occurred while processing the request.</exception>
        /// <exception cref="DirectoryNotFoundException">Part of the filename or directory cannot be found.</exception>
        /// <exception cref="UriFormatException"><paramref name="xmlFileUrl"/> is not a valid URI.</exception>
        public static void AddXmlFile(string xmlFileUrl)
        {
            Guard.ArgumentNotNullOrEmptyString(xmlFileUrl, "xmlFileUrl");

            using (var textReader = XmlReader.Create(xmlFileUrl))
            {
                AddXmlReader(textReader);
            }
        }


        /// <summary>
        /// Add all validation definitions from a directory tree.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Assume that any file named <c>*.validation.xml</c> is a validation definition document.
        /// </para>
        /// <para>
        /// This method is recursive.
        /// </para>
        /// </remarks>
        /// <param name="directoryInfo">a directory</param>
        /// <exception cref="ArgumentNullException"><paramref name="directoryInfo"/> is null.</exception>
        /// <exception cref="DirectoryNotFoundException">The path encapsulated in <paramref name="directoryInfo"/> is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="FileNotFoundException">A file in one of the directories cannot be found.</exception>
        /// <exception cref="WebException">The remote filename cannot be resolved.-or-An error occurred while processing the request.</exception>
        /// <exception cref="DirectoryNotFoundException">Part of the filename or directory cannot be found.</exception>
        public static void AddDirectory(DirectoryInfo directoryInfo)
        {
            Guard.ArgumentNotNull(directoryInfo, "directoryInfo");
            var directoryInfos = directoryInfo.GetDirectories();
            for (var directoryIndex = 0; directoryIndex < directoryInfos.Length; directoryIndex++)
            {
                var subDirectory = directoryInfos[directoryIndex];
                AddDirectory(subDirectory);
            }

            var fileInfos = directoryInfo.GetFiles("*.validation.xml");
            for (var fileIndex = 0; fileIndex < fileInfos.Length; fileIndex++)
            {
                var validationFileInfo = fileInfos[fileIndex];
                AddXmlFile(validationFileInfo);
            }
        }


        /// <summary>
        /// Add validation definitions from a <c>string</c>
        /// </summary>
        /// <param name="xml">The <see cref="string"/> containing the XML data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="xml"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="xml"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="XmlException"><paramref name="xml"/> is not a valid <see cref="XmlNodeType.Document"/>.</exception>
        public static void AddXmlString(string xml)
        {
            Guard.ArgumentNotNullOrEmptyString(xml, "xml");
            using (var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings(), (XmlParserContext)null))
            {
                // make a StringReader for the string passed in - the StringReader
                // inherits from TextReader.  We can use the XmlTextReader.ctor that
                // takes the TextReader to build from a string...
                AddXmlReader(reader);
            }
        }


        /// <summary>
        /// Adds the validation in the <see cref="XmlTextReader"/> after validating it against the validationFramework-validationDefinition-1.5 schema.
        /// </summary>
        /// <param name="xmlReader">The XmlTextReader that contains the validation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="xmlReader"/> is null.</exception>
        public static void AddXmlReader(XmlReader xmlReader)
        {
            Guard.ArgumentNotNull(xmlReader, "xmlReader");
            xmlReader.MoveToContent();
          var validationMappingData = ValidationMappingData.Read(xmlReader);
            var datas = validationMappingData.ClassDatas;
            for (var classDataIndex = 0; classDataIndex < datas.Count; classDataIndex++)
            {
                var classData = datas[classDataIndex];
                var classType = Type.GetType(classData.TypeName, true);
                ProcessProperties(classData, classType);
                ProcessFields(classData, classType);
                ProcessMethods(classData, classType);
            }
        }




        /// <summary>
        /// Read validation definitions from a URL.
        /// </summary>
        /// <param name="url">a URL</param>
        /// <exception cref="ArgumentNullException"><paramref name="url"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="url"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="FileNotFoundException"><paramref name="url"/> cannot be found.</exception>
        /// <exception cref="WebException">The remote filename cannot be resolved.-or-An error occurred while processing the request.</exception>
        /// <exception cref="DirectoryNotFoundException">Part of the filename or directory cannot be found.</exception>
        /// <exception cref="UriFormatException"><paramref name="url"/> is not a valid URI.</exception>
        public static void AddUrl(string url)
        {
            Guard.ArgumentNotNullOrEmptyString(url, "url");
            AddXmlFile(url);
        }


        /// <summary>
        /// Read validation definitions from a <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">a <see cref="Uri" /> to read the mappings from.</param>
        /// <returns>This configuration object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="uri"/> is null.</exception>
        public static void AddUrl(Uri uri)
        {
            Guard.ArgumentNotNull(uri, "uri");
			if (uri.IsFile())
			{
				AddUrl(uri.OriginalString);
			}
			else
			{
				AddUrl(uri.AbsolutePath);
			}
        }

#if (!SILVERLIGHT)
        /// <summary>
        /// Read validation definitions from an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="xmlDocument">A loaded <see cref="XmlDocument"/> that contains the validation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="xmlDocument"/> is null.</exception>
        public static void AddDocument(XmlDocument xmlDocument)
        {
            Guard.ArgumentNotNull(xmlDocument, "xmlDocument");
            using (var memoryStream = new MemoryStream())
            {
                xmlDocument.Save(memoryStream);
                memoryStream.Position = 0;
                AddInputStream(memoryStream);
            }
        }
#endif

     


        private static void ProcessMethods(ClassData classData, Type classType)
        {
            if ((classData.Methods != null) && (classData.Methods.Count > 0))
            {
                for (var methodIndex = 0; methodIndex < classData.Methods.Count; methodIndex++)
                {
                    var methodData = classData.Methods[methodIndex];
                    MethodInfo methodInfo;
                    if (methodData.OverloadTypes.Count == 0)
                    {
                        methodInfo = classType.GetMethod(methodData.Name);
                    }
                    else
                    {
                        var types = new Type[methodData.OverloadTypes.Count];
                        for (var overloadTypeIndex = 0; overloadTypeIndex < methodData.OverloadTypes.Count; overloadTypeIndex++)
                        {
                            var typeData = methodData.OverloadTypes[overloadTypeIndex];
                            types[overloadTypeIndex] = Type.GetType(typeData.TypeName, true);
                        }
                        methodInfo = classType.GetMethod(methodData.Name, types);
                    }
                    var methodDescriptor = MethodCache.GetMethod(methodInfo.MethodHandle);
                    for (var parameterIndex = 0; parameterIndex < methodData.Parameters.Count; parameterIndex++)
                    {
                        var parameterData = methodData.Parameters[parameterIndex];
                        var parameterDescriptor = methodDescriptor.Parameters[parameterData.Name];
                        AddRuleToInfoDescriptor(parameterData.RuleDatas, parameterDescriptor);
                    }
                }
            }
        }


        private static void ProcessProperties(ClassData classData, Type classType)
        {
            if ((classData.Properties != null) && (classData.Properties.Count > 0))
            {
                var runtimeTypeHandle = classType.TypeHandle;
                var typeDescriptor = TypeCache.GetType(runtimeTypeHandle);
                for (var propertyIndex = 0; propertyIndex < classData.Properties.Count; propertyIndex++)
                {
                    var propertyData = classData.Properties[propertyIndex];
                    PropertyDescriptor propertyDescriptor;
                    if (!typeDescriptor.Properties.TryGetValue(propertyData.Name, out propertyDescriptor))
                    {
                        propertyDescriptor = typeDescriptor.GetOrCreatePropertyDescriptor(propertyData.Name);
                    }
                    AddRuleToInfoDescriptor(propertyData.RuleDatas, propertyDescriptor);
                }
            }
        }

        private static void ProcessFields(ClassData classData, Type classType)
        {
            if ((classData.Fields != null) && (classData.Fields.Count > 0))
            {
                var runtimeTypeHandle = classType.TypeHandle;
                var typeDescriptor = TypeCache.GetType(runtimeTypeHandle);
                for (var fieldIndex = 0; fieldIndex < classData.Fields.Count; fieldIndex++)
                {
					var fieldData = classData.Fields[fieldIndex];
                    FieldDescriptor fieldDescriptor;
                    if (!typeDescriptor.Fields.TryGetValue(fieldData.Name, out fieldDescriptor))
                    {
                        fieldDescriptor = typeDescriptor.GetOrCreateFieldDescriptor(fieldData.Name);
                    }
                    AddRuleToInfoDescriptor(fieldData.RuleDatas, fieldDescriptor);
                }
            }
        }


        private static void AddRuleToInfoDescriptor(IList<RuleData> list, InfoDescriptor infoDescriptor)
        {
            for (var ruleDateIndex = 0; ruleDateIndex < list.Count; ruleDateIndex++)
            {
                var ruleData = list[ruleDateIndex];
                var runtimeTypeHandle = infoDescriptor.RuntimeTypeHandle;
                var rule = GetRule(ruleData, runtimeTypeHandle);
                infoDescriptor.Rules.Add(rule);
            }
        }


        internal static Rule GetRule(RuleData ruleData, RuntimeTypeHandle runtimeTypeHandle)
        {
            Type ruleConfigReaderType;

            var typeName = ruleData.TypeName;
            var containsCommas = ruleData.TypeName.Contains(",");

            if (containsCommas)
            {
					Type type;
				try
				{
					type = Type.GetType(typeName, true);
				}
				catch(Exception e)
				{
					throw new ArgumentException(string.Format("Could not load IRuleConfigReader type. Tried '{0}'.", typeName), e);
				}
            	//is it a IRuleConfigReader
                if (TypePointers.IRuleConfigReaderType.IsAssignableFrom(type))
                {
                    ruleConfigReaderType = type;
                }
                else //not a IRuleConfigReader so assume it is a Rule and try to derive. 
                {
                    var assemblyName = type.Assembly.FullName;
                    var namespaceName = type.Namespace;

                    //Assumes ConfigReader is of the form {RuleName}ConfigReader in the same assembly and namespace
                    var configReaderTypeName = string.Format("{0}.{1}ConfigReader,{2}", namespaceName, type.Name, assemblyName);
                    try
                    {
                        ruleConfigReaderType = Type.GetType(configReaderTypeName, true);
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException(string.Format("Could not load IRuleConfigReader type. Tried '{0}'.", configReaderTypeName), e);
                    }
                }
            }
            else
            {
                string internalTypeName;
                if (typeName.Contains("ConfigReader"))
                {
                    internalTypeName = string.Format("ValidationFramework.Configuration.{0},ValidationFramework", typeName);
                }
                else
                {
                    internalTypeName = string.Format("ValidationFramework.Configuration.{0}ConfigReader,ValidationFramework", typeName);
                }

                try
                {
                    ruleConfigReaderType = Type.GetType(internalTypeName, true);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(string.Format("Could not load IRuleConfigReader type. Tried '{0}'.", internalTypeName), e);
                }
            }
            var ruleConfigReader = (IRuleConfigReader)Activator.CreateInstance(ruleConfigReaderType);
            return ruleConfigReader.ReadConfig(ruleData, runtimeTypeHandle);
        }


        /// <summary>
        /// Add validation definition from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="xmlInputStream">The stream containing XML</param>
        /// <remarks>
        /// The <see cref="Stream"/> passed in through the parameter <c>xmlInputStream</c> is not <b>guaranteed</b> to be cleaned up by this method.  It is the callers responsibility to ensure that the <c>xmlInputStream</c> is properly handled when this method completes.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="xmlInputStream"/> is null.</exception>
        public static void AddInputStream(Stream xmlInputStream)
        {
            Guard.ArgumentNotNull(xmlInputStream, "xmlInputStream");
            using (var textReader = XmlReader.Create(xmlInputStream))
            {
                AddXmlReader(textReader);
            }
        }


        /// <summary>
        /// Adds the validation definitions in the Resource of the <see cref="Assembly"/>.
        /// </summary>
        /// <param name="path">The path to the Resource file in the <see cref="Assembly"/></param>
        /// <param name="assembly">The <see cref="Assembly"/> that contains the Resource file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="FileLoadException"><paramref name="path"/> could not be loaded.</exception>
        /// <exception cref="FileNotFoundException"><paramref name="path"/> was not found.</exception>
        /// <exception cref="BadImageFormatException"><paramref name="assembly"/> is not a valid assembly.</exception>
        public static void AddResource(string path, Assembly assembly)
        {
            Guard.ArgumentNotNullOrEmptyString(path, "path");
            Guard.ArgumentNotNull(assembly, "assembly");
            using (var resourceStream = assembly.GetManifestResourceStream(path))
            {

                AddInputStream(resourceStream);
            }
        }


        /// <summary>
        /// Add validation definitions a specific <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to map.</param>
        /// <returns>This configuration object.</returns>
        /// <remarks>
        /// <para>If <paramref name="type"/> has a full name of <c>MyNameSpace.MyClass</c> the resource named <c>MyNameSpace.MyClass.validation.xml</c>, embedded in the class' assembly, will be added.</para>
        /// <para>If the mappings and classes are defined in different assemblies or don't follow the naming convention, then this method cannot be used.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="FileLoadException">The resource for <paramref name="type"/> could not be loaded.</exception>
        /// <exception cref="FileNotFoundException">The resource for <paramref name="type"/> was not found.</exception>
        public static void AddClass(Type type)
        {
            Guard.ArgumentNotNull(type, "type");
            AddResource(string.Format("{0}.validation.xml", type.FullName), type.Assembly);
        }


        /// <summary>
        /// Adds all of the Assembly's Resource files that end with "<c>.validation.xml</c>"
        /// </summary>
        /// <param name="assemblyName">The name of the <see cref="Assembly"/> to load.</param>
        /// <remarks>
        /// <para>The <see cref="Assembly"/> must be in the local bin, probing path, or GAC so that the <see cref="Assembly"/> can be loaded by name.  
        /// If these conditions are not satisfied then your code should load the <see cref="Assembly"/> and call the override <see cref="AddAssembly(Assembly)"/> instead.</para> 
        /// <para>This method is case sensitive, hence the resource files must end with lower case "<c>.validation.xml</c>".</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="assemblyName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="assemblyName"/> is a <see cref="string.Empty"/>.</exception>
        /// <exception cref="FileLoadException"><paramref name="assemblyName"/> was found but could not be loaded.</exception>
        /// <exception cref="FileNotFoundException"><paramref name="assemblyName"/> is not found.</exception>
        /// <exception cref="BadImageFormatException"><paramref name="assemblyName"/> is not a valid assembly.</exception>
        public static void AddAssembly(string assemblyName)
        {
            Guard.ArgumentNotNullOrEmptyString(assemblyName, "assemblyName");
            AddAssembly(Assembly.Load(assemblyName));
        }


        /// <summary>
        /// Adds all of the <paramref name="assembly"/> resource files that end with "<c>.validation.xml</c>".
        /// </summary>
        /// <remarks>This method is case sensitive, hence the resource files must end with lower case "<c>.validation.xml</c>".</remarks>
        /// <param name="assembly">The <see cref="Assembly"/> to load.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is null.</exception>
        public static void AddAssembly(Assembly assembly)
        {
            Guard.ArgumentNotNull(assembly, "assembly");
            var resources = assembly.GetManifestResourceNames();

            for (var resourceIndex = 0; resourceIndex < resources.Length; resourceIndex++)
            {
                var fileName = resources[resourceIndex];
                if ((fileName != null) && (fileName.EndsWith(".validation.xml")))
                {
                    AddResource(fileName, assembly);
                }
            }
        }



        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="IErrorMessageProvider"/> to use.
        /// </summary>
        public static IErrorMessageProvider ErrorMessageProvider
        {
			get;
        	set;
        }


    
        #endregion
    }
}