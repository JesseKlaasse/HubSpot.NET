using System.Threading;
using System.Threading.Tasks;

namespace HubSpot.NET.Api.Timeline
{
    using HubSpot.NET.Api.Timeline.Dto;
    using HubSpot.NET.Core.Abstracts;
    using HubSpot.NET.Core.Interfaces;
    using System.Collections.Generic;

    public class HubSpotTimelineApi : ApiRoutable, IHubSpotTimelineApi
    {
        private readonly IHubSpotClient _client;

        public HubSpotTimelineApi(IHubSpotClient client)
        {
            _client = client;
            MidRoute = $"/integrations/v1/{client.AppId}";

            AddRoute<TimelineEventHubSpotModel>("/timeline/event");
            AddRoute<TimelineEventTypeHubSpotModel>("/timeline/event-types");
        }

        public Task CreateOrUpdateEventAsync(TimelineEventHubSpotModel entity, CancellationToken cancellationToken = default)
        {
            CreateTimelineEventModel transportModel = new CreateTimelineEventModel(entity.EventTypeId, entity.Id, entity.ContactEmail, entity.ExtraData);
            return _client.ExecuteOnlyAsync(GetRoute<TimelineEventHubSpotModel>(), transportModel, RestSharp.Method.PUT, cancellationToken);
        }
            

        public Task CreateEventTypeAsync(TimelineEventTypeHubSpotModel entity, CancellationToken cancellationToken = default)
        => _client.ExecuteOnlyAsync(GetRoute<TimelineEventHubSpotModel>(), entity, RestSharp.Method.POST, cancellationToken);
        

        public Task DeleteEventTypeAsync(long entityID, CancellationToken cancellationToken = default)
        => _client.ExecuteOnlyAsync(GetRoute<TimelineEventTypeHubSpotModel>(entityID.ToString()), RestSharp.Method.DELETE, cancellationToken);
        

        public Task<TimelineEventHubSpotModel> GetEventByIdAsync(long entityID, CancellationToken cancellationToken = default)
        => _client.ExecuteAsync<TimelineEventHubSpotModel>(GetRoute<TimelineEventHubSpotModel>(entityID.ToString()), cancellationToken: cancellationToken);
        

        public Task<List<TimelineEventTypeHubSpotModel>> GetAllEventTypesAsync(CancellationToken cancellationToken = default)
        => _client.ExecuteAsync<List<TimelineEventTypeHubSpotModel>>(GetRoute<TimelineEventTypeHubSpotModel>(), cancellationToken: cancellationToken);
        

        public Task UpdateEventAsync(TimelineEventHubSpotModel entity, CancellationToken cancellationToken = default)
        => _client.ExecuteOnlyAsync(GetRoute<TimelineEventHubSpotModel>(entity.Id.ToString()), entity, RestSharp.Method.PUT, cancellationToken);
        

        public Task UpdateEventTypeAsync(TimelineEventTypeHubSpotModel entity, CancellationToken cancellationToken = default)
        => _client.ExecuteOnlyAsync(GetRoute<TimelineEventTypeHubSpotModel>(entity.Id.ToString()), entity, RestSharp.Method.PUT, cancellationToken);
        
    }
}