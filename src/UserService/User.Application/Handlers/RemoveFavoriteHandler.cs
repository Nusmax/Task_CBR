using MediatR;
using SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Commands;
using User.Domain.Abstractions;

namespace User.Application.Handlers
{
    public class RemoveFavoriteHandler(IFavoriteRepository repo) : IRequestHandler<RemoveFavoriteCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(RemoveFavoriteCommand c, CancellationToken ct)
        {
            await repo.RemoveAsync(c.UserId, c.CurrencyId, ct);
            return Result<bool>.Ok(true);
        }
    }
}
