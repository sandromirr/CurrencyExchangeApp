using Microsoft.EntityFrameworkCore;
using CurrencyExchangeApp.Database;
using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Models.ViewModels;
using CurrencyExchangeApp.Models.Exceptions;

namespace CurrencyExchangeApp.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CurrencyExchangeDbContext _dbContext;
        
        public AccountRepository(CurrencyExchangeDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<bool> AccountExcists(string personalNumber)
        {
            return await _dbContext.Account.Where(x => x.PersonalNumber == personalNumber).AnyAsync();
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

            var accontExists = _dbContext.Account.Where(x => x.PersonalNumber == createAccountViewModel.PersonalNumber).Any();

            if (accontExists) 
            {
                string messageText = $"Account with personal number {account.PersonalNumber} already created account";
                throw new CurrencyExchangeException(messageText, CurrencyExhangeExceptionEnum.AccountExists);
            }

            var recommenderAccount = _dbContext.Account.Where(x => x.PersonalNumber == account.RecommenderNumber).Any();

            if (!recommenderAccount && account.PersonalNumber != account.RecommenderNumber)
            {
                string messageText = $"There is not recommender account";
                throw new CurrencyExchangeException(messageText, CurrencyExhangeExceptionEnum.NotFoundRecomderAccount);
            }

            await _dbContext.Account.AddAsync(account);
        }

        public async Task<Account?> GetAccountByPersonalNumber(string personalNumber)
        {
            var accountExists = await _dbContext.Account.Where(x => x.PersonalNumber == personalNumber).AnyAsync();

            if (!accountExists)
            {
                throw new CurrencyExchangeException($"Account does not exists", CurrencyExhangeExceptionEnum.AccountDoesNotExists);
            }

            return await _dbContext.Account.Where(x => x.PersonalNumber == personalNumber).FirstOrDefaultAsync();
        }

        public async Task<List<AccountReport>> GetAccountReports(AccountReportFilter accountReportFilter)
        {
            var reports = new List<AccountReport>();
            var accounts = await _dbContext.Account.ToListAsync();

            if (!accountReportFilter.From.HasValue)
            { 
                accountReportFilter.From = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }

            if (!accountReportFilter.To.HasValue)
            {
                accountReportFilter.To = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;
            }

            foreach (var account in accounts)
            {
                var report = new AccountReport();
                report.Account = account;

                var hirerchyAccounts = _dbContext.Account.Where(x => x.RecommenderNumber == account.PersonalNumber || x.Id == account.Id)
                                                         .Select(x => x.Id)
                                                         .ToHashSet();

                var transactions = _dbContext.CurrencyExchange.Where(x => 
                                            hirerchyAccounts.Contains(x.AccountId) && 
                                            x.TransactionDate >= accountReportFilter.From && 
                                            x.TransactionDate <= accountReportFilter.To);

                var list = transactions.ToList();

                report.HirerchyConvertionCount = await transactions.CountAsync();
                report.PersonalConvertionCount = await transactions.Where(x => x.Id == account.Id).CountAsync();

                reports.Add(report);
            }

            return reports; 
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _dbContext.Account.ToListAsync();
        }
    }
}
