using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Api.Company;
using HubSpot.NET.Api.Company.Dto;

namespace HubSpot.NET.Core.Interfaces
{
    public interface IHubSpotCompanyApi : IHubSpotCompanyApi<CompanyHubSpotModel>
    { }

    public interface IHubSpotCompanyApi<T> : ICRUDable<T>
        where T : IHubSpotModel
    {
        Task<CompanySearchResultModel<T>> GetByDomainAsync(string domain, CompanySearchByDomain options = null, CancellationToken cancellationToken = default);
        Task<CompanyListHubSpotModel<T>> ListAsync(ListRequestOptions opts = null, CancellationToken cancellationToken = default);
    }
}