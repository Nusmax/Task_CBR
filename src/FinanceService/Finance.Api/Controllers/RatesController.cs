using Finance.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Finance.Api.Controllers
{
    [ApiController, Route("api/finance"), Authorize]
    public class RatesController(IMediator m) : ControllerBase
    {
        [HttpGet("rates")]
        public async Task<IActionResult> Get()
        {
            var uid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var r = await m.Send(new GetFavoritesQuery(uid));
            return Ok(r.Value);
        }
    }
}
