using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Abstractions;
using User.Domain.Entities;
using User.Infrastructure.Persistence;

namespace User.Infrastructure.Repositories
{
    public class EfFavoriteRepository(UserDbContext db) : IFavoriteRepository
    {
        public async Task AddAsync(Favorite f, CancellationToken ct = default)
        { db.Favorites.Add(f); await db.SaveChangesAsync(ct); }
        public async Task RemoveAsync(Guid u, string c, CancellationToken ct = default)
        {
            var f = await db.Favorites.FindAsync([u, c], ct);
            if (f is not null) { db.Favorites.Remove(f); await db.SaveChangesAsync(ct); }
        }
        public async Task<bool> CurrencyExistsAsync(string id, CancellationToken ct = default)
            => await db.Currencies.AnyAsync(x => x.Id == id, ct);
    }
}
