using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entities;

namespace User.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(AppUser user, CancellationToken ct = default);
    }
}
