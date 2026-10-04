using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entities;

namespace User.Domain.Abstractions
{
    public interface IFavoriteRepository
    {
        Task AddAsync(Favorite fav, CancellationToken ct = default);
        Task RemoveAsync(Guid userId, string currencyId, CancellationToken ct = default);
        Task<bool> CurrencyExistsAsync(string currencyId, CancellationToken ct = default);
    }
}
