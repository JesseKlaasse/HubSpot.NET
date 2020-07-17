using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Api.Properties.Dto;
using HubSpot.NET.Core;

namespace HubSpot.NET.Examples
{
    public class CompanyProperties
    {
        public static async Task Example(HubSpotApi api, CancellationToken cancellationToken = default)
        {
           /**
             * Get all company properties
             */
            var properties = await api.CompanyProperties.GetAllAsync(cancellationToken);

            /**
             * Create a new company property
             * See https://developers.hubspot.com/docs/methods/companies/create_company_property for information of type/field type etc.
             */
            var newProp = await api.CompanyProperties.CreateAsync(new CompanyPropertyHubSpotModel()
            {
                Name = "exampleproperty", //should be lowercase
                Label = "Example Property",
                Description = "This is an example property",
                GroupName = "companyinformation",
                Type = "string",
                FieldType = "text"
            }, cancellationToken);
        }
    }
}
