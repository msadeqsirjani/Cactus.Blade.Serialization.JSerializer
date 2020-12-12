using FluentAssertions;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Xunit;

namespace Serialization.JSerializer.Test
{
    public class JSerializerJsonSerializerTest
    {
        private readonly string _expectedJson;
        private readonly TypeForJsonSerializer _expectedItem;

        public JSerializerJsonSerializerTest()
        {
            _expectedItem = new TypeForJsonSerializer { PropA = 5, PropB = true, PropC = "PropC" };
            _expectedJson = @"{""PropA"":5,""PropB"":true,""PropC"":""PropC""}";
        }

        [Fact]
        public void EmptyConstructorCreatesDefaultValues()
        {
            var serializer = new JSerializerJsonSerializer();

            serializer.Name.Should().Be("default");
            serializer.Settings.Should().BeNull();
        }

        [Fact]
        public void ConstructorWithNullNameCreatesDefaultName()
        {
            var serializer = new JSerializerJsonSerializer(null);

            serializer.Name.Should().Be("default");
        }

        [Fact]
        public void ConstructorPassesValuesCorrectly()
        {
            var name = "notdefault";
            var options = new DataContractJsonSerializerSettings();

            var serializer = new JSerializerJsonSerializer(name, options);

            serializer.Name.Should().Be(name);
            serializer.Settings.Should().NotBeNull();
            serializer.Settings.Should().BeSameAs(options);
        }

        [Fact]
        public void SerializeToStreamThrowWhenStreamIsNull()
        {
            Action action = () => new JSerializerJsonSerializer().SerializeToStream(null, new object(), typeof(object));

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: stream");
        }

        [Fact]
        public void SerializeToStreamThrowWhenItemIsNull()
        {
            Action action = () => new JSerializerJsonSerializer().SerializeToStream(new MemoryStream(), null, typeof(object));

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: item");
        }

        [Fact]
        public void SerializeToStreamThrowWhenTypeIsNull()
        {
            Action action = () => new JSerializerJsonSerializer().SerializeToStream(new MemoryStream(), new object(), null);

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: type");
        }

        [Fact]
        public void DeserializeFromStreamThrowWhenStreamIsNull()
        {
            Action action = () => new JSerializerJsonSerializer().DeserializeFromStream(null, typeof(object));

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: stream");
        }

        [Fact]
        public void DeserializeFromStreamThrowWhenTypeIsNull()
        {
            Action action = () => new JSerializerJsonSerializer().DeserializeFromStream(new MemoryStream(), null);

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: type");
        }

        [Fact]
        public void SerializeToStringThrowWhenItemIsNull()
        {
            Action action = () => new JSerializerJsonSerializer().SerializeToString(null, typeof(object));

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: item");
        }

        [Fact]
        public void SerializeToStringThrowWhenTypeIsNull()
        {
            Action action = () => new JSerializerJsonSerializer().SerializeToString(new object(), null);

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: type");
        }

        [Fact]
        public void DeserializeFromStringThrowWhenItemIsNull()
        {
            Action action = () => new JSerializerJsonSerializer().DeserializeFromString(null, typeof(object));
            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: data");
        }

        [Fact]
        public void DeserializeFromStringThrowWhenTypeIsNull()
        {
            Action action = () => new JSerializerJsonSerializer().DeserializeFromString("", null);

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: type");
        }

        [Fact]
        public void SerializeToStreamSerializesCorrectly()
        {
            var serializer = new JSerializerJsonSerializer();

            using var stream = new MemoryStream();
            serializer.SerializeToStream(stream, _expectedItem, typeof(TypeForJsonSerializer));

            stream.Flush();
            var data = stream.ToArray();

            using var readStream = new MemoryStream(data);
            using var reader = new StreamReader(readStream);
            var json = reader.ReadToEnd();
            json.Should().Be(_expectedJson);
        }

        [Fact]
        public void DeserializeFromStreamDeserializesCorrectly()
        {
            var serializer = new JSerializerJsonSerializer();

            TypeForJsonSerializer item;
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream, new UTF8Encoding(false, true), 1024, true))
                {
                    writer.Write(_expectedJson);
                }
                stream.Seek(0, SeekOrigin.Begin);

                item = serializer.DeserializeFromStream(stream, typeof(TypeForJsonSerializer)) as TypeForJsonSerializer;
            }

            item.Should().NotBeNull();
            item.Should().BeEquivalentTo(_expectedItem);
        }

        [Fact]
        public void SerializeToStringSerializesCorrectly()
        {
            var serializer = new JSerializerJsonSerializer();

            var json = serializer.SerializeToString(_expectedItem, typeof(TypeForJsonSerializer));

            json.Should().Be(_expectedJson);
        }

        [Fact]
        public void DeserializeFromStringDeserializesCorrectly()
        {
            var serializer = new JSerializerJsonSerializer();

            var item = serializer.DeserializeFromString(_expectedJson, typeof(TypeForJsonSerializer)) as TypeForJsonSerializer;

            item.Should().NotBeNull();
            item.Should().BeEquivalentTo(_expectedItem);
        }

        [DataContract]
        public class TypeForJsonSerializer
        {
            [DataMember]
            public int PropA { get; set; }
            [DataMember]
            public bool PropB { get; set; }
            [DataMember]
            public string PropC { get; set; }
        }
    }
}
