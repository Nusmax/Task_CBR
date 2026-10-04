using MediatR;
using SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Commands
{
    public record RegisterUserCommand(string Name, string Password) : IRequest<Result<Guid>>;
}
