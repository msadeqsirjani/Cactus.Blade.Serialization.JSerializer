using System.Runtime.Serialization;

namespace Serialization.JSerializer.Sample
{
    [DataContract]
    public class NestedExampleClass
    {
        [DataMember]
        public int IntProperty { get; set; }
        [DataMember]
        public bool BoolProperty { get; set; }
        [DataMember]
        public string StringProperty { get; set; }
    }
}