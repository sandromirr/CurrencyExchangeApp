using CurrencyExchangeApp.Database;
using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeApp.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CurrencyExchangeDbContext _dbContext;

        public AccountRepository(CurrencyExchangeDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public Task<bool> AccountExcists(string personalNumber)
        {
            var account = _dbContext.Account.Where(x => x.PersonalNumber == personalNumber).AnyAsync();
            return account;
        }

        public async Task Create(CreateAccountViewModel createAccountViewModel)
        {
            var account = new Account()
            {
                Name = createAccountViewModel.Name,
                Surname = createAccountViewModel.Surname,
                PersonalNumber = createAccountViewModel.PersonalNumber,
                RecommenderNumber = createAccountViewModel.RecommenderNumber
            };
            
            await _dbContext.Account.AddAsync(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Account?> GetAccountByPersonalNumber(string personalNumber)
        {
            var accountExcists = await _dbContext.Account.Where(x => x.PersonalNumber == personalNumber).AnyAsync();

            if (!accountExcists)
            {
                throw new Exception("Account does not excists");
            }

            var account = await _dbContext.Account.Where(x => x.PersonalNumber == personalNumber).FirstOrDefaultAsync();

            return account;
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            var accounts = await _dbContext.Account.ToListAsync();
            return accounts;
        }
    }
}
