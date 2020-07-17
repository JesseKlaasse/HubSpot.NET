using System.Threading;
using System.Threading.Tasks;

namespace HubSpot.NET.Core.Interfaces
{
    using HubSpot.NET.Api.OAuth.Dto;

    public interface IHubSpotOAuthApi
    {
        Task<HubSpotToken> AuthorizeAsync(string redirectCode, string redirectUri, CancellationToken cancellationToken = default);
        Task<HubSpotToken> RefreshAsync(string redirectUri, HubSpotToken token, CancellationToken cancellationToken = default);
        Task UpdateCredentialsAsync(string id, string secret, CancellationToken cancellationToken = default);
    }
}
