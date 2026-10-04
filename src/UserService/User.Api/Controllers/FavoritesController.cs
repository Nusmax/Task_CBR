using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.Application.Commands;

namespace User.Api.Controllers
{
    [ApiController, Route("api/favorites"), Authorize]
    public class FavoritesController(IMediator m) : ControllerBase
    {
        Guid Me => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        [HttpPost("{code}")]
        public async Task<IActionResult> Add(string code)
        { var r = await m.Send(new AddFavoriteCommand(Me, code.ToUpper())); return r.IsSuccess ? Ok() : NotFound(r.Error); }
        [HttpDelete("{code}")]
        public async Task<IActionResult> Remove(string code)
        { await m.Send(new RemoveFavoriteCommand(Me, code.ToUpper())); return Ok(); }
    }
}
