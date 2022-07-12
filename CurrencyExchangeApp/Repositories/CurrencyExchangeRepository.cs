using CurrencyExchangeApp.Database;
using CurrencyExchangeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeApp.Repositories
{
    public class CurrencyExchangeRepository : ICurrencyExchangeRepository
    {
        private readonly CurrencyExchangeDbContext _dbContext;

        public CurrencyExchangeRepository(CurrencyExchangeDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }


        public async Task AddCurrencyExchanges(CurrencyExchange currencyExchange)
        {
            await _dbContext.CurrencyExchange.AddAsync(currencyExchange);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CurrencyExchange>> GetCurrencyExchanges()
        {
            var currencyExchanges = await _dbContext.CurrencyExchange.ToListAsync();
            return currencyExchanges;
        }
    }
}
