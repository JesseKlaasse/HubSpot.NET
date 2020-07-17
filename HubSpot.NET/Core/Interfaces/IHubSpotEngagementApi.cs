using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Api.Engagement;
using HubSpot.NET.Api.Engagement.Dto;

namespace HubSpot.NET.Core.Interfaces
{
    public interface IHubSpotEngagementApi
    {
        Task AssociateAsync(long engagementId, string objectType, long objectId, CancellationToken cancellationToken = default);
        Task<EngagementHubSpotModel> CreateAsync(EngagementHubSpotModel entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(long engagementId, CancellationToken cancellationToken = default);
        Task<EngagementHubSpotModel> GetByIdAsync(long engagementId, CancellationToken cancellationToken = default);
        Task<EngagementListHubSpotModel<T>> ListAsync<T>(EngagementListRequestOptions opts = null, CancellationToken cancellationToken = default) where T : EngagementHubSpotModel;
        Task<EngagementListHubSpotModel<T>> ListAssociatedAsync<T>(long objectId, string objectType, EngagementListRequestOptions opts = null, CancellationToken cancellationToken = default) where T : EngagementHubSpotModel;
        Task<EngagementListHubSpotModel<T>> ListRecentAsync<T>(EngagementListRequestOptions opts = null, CancellationToken cancellationToken = default) where T : EngagementHubSpotModel;
        Task UpdateAsync(EngagementHubSpotModel entity, CancellationToken cancellationToken = default);
    }
}