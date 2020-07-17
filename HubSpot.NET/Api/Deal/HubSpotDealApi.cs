using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using HubSpot.NET.Api.Deal.Dto;
using HubSpot.NET.Api.Shared;
using HubSpot.NET.Core;
using HubSpot.NET.Core.Abstracts;
using HubSpot.NET.Core.Interfaces;
using RestSharp;

namespace HubSpot.NET.Api.Deal
{
    public class HubSpotDealApi : ApiRoutable, IHubSpotDealApi
    {
        private readonly IHubSpotClient _client;
        public override string MidRoute => "/deals/v1";

        public HubSpotDealApi(IHubSpotClient client)
        {
            _client = client;
            AddRoute<DealHubSpotModel>("/deal");
        }

        /// <summary>
        /// Creates a deal entity
        /// </summary>
        /// <typeparam name="T">Implementation of DealHubSpotModel</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>The created entity (with ID set)</returns>
        public Task<DealHubSpotModel> CreateAsync(DealHubSpotModel entity, CancellationToken cancellationToken = default)
        {
            NameTransportModel<DealHubSpotModel> model = new NameTransportModel<DealHubSpotModel>();
            model.ToPropertyTransportModel(entity);

            return _client.ExecuteAsync<DealHubSpotModel,NameTransportModel<DealHubSpotModel>>(GetRoute<DealHubSpotModel>(), model, Method.POST, cancellationToken);
        }

        /// <summary>
        /// Gets a single deal by ID
        /// </summary>
        /// <param name="dealId">ID of the deal</param>
        /// <typeparam name="T">Implementation of DealHubSpotModel</typeparam>
        /// <returns>The deal entity</returns>
        public Task<DealHubSpotModel> GetByIdAsync(long dealId, CancellationToken cancellationToken = default) 
            => _client.ExecuteAsync<DealHubSpotModel>(GetRoute<DealHubSpotModel>(dealId.ToString()), cancellationToken: cancellationToken);

        /// <summary>
        /// Updates a given deal
        /// </summary>
        /// <typeparam name="T">Implementation of DealHubSpotModel</typeparam>
        /// <param name="entity">The deal entity</param>
        /// <returns>The updated deal entity</returns>
        public Task<DealHubSpotModel> UpdateAsync(DealHubSpotModel entity, CancellationToken cancellationToken = default)
        {
            if (entity.Id < 1)            
                throw new ArgumentException("Deal entity must have an id set!");

            return _client.ExecuteAsync<DealHubSpotModel, DealHubSpotModel>(GetRoute<DealHubSpotModel>(entity.Id.ToString()), entity, method: Method.PUT, cancellationToken: cancellationToken);            
        }

        /// <summary>
        /// Gets a list of deals
        /// </summary>
        /// <typeparam name="T">Implementation of DealListHubSpotModel</typeparam>
        /// <param name="opts">Options (limit, offset) relating to request</param>
        /// <returns>List of deals</returns>
        public Task<DealListHubSpotModel<DealHubSpotModel>> ListAsync(bool includeAssociations, ListRequestOptions opts = null, CancellationToken cancellationToken = default)
        {
            opts = opts ?? new ListRequestOptions(250);         

            Url path = GetRoute<DealListHubSpotModel<DealHubSpotModel>>("deal", "paged").SetQueryParam("limit", opts.Limit);

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("offset", opts.Offset);            

            if (includeAssociations)            
                path = path.SetQueryParam("includeAssociations", "true");            

            if (opts.PropertiesToInclude.Any())            
                path = path.SetQueryParam("properties", opts.PropertiesToInclude);           

            return _client.ExecuteAsync<DealListHubSpotModel<DealHubSpotModel>, ListRequestOptions>(path, opts, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Gets a list of deals associated to a hubSpot Object
        /// </summary>
        /// <typeparam name="T">Implementation of DealListHubSpotModel</typeparam>
        /// <param name="includeAssociations">Bool include associated Ids</param>
        /// <param name="hubId">Long Id of Hubspot object related to deals</param>
        /// <param name="objectName">String name of Hubspot object related to deals (contact\account)</param>
        /// <param name="opts">Options (limit, offset) relating to request</param>
        /// <returns>List of deals</returns>
        public Task<DealListHubSpotModel<DealHubSpotModel>> ListAssociatedAsync(bool includeAssociations, long hubId, ListRequestOptions opts = null, string objectName = "contact", CancellationToken cancellationToken = default)
        {
            opts = opts ?? new ListRequestOptions();            

            Url path = $"{GetRoute<DealListHubSpotModel<DealHubSpotModel>>()}/deal/associated/{objectName}/{hubId}/paged"
                .SetQueryParam("limit", opts.Limit);

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("offset", opts.Offset);            

            if (includeAssociations)            
                path = path.SetQueryParam("includeAssociations", "true");            

            if (opts.PropertiesToInclude.Any())            
                path = path.SetQueryParam("properties", opts.PropertiesToInclude);            

            return _client.ExecuteAsync<DealListHubSpotModel<DealHubSpotModel>, ListRequestOptions>(path, opts, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Deletes a given deal (by ID)
        /// </summary>
        /// <param name="dealId">ID of the deal</param>
        public Task DeleteAsync(long dealId, CancellationToken cancellationToken = default) 
            => _client.ExecuteOnlyAsync(GetRoute<DealHubSpotModel>(dealId.ToString()), method: Method.DELETE, cancellationToken: cancellationToken);

        /// <summary>
        /// Gets a list of recently created deals
        /// </summary>
        /// <typeparam name="T">Implementation of DealListHubSpotModel</typeparam>
        /// <param name="opts">Options (limit, offset) relating to request</param>
        /// <returns>List of deals</returns>
        public Task<DealRecentListHubSpotModel<DealHubSpotModel>> RecentlyCreatedAsync(DealRecentRequestOptions opts = null, CancellationToken cancellationToken = default)
        {
            opts = opts ?? new DealRecentRequestOptions();            

            Url path = $"{GetRoute<DealRecentListHubSpotModel<DealHubSpotModel>>()}/deal/recent/created"
                            .SetQueryParam("limit", opts.Limit);

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("offset", opts.Offset);            

            if (opts.IncludePropertyVersion)            
                path = path.SetQueryParam("includePropertyVersions", "true");            

            if (!string.IsNullOrEmpty(opts.Since))            
                path = path.SetQueryParam("since", opts.Since);            

            return _client.ExecuteAsync<DealRecentListHubSpotModel<DealHubSpotModel>, DealRecentRequestOptions>(path, opts, cancellationToken: cancellationToken);            
        }

        /// <summary>
        /// Gets a list of recently modified deals
        /// </summary>
        /// <typeparam name="T">Implementation of DealListHubSpotModel</typeparam>
        /// <param name="opts">Options (limit, offset) relating to request</param>
        /// <returns>List of deals</returns>
        public Task<DealRecentListHubSpotModel<DealHubSpotModel>> RecentlyUpdatedAsync(DealRecentRequestOptions opts = null, CancellationToken cancellationToken = default)
        {
            opts = opts ?? new DealRecentRequestOptions();            

            var path = $"{GetRoute<DealRecentListHubSpotModel<DealHubSpotModel>>()}/deal/recent/modified"
                .SetQueryParam("limit", opts.Limit);

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("offset", opts.Offset);

            if (opts.IncludePropertyVersion)
                path = path.SetQueryParam("includePropertyVersions", "true");

            if (!string.IsNullOrEmpty(opts.Since))             
                path = path.SetQueryParam("since", opts.Since);

            return _client.ExecuteAsync<DealRecentListHubSpotModel<DealHubSpotModel>, DealRecentRequestOptions>(path, opts, cancellationToken: cancellationToken);
        }
    }
}
