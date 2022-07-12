using CurrencyExchangeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeApp.Database
{
    public class CurrencyExchangeDbContext : DbContext
    {
        public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options) : base(options) { }

        public DbSet<Currency> Currency { get; set; }
        public DbSet<CurrencyRate> CurrencyRate { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<CurrencyExchange> CurrencyExchange { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}