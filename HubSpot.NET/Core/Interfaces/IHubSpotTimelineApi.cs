using System.Threading;

namespace HubSpot.NET.Core.Interfaces
{
    using HubSpot.NET.Api.Timeline.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IHubSpotTimelineApi
    {

        Task CreateOrUpdateEventAsync(TimelineEventHubSpotModel entity, CancellationToken cancellationToken = default);
        Task CreateEventTypeAsync(TimelineEventTypeHubSpotModel entity, CancellationToken cancellationToken = default);
        Task DeleteEventTypeAsync(long entityID, CancellationToken cancellationToken = default);
        Task UpdateEventTypeAsync(TimelineEventTypeHubSpotModel entity, CancellationToken cancellationToken = default);
        Task<TimelineEventHubSpotModel> GetEventByIdAsync(long entityID, CancellationToken cancellationToken = default);
        //TimelineEventTypeHubSpotModel GetEventTypeById(long entityID);
        Task<List<TimelineEventTypeHubSpotModel>> GetAllEventTypesAsync(CancellationToken cancellationToken = default);
    }
}
