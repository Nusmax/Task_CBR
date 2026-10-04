using Finance.Domain.Abstractions;
using Finance.Domain.Entities;
using Finance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Infrastructure.Repositories
{
    public class EfFinanceRepository(FinanceDbContext db) : IFinanceRepository
    {
        public async Task<List<Currency>> GetFavoritesAsync(Guid userId, CancellationToken ct = default)
        {
            var ids = await db.Favorites.Where(f => f.UserId == userId).Select(f => f.CurrencyId).ToListAsync(ct);
            return await db.Currencies.Where(c => ids.Contains(c.Id)).ToListAsync(ct);
        }
    }
}
