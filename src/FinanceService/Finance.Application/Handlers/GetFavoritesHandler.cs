using Finance.Application.Queries;
using Finance.Domain.Abstractions;
using Finance.Domain.Entities;
using MediatR;
using SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Application.Handlers
{
    public class GetFavoritesHandler(IFinanceRepository repo)
    : IRequestHandler<GetFavoritesQuery, Result<List<Currency>>>
    {
        public async Task<Result<List<Currency>>> Handle(GetFavoritesQuery q, CancellationToken ct)
            => Result<List<Currency>>.Ok(await repo.GetFavoritesAsync(q.UserId, ct));
    }

}
