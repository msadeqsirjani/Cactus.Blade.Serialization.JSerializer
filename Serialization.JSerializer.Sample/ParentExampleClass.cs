using System;
using System.Runtime.Serialization;

namespace Serialization.JSerializer.Sample
{
    [DataContract]
    public class ParentExampleClass
    {
        [DataMember]
        public DateTime DateProperty { get; set; }
        [DataMember]
        public NestedExampleClass ClassProperty { get; set; }
    }
}