using Finance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Infrastructure.Persistence
{
    public class FinanceDbContext(DbContextOptions<FinanceDbContext> o) : DbContext(o)
    {
        public DbSet<Currency> Currencies => Set<Currency>();
        public DbSet<FavRow> Favorites => Set<FavRow>();

        protected override void OnModelCreating(ModelBuilder m)
        {
            m.Entity<Currency>(e =>
            {
                e.ToTable("currency"); e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Name).HasColumnName("name");
                e.Property(x => x.Rate).HasColumnName("rate");
            });
            m.Entity<FavRow>(e =>
            {
                e.ToTable("favorite_currency");
                e.HasKey(x => new { x.UserId, x.CurrencyId });
                e.Property(x => x.UserId).HasColumnName("user_id");
                e.Property(x => x.CurrencyId).HasColumnName("currency_id");
            });
        }
    }
}
