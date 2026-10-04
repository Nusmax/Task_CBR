using Finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Domain.Abstractions
{
    public interface IFinanceRepository
    {
        Task<List<Currency>> GetFavoritesAsync(Guid userId, CancellationToken ct = default);
    }

}
