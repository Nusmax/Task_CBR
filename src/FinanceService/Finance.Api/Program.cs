using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Finance.Application.Queries;
using Finance.Domain.Abstractions;
using Finance.Infrastructure.Persistence;
using Finance.Infrastructure.Repositories;

var b = WebApplication.CreateBuilder(args);
b.Services.AddDbContext<FinanceDbContext>(o => o.UseNpgsql(b.Configuration.GetConnectionString("Default")));
b.Services.AddScoped<IFinanceRepository, EfFinanceRepository>();
b.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<GetFavoritesQuery>());
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
app.UseAuthentication(); app.UseAuthorization(); app.MapControllers();
app.Run();