using Microsoft.EntityFrameworkCore;
using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Database;
using CurrencyExchangeApp.Models.ViewModels;
using CurrencyExchangeApp.Models.Exceptions;

namespace CurrencyExchangeApp.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyExchangeDbContext _dbContext;
        
        public CurrencyRepository(CurrencyExchangeDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<IEnumerable<Currency>> GetAll()
        {
            return await _dbContext.Currency.ToListAsync();
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
                throw new CurrencyExchangeException($"Currency code = {currency.Code} Already excists", CurrencyExhangeExceptionEnum.CurrencyExists);
            }

            await _dbContext.Currency.AddAsync(currency);
        }

        public async Task<CurrencyExchangeResultViewModel> ExchangeCurrency(CurrencyExchangeViewModel currencyExchangeViewModel)
        {
            await ValidateCurrencyCouple(currencyExchangeViewModel.CurrencyFromId, currencyExchangeViewModel.CurrencyToId);

            var currencyFrom = await _dbContext.Currency.Where(x => x.Id == currencyExchangeViewModel.CurrencyFromId).FirstAsync();
            var currencyTo = await _dbContext.Currency.Where(x => x.Id == currencyExchangeViewModel.CurrencyToId).FirstAsync();

            Account? account = currencyExchangeViewModel.Account;
            
            if (account != null) {
                var findAccount = _dbContext.Account.Where(x => x.PersonalNumber == account.PersonalNumber);

                if (!findAccount.Any())
                {
                    await _dbContext.Account.AddAsync(account);
                    await _dbContext.SaveChangesAsync();
                }
                else 
                {
                    account = await findAccount.FirstAsync();
                }
            }

            var currencyFromInGel = ConvertCurrencyToGel(currencyFrom, currencyExchangeViewModel.Amount);
            await ValidateCurrencyExchange(account, currencyFromInGel);

            decimal convertedCurrencyAmount = await ConvertCurrency(currencyFrom, currencyTo, currencyExchangeViewModel.Amount);

            var result = new CurrencyExchangeResultViewModel()
            {
                CurrencyFrom = currencyFrom,
                CurrencyTo = currencyTo,
                Amount = convertedCurrencyAmount
            };

            var exchangeModel = new CurrencyExchange()
            {
                CurrencyFromId = currencyFrom.Id,
                CurrencyToId = currencyTo.Id,
                Amount = currencyFromInGel,
                TransactionDate = DateTime.Now,
                AccountId = (account != null) ? account.Id : -1
            };

            await _dbContext.CurrencyExchange.AddAsync(exchangeModel);

            return result;
        }

        public async Task<CurrencyRateResultViewModel> GetCurrencyRate(CurrencyRateViewModel currencyRateViewModel)
        {
            await ValidateCurrencyCouple(currencyRateViewModel.CurrencyFromId, currencyRateViewModel.CurrencyToId);

            decimal rate = await CurrencyRateBetweenTwoCurrencies(currencyRateViewModel.CurrencyFromId, currencyRateViewModel.CurrencyToId);

            var currencyFrom = await _dbContext.Currency.Where(x => x.Id == currencyRateViewModel.CurrencyFromId).FirstAsync();
            var currencyTo = await _dbContext.Currency.Where(x => x.Id == currencyRateViewModel.CurrencyToId).FirstAsync();

            var currencyRateResultModel = new CurrencyRateResultViewModel()
            {
                CurrencyFrom = currencyFrom,
                CurrencyTo = currencyTo,
                Rate = rate
            };

            return currencyRateResultModel;
        }


        private bool IsCurrencyGEL(Currency currency) => currency.Code == "GEL";
        
        private decimal ConvertCurrencyToGel(Currency currency, decimal amount)
        {
            decimal amountInGel = 0.0m;

            if (amount < 0) 
            {
                string messageText = $"Exchange Currency Amount must be positive.";
                throw new CurrencyExchangeException(messageText, CurrencyExhangeExceptionEnum.CurrencyExchangeBalanceIsNegative);
            }

            if (IsCurrencyGEL(currency))
            {
                return amount;
            }

            var currencyRateList = _dbContext.CurrencyRate.Where(x => x.CurrencyId == currency.Id);

            if (!currencyRateList.Any())
            {
                string messageText = $"Currency code {currency.Code} does not excists!";
                throw new CurrencyExchangeException(messageText, CurrencyExhangeExceptionEnum.CurrencyDoesNotExists);
            }

            var currencyRate = currencyRateList.First();

            amountInGel = amount * currencyRate.BuyRate;

            return amountInGel;
        }

        private async Task<decimal> CurrencyRateBetweenTwoCurrencies(int currencyFromId, int currencyToId)
        {
            decimal rate = 0.0m;

            var currencyFrom = _dbContext.Currency.Where(x => x.Id == currencyFromId).First();
            var currencyTo = _dbContext.Currency.Where(x => x.Id == currencyToId).First();

            if (IsCurrencyGEL(currencyFrom) && !IsCurrencyGEL(currencyTo))
            {
                var currencyToRate = await _dbContext.CurrencyRate.Where(x => x.CurrencyId == currencyTo.Id).FirstAsync();
                rate = decimal.Round(1.0m / currencyToRate.SoldRate, 2, MidpointRounding.AwayFromZero);
            }

            if (!IsCurrencyGEL(currencyFrom) && IsCurrencyGEL(currencyTo))
            {
                var currencyFromRate = await _dbContext.CurrencyRate.Where(x => x.CurrencyId == currencyFrom.Id).FirstAsync();
                rate = currencyFromRate.BuyRate;
            }

            if (!IsCurrencyGEL(currencyFrom) && !IsCurrencyGEL(currencyTo))
            {
                var currencyFromRate = await _dbContext.CurrencyRate.Where(x => x.CurrencyId == currencyFrom.Id).FirstAsync();
                var currencyToRate = await _dbContext.CurrencyRate.Where(x => x.CurrencyId == currencyTo.Id).FirstAsync();
                rate = decimal.Round(currencyFromRate.BuyRate / currencyToRate.SoldRate, 2, MidpointRounding.AwayFromZero);
            }

            return rate;
        }

        private async Task<decimal> ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal currencyAmount)
        {
            var currenctRateModel = new CurrencyRateViewModel()
            {
                CurrencyFromId = currencyFrom.Id,
                CurrencyToId = currencyTo.Id
            };

            var currencyRate = await GetCurrencyRate(currenctRateModel);

            decimal rate = currencyRate.Rate;

            return currencyAmount * rate;
        }

        private async Task<decimal> CalculateAccountDailyExchangeAmount(Account account)
        {
            decimal amount = 0.0m;

            var accountIdSet = _dbContext.Account.Where(x => x.Id == account.Id || x.RecommenderNumber == account.PersonalNumber)
                                                .Select(x => x.Id)
                                                .ToHashSet();

            amount = await _dbContext.CurrencyExchange.Where(x => accountIdSet.Contains(x.AccountId) && x.TransactionDate >= DateTime.Now.AddDays(-1))
                                                .Select(x => x.Amount)
                                                .SumAsync();
            return amount;
        }

        private async Task ValidateCurrencyExchange(Account? account, decimal currencyFromInGel)
        {
            const decimal MaxCurrencyExchangeAmountUnRegisteredAccount = 3_000m;
            const decimal MaxCurrencyExchangeDailyLimit = 100_000m;

            if (currencyFromInGel > MaxCurrencyExchangeAmountUnRegisteredAccount && account == null)
            {
                string messageText = $"Exchange limit exceeded {MaxCurrencyExchangeAmountUnRegisteredAccount} GEL. Please proceed to enter your account details.";
                throw new CurrencyExchangeException(messageText, CurrencyExhangeExceptionEnum.AnnonymousExchangeAmountExceeded);
            }

            if (account != null)
            {
                decimal accountDailyExchangeAmount = await CalculateAccountDailyExchangeAmount(account);

                if (accountDailyExchangeAmount + currencyFromInGel > MaxCurrencyExchangeDailyLimit)
                {
                    string messageText = $"Exchange daily limit exceeded {MaxCurrencyExchangeDailyLimit} GEL";
                    throw new CurrencyExchangeException(messageText, CurrencyExhangeExceptionEnum.CurrencyExchangeDailyLimitExceeded);
                }
            }
        }

        private async Task ValidateCurrency(int currencyId, string fieldName) 
        {
            var currency = await _dbContext.Currency.Where(x => x.Id == currencyId).AnyAsync();
            if (!currency)
            {
                string messageText = $"{fieldName} select currency!";
                throw new CurrencyExchangeException(messageText, CurrencyExhangeExceptionEnum.InvalidFieldValue);
            }
        }

        private async Task ValidateCurrencyCouple(int currencyFromId, int currencyToId) 
        {
            await ValidateCurrency(currencyFromId, nameof(currencyFromId));
            await ValidateCurrency(currencyToId, nameof(currencyToId));

            if (currencyToId == currencyFromId)
            {
                throw new CurrencyExchangeException("You can't convert two same currencies!", CurrencyExhangeExceptionEnum.CanNotConvertSameCurrencies);
            }
        }
    }
}
