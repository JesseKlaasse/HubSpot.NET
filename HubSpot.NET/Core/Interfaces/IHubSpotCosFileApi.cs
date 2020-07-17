using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Api.Files.Dto;

namespace HubSpot.NET.Core.Interfaces
{
    public interface IHubSpotCosFileApi
    {
        Task<FolderHubSpotModel> CreateFolderAsync(FolderHubSpotModel folder, CancellationToken cancellationToken = default);
        Task<FileListHubSpotModel<FileHubSpotModel>> UploadAsync(FileHubSpotModel entity, CancellationToken cancellationToken = default);
    }
}