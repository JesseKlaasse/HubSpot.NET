using System.Threading;
using System.Threading.Tasks;

namespace HubSpot.NET.Api.Contact
{
    using HubSpot.NET.Api.Contact.Dto;
    using HubSpot.NET.Core.Abstracts;
    using HubSpot.NET.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HubSpotContactPropertyApi : ApiRoutable, IHubSpotContactPropertyApi
    {
        private readonly IHubSpotClient _client;

        public HubSpotContactPropertyApi(IHubSpotClient client)
        {
             MidRoute = "/properties/v1/contacts";
            _client = client;

            AddRoute<ContactPropertyModel>("properties");
        }

        public Task<ContactPropertyModel> CreatePropertyAsync(ContactPropertyModel entity, CancellationToken cancellationToken = default)
        {
            string path = GetRoute<ContactPropertyModel>();
            return _client.ExecuteAsync<ContactPropertyModel, ContactPropertyModel>(path, entity, RestSharp.Method.GET, cancellationToken);
        }

        public Task<List<ContactPropertyModel>> GetPropertiesAsync(CancellationToken cancellationToken = default)
        {
            return _client.ExecuteAsync<List<ContactPropertyModel>>(GetRoute<ContactPropertyModel>(), cancellationToken: cancellationToken);
        }

        public Task<ContactPropertyModel> GetPropertyAsync(string propertyName, CancellationToken cancellationToken = default)
        {
            string path = GetRoute<ContactPropertyModel>("named", propertyName);
            return _client.ExecuteAsync<ContactPropertyModel>(path, cancellationToken: cancellationToken);
        }

        public Task<ContactPropertyModel> UpdatePropertyAsync(ContactPropertyModel model, CancellationToken cancellationToken = default)
        {
            string path = GetRoute<ContactPropertyModel>("named", model.Name);
            return _client.ExecuteAsync<ContactPropertyModel, ContactPropertyModel>(path, model, RestSharp.Method.PUT, cancellationToken);
        }
    }
}
