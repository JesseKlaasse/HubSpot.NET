using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Api.Contact.Dto;

namespace HubSpot.NET.Core.Interfaces
{
    public interface IHubSpotContactApi : IHubSpotContactApi<ContactHubSpotModel>
    { }

    public interface IHubSpotContactApi<T> : ICRUDable<T>
        where T : IHubSpotModel
    {
        Task<ContactListHubSpotModel<T>> ListAsync(ListRequestOptions opts = null, CancellationToken cancellationToken = default);
        Task<ContactListHubSpotModel<T>> RecentlyCreatedAsync(ListRecentRequestOptions opts = null, CancellationToken cancellationToken = default);
        Task<ContactListHubSpotModel<T>> RecentlyUpdatedAsync(ListRecentRequestOptions opts = null, CancellationToken cancellationToken = default);
        Task<ContactSearchHubSpotModel<T>> SearchAsync(ContactSearchRequestOptions opts = null, CancellationToken cancellationToken = default);
        Task<T> GetByEmailAsync(string email, bool includeHistory = true, CancellationToken cancellationToken = default);
        Task<T> GetByUserTokenAsync(string userToken, bool includeHistory = true, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(long contactId, bool includeHistory = true, CancellationToken cancellationToken = default);
        Task<T> CreateOrUpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> CreateOrUpdateAsync(string originalEmail, T entity, CancellationToken cancellationToken = default);
        Task BatchAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
