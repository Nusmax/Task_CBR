using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User.Application.Commands;
using User.Domain.Abstractions;
using User.Infrastructure.Persistence;
using User.Infrastructure.Repositories;
using User.Infrastructure.Security;

var b = WebApplication.CreateBuilder(args);
b.Services.Configure<SharedKernel.Common.JwtOptions>(b.Configuration.GetSection("Jwt"));
b.Services.AddDbContext<UserDbContext>(o => o.UseNpgsql(b.Configuration.GetConnectionString("Default")));
b.Services.AddScoped<IUserRepository, EfUserRepository>();
b.Services.AddScoped<IFavoriteRepository, EfFavoriteRepository>();
b.Services.AddScoped<IPasswordHasher, BCryptHasher>();
b.Services.AddScoped<IJwtProvider, JwtProvider>();
b.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<RegisterUserCommand>());
b.Services.AddControllers();
var jwt = b.Configuration.GetSection("Jwt").Get<SharedKernel.Common.JwtOptions>() ?? new();
b.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
        ValidateIssuer = true,
        ValidIssuer = jwt.Issuer,
        ValidateAudience = true,
        ValidAudience = jwt.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
var app = b.Build();
app.UseAuthentication(); 
app.UseAuthorization(); 
app.MapControllers();
app.Run();

