using System.Threading;
using System.Threading.Tasks;

namespace HubSpot.NET.Api.Company
{
    using Flurl;
    using HubSpot.NET.Api.Company.Dto;
    using HubSpot.NET.Core;
    using HubSpot.NET.Core.Abstracts;
    using HubSpot.NET.Core.Interfaces;
    using RestSharp;
    using System;
    using System.Linq;

    public class HubSpotCompanyApi : ApiRoutable, IHubSpotCompanyApi
    {
        private readonly IHubSpotClient _client;
        public override string MidRoute => "/companies/v2";        

        public HubSpotCompanyApi(IHubSpotClient client)
        {
            _client = client;            
        }

        /// <summary>
        /// Creates a Company entity
        /// </summary>
        /// <typeparam name="T">Implementation of CompanyHubSpotModel</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>The created entity (with ID set)</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CompanyHubSpotModel> CreateAsync(CompanyHubSpotModel entity, CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<CompanyHubSpotModel,CompanyHubSpotModel>(GetRoute<CompanyHubSpotModel>("companies"), entity, Method.POST, cancellationToken);

        /// <summary>
        /// Gets a specific company by it's ID
        /// </summary>
        /// <typeparam name="T">Implementation of CompanyHubSpotModel</typeparam>
        /// <param name="companyId">The ID</param>
        /// <returns>The company entity</returns>
        public Task<CompanyHubSpotModel> GetByIdAsync(long companyId, CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<CompanyHubSpotModel>(GetRoute<CompanyHubSpotModel>("companies", companyId.ToString()), cancellationToken: cancellationToken);

        /// <summary>
        /// Gets a company by domain name
        /// </summary>
        /// <typeparam name="T">Implementation of CompanyHubSpotModel</typeparam>
        /// <param name="domain">Domain name to search for</param>
        /// <param name="options">Set of search options</param>
        /// <returns>The company entity</returns>
        public Task<CompanySearchResultModel<CompanyHubSpotModel>> GetByDomainAsync(string domain, CompanySearchByDomain opts = null, CancellationToken cancellationToken = default)
        {
            opts = opts ?? new CompanySearchByDomain();

            var path = GetRoute<CompanyHubSpotModel>("domains", domain, "companies");

            return _client.ExecuteAsync<CompanySearchResultModel<CompanyHubSpotModel>, CompanySearchByDomain>(path, opts, Method.POST, cancellationToken);
        }

        public Task<CompanyListHubSpotModel<CompanyHubSpotModel>> ListAsync(ListRequestOptions opts = null, CancellationToken cancellationToken = default)
        {
            opts = opts ?? new ListRequestOptions();

            var path = GetRoute<CompanyHubSpotModel>("companies", "paged").SetQueryParam("count", opts.Limit);

            if (opts.PropertiesToInclude.Any())
                path.SetQueryParam("properties", opts.PropertiesToInclude);

            if (opts.Offset.HasValue)
                path = path.SetQueryParam("offset", opts.Offset);

            return _client.ExecuteAsync<CompanyListHubSpotModel<CompanyHubSpotModel>, ListRequestOptions>(path, opts, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Updates a given company entity, any changed properties are updated
        /// </summary>
        /// <typeparam name="T">Implementation of CompanyHubSpotModel</typeparam>
        /// <param name="entity">The company entity</param>
        /// <returns>The updated company entity</returns>
        public Task<CompanyHubSpotModel> UpdateAsync(CompanyHubSpotModel entity, CancellationToken cancellationToken = default)
        {
            if (entity.Id < 1)
                throw new ArgumentException("Company entity must have an id set!");

            return _client.ExecuteAsync<CompanyHubSpotModel, CompanyHubSpotModel>(GetRoute<CompanyHubSpotModel>("companies", entity.Id.ToString()), entity, Method.PUT, cancellationToken);
        }

        /// <summary>
        /// Deletes the given company
        /// </summary>
        /// <param name="companyId">ID of the company</param>
        public Task DeleteAsync(long companyId, CancellationToken cancellationToken = default)
            => _client.ExecuteOnlyAsync(GetRoute<CompanyHubSpotModel>("companies", companyId.ToString()), method: Method.DELETE, cancellationToken: cancellationToken);
    }
}