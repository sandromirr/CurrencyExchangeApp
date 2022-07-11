using Microsoft.EntityFrameworkCore;
using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Database;
using CurrencyExchangeApp.Models.ViewModels;

namespace CurrencyExchangeApp.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyExchangeDbContext _dbContext;

        public CurrencyRepository(CurrencyExchangeDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task Create(CreateCurrencyViewModel currencyViewModel)
        {
            var currency = new Currency()
            {
                Code = currencyViewModel.Code,
                Name = currencyViewModel.Name,
                NameEn = currencyViewModel.NameEN
            };

            var currenctExcists = _dbContext.Currency.Where(x => x.Code == currency.Code).Any();

            if (currenctExcists) 
            {
                throw new Exception("Currency Already excists");
            }

            await _dbContext.Currency.AddAsync(currency); 
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Currency>> GetAll()
        {
            return await _dbContext.Currency.ToListAsync();
        }

        public CurrencyRateResultViewModel GetCurrencyRate(CurrencyRateViewModel currencyRateViewModel)
        {
            var currencyFromExcists = _dbContext.Currency.Where(x=>x.Id == currencyRateViewModel.CurrencyFromId).Any();

            if (!currencyFromExcists)
            {
                throw new Exception("Currency from does not excists!");
            }

            var currencyToExcists = _dbContext.Currency.Where(x=>x.Id == currencyRateViewModel.CurrencyToId).Any();

            if (!currencyToExcists)
            {
                throw new Exception("Currency to does not excists!");
            }

            var currencyFrom = _dbContext.Currency.Where(x => x.Id == currencyRateViewModel.CurrencyFromId).First();
            var currencyTo = _dbContext.Currency.Where(x => x.Id == currencyRateViewModel.CurrencyToId).First();

            if (currencyFrom.Id == currencyTo.Id)
            {
                throw new Exception("You can't convert same currency!");
            }

            decimal rate = 0.0m;

            var result = new CurrencyRateResultViewModel()
            {
                CurrencyFrom = currencyFrom,
                CurrencyTo = currencyTo,
            };

            if (IsCurrencyGEL(currencyFrom) && !IsCurrencyGEL(currencyTo))
            {
                var currencyToRate = _dbContext.CurrencyRate.Where(x => x.CurrencyId == currencyTo.Id).First();
                rate = currencyToRate.BuyRate;
            }

            if (!IsCurrencyGEL(currencyFrom) && IsCurrencyGEL(currencyTo)) 
            {
                var currencyFromRate = _dbContext.CurrencyRate.Where(x => x.CurrencyId == currencyFrom.Id).First();
                rate = currencyFromRate.SoldRate;
            }

            if (!IsCurrencyGEL(currencyFrom) && !IsCurrencyGEL(currencyTo)) 
            {
                var currencyFromRate = _dbContext.CurrencyRate.Where(x => x.CurrencyId == currencyFrom.Id).First();
                var currencyToRate = _dbContext.CurrencyRate.Where(x => x.CurrencyId == currencyTo.Id).First();
                rate = decimal.Round(currencyFromRate.BuyRate / currencyToRate.SoldRate, 2, MidpointRounding.AwayFromZero);
            }

            result.Rate = rate;

            return result;
        }

        private bool IsCurrencyGEL(Currency currency) => currency.Id == 1;
    }
}
