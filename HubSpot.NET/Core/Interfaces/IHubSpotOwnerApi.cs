using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Api.Owner.Dto;

namespace HubSpot.NET.Core.Interfaces
{
    public interface IHubSpotOwnerApi
    {
        Task<OwnerListHubSpotModel<T>> GetAllAsync<T>(CancellationToken cancellationToken = default) where T : OwnerHubSpotModel;
    }
}