using Finance.Domain.Entities;
using MediatR;
using SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Application.Queries
{
    public record GetFavoritesQuery(Guid UserId) : IRequest<Result<List<Currency>>>;
}
