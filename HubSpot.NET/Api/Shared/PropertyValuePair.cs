namespace HubSpot.NET.Api.Shared
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PropertyValuePair
    {
        public PropertyValuePair() { }
        public PropertyValuePair(string prop, object value) : this()
        {
            Property = prop;
            Value = value;
        }

        [DataMember(Name = "property")]
        public string Property { get; set; }

        [DataMember(Name = "value")]
        public object Value { get; set; } // This is an object to let the (de)serializer take care of the correct formats

        public override string ToString() => $"{Property}: {Value}";
    }
}
