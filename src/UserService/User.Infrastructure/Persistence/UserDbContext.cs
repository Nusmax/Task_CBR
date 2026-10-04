using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entities;

namespace User.Infrastructure.Persistence
{
    public class UserDbContext(DbContextOptions<UserDbContext> o) : DbContext(o)
    {
        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<Currency> Currencies => Set<Currency>();
        public DbSet<Favorite> Favorites => Set<Favorite>();

        protected override void OnModelCreating(ModelBuilder m)
        {
            m.Entity<AppUser>(e =>
            {
                e.ToTable("user");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Name).HasColumnName("name");
                e.Property(x => x.PasswordHash).HasColumnName("password");
            });
            m.Entity<Currency>(e =>
            {
                e.ToTable("currency"); e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Name).HasColumnName("name");
                e.Property(x => x.Rate).HasColumnName("rate");
            });
            m.Entity<Favorite>(e =>
            {
                e.ToTable("favorite_currency");
                e.HasKey(x => new { x.UserId, x.CurrencyId });
                e.Property(x => x.UserId).HasColumnName("user_id");
                e.Property(x => x.CurrencyId).HasColumnName("currency_id");
            });
        }
    }
}
