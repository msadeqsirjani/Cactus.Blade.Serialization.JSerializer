using Cactus.Blade.Guard;
using Cactus.Blade.Serialization;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Serialization.JSerializer
{
    /// <summary>
    /// An XML implementation of the <see cref="ISerializer"/> interface using <see cref="DataContractSerializer"/>/>.
    /// </summary>
    public class JSerializerXmlSerializer : ISerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JSerializerXmlSerializer"/> class.
        /// </summary>
        /// <param name="name">The name of the serializer, used to when selecting which serializer to use.</param>
        /// <param name="settings">Settings for customizing the XmlSerializer.</param>
        public JSerializerXmlSerializer(string name = "default", DataContractSerializerSettings settings = null)
        {
            Name = name ?? "default";
            Settings = settings;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="DataContractSerializerSettings"/> settings.
        /// </summary>
        public DataContractSerializerSettings Settings { get; }

        /// <inheritdoc />
        public void SerializeToStream(Stream stream, object item, Type type)
        {
            Guard.Against.Null(stream, nameof(stream));
            Guard.Against.Null(item, nameof(item));
            Guard.Against.Null(type, nameof(type));

            var serializer = Settings.IsNull()
                ? new DataContractSerializer(type)
                : new DataContractSerializer(type, Settings);

            serializer.WriteObject(stream, item);
        }

        /// <inheritdoc />
        public object DeserializeFromStream(Stream stream, Type type)
        {
            Guard.Against.Null(stream, nameof(stream));
            Guard.Against.Null(type, nameof(type));

            var serializer = Settings.IsNull()
                ? new DataContractSerializer(type)
                : new DataContractSerializer(type, Settings);

            return serializer.ReadObject(stream);
        }

        /// <inheritdoc />
        public string SerializeToString(object item, Type type)
        {
            Guard.Against.Null(item, nameof(item));
            Guard.Against.Null(type, nameof(type));

            var serializer = Settings.IsNull()
                ? new DataContractSerializer(type)
                : new DataContractSerializer(type, Settings);

            var builder = new StringBuilder();
            using var xmlWriter = XmlWriter.Create(builder);
            serializer.WriteObject(xmlWriter, item);

            return builder.ToString();
        }

        /// <inheritdoc />
        public object DeserializeFromString(string data, Type type)
        {
            Guard.Against.Null(data, nameof(data));
            Guard.Against.Null(type, nameof(type));

            var serializer = Settings.IsNull()
                ? new DataContractSerializer(type)
                : new DataContractSerializer(type, Settings);

            using var stringReader = new StringReader(data);
            using var xmlReader = XmlReader.Create(stringReader);

            return serializer.ReadObject(xmlReader);
        }
    }
}
