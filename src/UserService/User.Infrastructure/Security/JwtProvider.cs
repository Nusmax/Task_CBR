using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Abstractions;
using User.Domain.Entities;

namespace User.Infrastructure.Security
{
    public class JwtProvider(IOptions<JwtOptions> o) : IJwtProvider
    {
        public string Generate(AppUser u)
        {
            var opt = o.Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()), new Claim(ClaimTypes.Name, u.Name) };
            var t = new JwtSecurityToken(opt.Issuer, opt.Audience, claims, expires: DateTime.UtcNow.AddMinutes(opt.ExpiryMinutes), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(t);
        }
    }
}
