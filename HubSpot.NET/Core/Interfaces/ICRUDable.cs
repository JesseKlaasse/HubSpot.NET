using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HubSpot.NET.Core.Interfaces
{
    public interface ICRUDable
    {
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    }

    public interface ICRUDable<T> : ICRUDable
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    }
}
