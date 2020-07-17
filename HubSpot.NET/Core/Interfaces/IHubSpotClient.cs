using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Api.OAuth.Dto;
using RestSharp;

namespace HubSpot.NET.Core.Interfaces
{
    public interface IHubSpotClient
    {
        string AppId { get; }
        string BasePath { get; }

        Task<T> ExecuteAsync<T>(string absoluteUriPath, Method method = Method.GET, CancellationToken cancellationToken = default) where T: new();
        Task<T> ExecuteAsync<T,K>(string absoluteUriPath, K entity, Method method = Method.GET, CancellationToken cancellationToken = default) where T: new();        
        Task<T> ExecuteMultipartAsync<T>(string absoluteUriPath, byte[] data, string filename, Dictionary<string, string> parameters, Method method = Method.POST, CancellationToken cancellationToken = default);
        Task ExecuteOnlyAsync(string absoluteUriPath, Method method = Method.GET, CancellationToken cancellationToken = default);
        Task ExecuteOnlyAsync<T>(string absoluteUriPath, T entity, Method method = Method.GET, CancellationToken cancellationToken = default);
        Task ExecuteBatchAsync(string absoluteUriPath, IEnumerable<object> entities, Method method = Method.GET, CancellationToken cancellationToken = default);
        Task UpdateTokenAsync(HubSpotToken token, CancellationToken cancellationToken = default);
    }
}