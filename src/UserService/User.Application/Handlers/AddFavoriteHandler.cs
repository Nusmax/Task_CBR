using MediatR;
using SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Commands;
using User.Domain.Abstractions;
using User.Domain.Entities;

namespace User.Application.Handlers
{
    public class AddFavoriteHandler(IFavoriteRepository repo) : IRequestHandler<AddFavoriteCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddFavoriteCommand c, CancellationToken ct)
        {
            if (!await repo.CurrencyExistsAsync(c.CurrencyId, ct))
                return Result<bool>.Fail("CurrencyNotFound");
            await repo.AddAsync(new Favorite { UserId = c.UserId, CurrencyId = c.CurrencyId }, ct);
            return Result<bool>.Ok(true);
        }
    }
}
