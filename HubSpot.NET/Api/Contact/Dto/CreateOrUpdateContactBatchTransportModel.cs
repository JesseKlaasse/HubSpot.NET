using System.Linq;
using System.Runtime.Serialization;

namespace HubSpot.NET.Api.Contact.Dto
{
    [DataContract]
    public class CreateOrUpdateContactBatchTransportModel : CreateOrUpdateContactTransportModel
    {
        [DataMember(Name = "email")]
        public object Email => Properties.SingleOrDefault(p => p.Property == "email")?.Value;

        public CreateOrUpdateContactBatchTransportModel()
        {
        }

        public CreateOrUpdateContactBatchTransportModel(ContactHubSpotModel model) : base(model)
        {
        }
    }
}