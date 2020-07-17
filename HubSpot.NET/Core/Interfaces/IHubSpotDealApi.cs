using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Api.Deal.Dto;

namespace HubSpot.NET.Core.Interfaces
{
    public interface IHubSpotDealApi<T> : ICRUDable<T>
        where T : IHubSpotModel   
    {
        Task<DealListHubSpotModel<T>> ListAsync(bool includeAssociations, ListRequestOptions opts = null, CancellationToken cancellationToken = default);     
        Task<DealListHubSpotModel<T>> ListAssociatedAsync(bool includeAssociations, long hubId, ListRequestOptions opts = null, string objectName = "contact", CancellationToken cancellationToken = default);
        Task<DealRecentListHubSpotModel<T>> RecentlyCreatedAsync(DealRecentRequestOptions opts = null, CancellationToken cancellationToken = default);
        Task<DealRecentListHubSpotModel<T>> RecentlyUpdatedAsync(DealRecentRequestOptions opts = null, CancellationToken cancellationToken = default);
    }

    public interface IHubSpotDealApi : IHubSpotDealApi<DealHubSpotModel> { }
}