using Cactus.Blade.Guard;
using Cactus.Blade.Serialization;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Serialization.JSerializer
{
    /// <summary>
    /// A JSON implementation of the <see cref="ISerializer"/> interface using <see cref="DataContractJsonSerializer"/>/>.
    /// </summary>
    public class JSerializerJsonSerializer : ISerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContractJsonSerializer"/> class.
        /// </summary>
        /// <param name="name">The name of the serializer, used to when selecting which serializer to use.</param>
        /// <param name="settings">Options for customizing the JsonSerializer.</param>
        public JSerializerJsonSerializer(string name = "default", DataContractJsonSerializerSettings settings = null)
        {
            Name = name ?? "default";
            Settings = settings;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="DataContractJsonSerializerSettings"/> options.
        /// </summary>
        public DataContractJsonSerializerSettings Settings { get; }

        /// <inheritdoc />
        public void SerializeToStream(Stream stream, object item, Type type)
        {
            Guard.Against.Null(stream, nameof(stream));
            Guard.Against.Null(item, nameof(item));
            Guard.Against.Null(type, nameof(type));

            var serializer = Settings.IsNull()
                ? new DataContractJsonSerializer(type)
                : new DataContractJsonSerializer(type, Settings);

            serializer.WriteObject(stream, item);
        }

        /// <inheritdoc />
        public object DeserializeFromStream(Stream stream, Type type)
        {
            Guard.Against.Null(stream, nameof(stream));
            Guard.Against.Null(type, nameof(type));

            var serializer = Settings.IsNull()
                ? new DataContractJsonSerializer(type)
                : new DataContractJsonSerializer(type, Settings);

            return serializer.ReadObject(stream);
        }

        /// <inheritdoc />
        public string SerializeToString(object item, Type type)
        {
            Guard.Against.Null(item, nameof(item));
            Guard.Against.Null(type, nameof(type));

            var serializer = Settings.IsNull()
                ? new DataContractJsonSerializer(type)
                : new DataContractJsonSerializer(type, Settings);

            using var stream = new MemoryStream();
            serializer.WriteObject(stream, item);
            stream.Flush();

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <inheritdoc />
        public object DeserializeFromString(string data, Type type)
        {
            Guard.Against.Null(data, nameof(data));
            Guard.Against.Null(type, nameof(type));

            var serializer = Settings.IsNull()
                ? new DataContractJsonSerializer(type)
                : new DataContractJsonSerializer(type, Settings);

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));

            return serializer.ReadObject(stream);
        }
    }
}
