using MediatR;
using SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Queries
{
    public record LoginUserQuery(string Name, string Password) : IRequest<Result<string>>;
}
