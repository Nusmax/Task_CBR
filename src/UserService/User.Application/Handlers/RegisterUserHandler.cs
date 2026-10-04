using MediatR;
using SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Abstractions;

namespace User.Application.Handlers
{
    public class RegisterUserHandler(IUserRepository repo, IPasswordHasher hasher)
    : IRequestHandler<Commands.RegisterUserCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(Commands.RegisterUserCommand r, CancellationToken ct)
        {
            if (await repo.GetByNameAsync(r.Name, ct) is not null)
                return Result<Guid>.Fail("UserAlreadyExists");
            var u = new Domain.Entities.AppUser { Name = r.Name, PasswordHash = hasher.Hash(r.Password) };
            await repo.AddAsync(u, ct);
            return Result<Guid>.Ok(u.Id);
        }
    }
}
