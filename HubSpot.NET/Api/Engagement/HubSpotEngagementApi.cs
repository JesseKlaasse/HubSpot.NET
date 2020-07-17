using System;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using HubSpot.NET.Api.Engagement.Dto;
using HubSpot.NET.Core.Abstracts;
using HubSpot.NET.Core.Interfaces;
using RestSharp;

namespace HubSpot.NET.Api.Engagement
{
    public class HubSpotEngagementApi : ApiRoutable, IHubSpotEngagementApi
    {
        private readonly IHubSpotClient _client;
        public override string MidRoute => "/engagements/v1";

        public HubSpotEngagementApi(IHubSpotClient client)
        {
            _client = client;
            AddRoute<EngagementHubSpotModel>("engagements/");
        }

        /// <summary>
        /// Creates an engagement
        /// </summary>
        /// <param name="entity">The engagement to create</param>
        /// <returns>The created engagement (with ID set)</returns>
        public Task<EngagementHubSpotModel> CreateAsync(EngagementHubSpotModel entity, CancellationToken cancellationToken = default) 
            => _client.ExecuteAsync<EngagementHubSpotModel, EngagementHubSpotModel>(GetRoute<EngagementHubSpotModel>(), entity, Method.POST, cancellationToken);

        /// <summary>
        /// Updates a given engagement
        /// </summary>
        /// <param name="entity">The updated engagement</param>
        public Task UpdateAsync(EngagementHubSpotModel entity, CancellationToken cancellationToken = default)
        {
            if (entity.Engagement.Id < 1)
                throw new ArgumentException("Engagement entity must have an id set!");

            return _client.ExecuteOnlyAsync(GetRoute<EngagementHubSpotModel>(entity.Engagement.Id.ToString()), entity, Method.PATCH, cancellationToken);
        }

        /// <summary>
        /// Gets a given engagement (by ID)
        /// </summary>
        /// <param name="engagementId">The ID of the engagement</param>
        /// <returns>The engagement</returns>
        public Task<EngagementHubSpotModel> GetByIdAsync(long engagementId, CancellationToken cancellationToken = default) 
            => _client.ExecuteAsync<EngagementHubSpotModel>(GetRoute<EngagementHubSpotModel>(engagementId.ToString()), cancellationToken: cancellationToken);

        /// <summary>
        /// Retrieves a paginated list of engagements
        /// </summary>
        /// <param name="opts">Options for querying</param>
        /// <returns>List of engagements, with additional metadata, e.g. total</returns>
        public Task<EngagementListHubSpotModel<T>> ListAsync<T>(EngagementListRequestOptions opts = null, CancellationToken cancellationToken = default) where T: EngagementHubSpotModel
        {
            opts = opts ?? new EngagementListRequestOptions();

            var path = $"{GetRoute<T>("paged")}".SetQueryParam("limit", opts.Limit);

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("offset", opts.Offset);            

            return _client.ExecuteAsync<EngagementListHubSpotModel<T>, EngagementListRequestOptions>(path, opts, cancellationToken: cancellationToken);            
        }

        /// <summary>
        /// Lists recent engagements (i.e. in date order)
        /// </summary>
        /// <param name="opts">Options for querying</param>
        /// <returns>List of engagements, with additional metadata, e.g. total</returns>
        public Task<EngagementListHubSpotModel<T>> ListRecentAsync<T>(EngagementListRequestOptions opts = null, CancellationToken cancellationToken = default) where T : EngagementHubSpotModel
        {
            opts = opts ?? new EngagementListRequestOptions();

            var path = $"{GetRoute<T>()}/engagements/recent/modified".SetQueryParam("count", opts.Limit);

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("offset", opts.Offset);

            return _client.ExecuteAsync<EngagementListHubSpotModel<T>, EngagementListRequestOptions>(path, opts, cancellationToken: cancellationToken);           
        }

        /// <summary>
        /// Deletes a given engagement (by ID)
        /// </summary>
        /// <param name="engagementId">The ID of the engagement</param>
        public Task DeleteAsync(long engagementId, CancellationToken cancellationToken = default)
            => _client.ExecuteOnlyAsync(GetRoute<EngagementHubSpotModel>(engagementId.ToString()), method: Method.DELETE, cancellationToken: cancellationToken);

        /// <summary>
        /// Associates an engagement with a specific object type and ID 
        /// </summary>
        /// <param name="engagementId">The ID of the enagement</param>
        /// <param name="objectType">The object type, e.g CONTACT</param>
        /// <param name="objectId">The ID of the object</param>
        public Task AssociateAsync(long engagementId, string objectType, long objectId, CancellationToken cancellationToken = default) 
            => _client.ExecuteOnlyAsync(GetRoute<EngagementHubSpotModel>(engagementId.ToString(), "associations", objectType, objectId.ToString()), method: Method.PUT, cancellationToken: cancellationToken);

        /// <summary>
        /// Lists associated engagements for a given object type and ID
        /// </summary>
        /// <param name="objectId">The object ID</param>
        /// <param name="objectType">The object type, e.g. CONTACT</param>
        /// <param name="opts">Options used for querying</param>
        /// <returns>List of associated engagements</returns>
        public Task<EngagementListHubSpotModel<T>> ListAssociatedAsync<T>(long objectId, string objectType, EngagementListRequestOptions opts = null, CancellationToken cancellationToken = default) where T: EngagementHubSpotModel
        {
            opts = opts ?? new EngagementListRequestOptions();
            
            var path = $"{GetRoute<T>()}/engagements/associated/{objectType}/{objectId}/paged".SetQueryParam("limit", opts.Limit);

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("offset", opts.Offset);            

            return _client.ExecuteAsync<EngagementListHubSpotModel<T>, EngagementListRequestOptions>(path, opts, cancellationToken: cancellationToken);            
        }
    }
}
