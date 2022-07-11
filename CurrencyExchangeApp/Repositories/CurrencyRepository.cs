using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Database;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeApp.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyExchangeDbContext _dbContext;

        public CurrencyRepository(CurrencyExchangeDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task Create(Currency currency)
        {
            var result = _dbContext.Currency.AddAsync(currency);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Currency>> GetAll()
        {
            return await _dbContext.Currency.ToListAsync();
        }
    }
}
