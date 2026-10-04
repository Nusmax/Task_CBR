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
    public class EfUserRepository(UserDbContext db) : IUserRepository
    {
        public async Task<AppUser?> GetByNameAsync(string n, CancellationToken ct = default)
            => await db.Users.FirstOrDefaultAsync(x => x.Name == n, ct);
        public async Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await db.Users.FindAsync([id], ct);
        public async Task AddAsync(AppUser u, CancellationToken ct = default)
        { db.Users.Add(u); await db.SaveChangesAsync(ct); }
    }
}
