using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Api.Properties.Dto;
using HubSpot.NET.Core.Abstracts;
using HubSpot.NET.Core.Interfaces;
using RestSharp;

namespace HubSpot.NET.Api.Properties
{
    public class HubSpotCompaniesPropertiesApi : ApiRoutable, IHubSpotCompanyPropertiesApi
    {
        private readonly IHubSpotClient _client;
        public override string MidRoute => "/properties/v1";

        public HubSpotCompaniesPropertiesApi(IHubSpotClient client)
        {
            _client = client;
            AddRoute<CompanyPropertyHubSpotModel>("/companies/properties");
        }

        public Task<PropertiesListHubSpotModel<CompanyPropertyHubSpotModel>> GetAllAsync(CancellationToken cancellationToken = default) 
            => _client.ExecuteAsync<PropertiesListHubSpotModel<CompanyPropertyHubSpotModel>>(GetRoute<CompanyPropertyHubSpotModel>(), cancellationToken: cancellationToken);

        public Task<CompanyPropertyHubSpotModel> CreateAsync(CompanyPropertyHubSpotModel property, CancellationToken cancellationToken = default) 
            => _client.ExecuteAsync<CompanyPropertyHubSpotModel, CompanyPropertyHubSpotModel>(GetRoute<CompanyPropertyHubSpotModel>(), property, Method.POST, cancellationToken);
    }
}