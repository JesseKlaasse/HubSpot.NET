using System.Threading;
using System.Threading.Tasks;

namespace HubSpot.NET.Core.Interfaces
{
    using HubSpot.NET.Api.Contact.Dto;
    using System.Collections.Generic;

    public interface IHubSpotContactPropertyApi
    {
        Task<ContactPropertyModel> CreatePropertyAsync(ContactPropertyModel entity, CancellationToken cancellationToken = default);
        Task<List<ContactPropertyModel>> GetPropertiesAsync(CancellationToken cancellationToken = default);
        Task<ContactPropertyModel> GetPropertyAsync(string propertyName, CancellationToken cancellationToken = default);
        Task<ContactPropertyModel> UpdatePropertyAsync(ContactPropertyModel model, CancellationToken cancellationToken = default);
    }
}
