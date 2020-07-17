using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Api.Properties.Dto;

namespace HubSpot.NET.Core.Interfaces
{
    public interface IHubSpotCompanyPropertiesApi
    {
        Task<PropertiesListHubSpotModel<CompanyPropertyHubSpotModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CompanyPropertyHubSpotModel> CreateAsync(CompanyPropertyHubSpotModel property, CancellationToken cancellationToken = default);
    }
}
