using CurrencyExchangeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeApp.Database
{
    public class CurrencyExchangeDbContext : DbContext
    {
        public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options) : base(options)
        {
        }

        public DbSet<Currency> Currency { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}