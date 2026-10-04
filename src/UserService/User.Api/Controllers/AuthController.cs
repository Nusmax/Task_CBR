using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application.Commands;
using User.Application.Queries;

namespace User.Api.Controllers
{
    [ApiController, Route("api/auth")]
    public class AuthController(IMediator m) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand c)
        { var r = await m.Send(c); return r.IsSuccess ? Ok(new { id = r.Value }) : Conflict(r.Error); }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserQuery q)
        { var r = await m.Send(q); return r.IsSuccess ? Ok(new { token = r.Value }) : Unauthorized(r.Error); }

        [Authorize, HttpPost("logout")] 
        public IActionResult Logout() => Ok();
    }
}
