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
    public class LoginUserHandler(IUserRepository repo, IPasswordHasher hasher, IJwtProvider jwt)
    : IRequestHandler<Queries.LoginUserQuery, Result<string>>
    {
        public async Task<Result<string>> Handle(Queries.LoginUserQuery q, CancellationToken ct)
        {
            var u = await repo.GetByNameAsync(q.Name, ct);
            if (u is null || !hasher.Verify(q.Password, u.PasswordHash))
                return Result<string>.Fail("InvalidCredentials");
            return Result<string>.Ok(jwt.Generate(u));
        }
    }
}
