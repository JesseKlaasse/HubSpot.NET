﻿namespace HubSpot.NET.Core.Interfaces
{
    using HubSpot.NET.Api.Timeline.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IHubSpotTimelineApi
    {

        TimelineEventTypeHubSpotModel CreateOrUpdateEvent(TimelineEventTypeHubSpotModel entity);
        void CreateEventType(TimelineEventHubSpotModel entity);
        void DeleteEventType(long entityID);
        void UpdateEventType(TimelineEventTypeHubSpotModel entity);
        TimelineEventHubSpotModel GetEventById(long entityID);
        //TimelineEventTypeHubSpotModel GetEventTypeById(long entityID);
        IEnumerable<TimelineEventTypeHubSpotModel> GetAllEventTypes();
    }
}
