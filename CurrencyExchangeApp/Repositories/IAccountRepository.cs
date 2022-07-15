using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Models.ViewModels;

namespace CurrencyExchangeApp.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccounts();
        Task Create(CreateAccountViewModel account);
        Task<bool> AccountExcists(string personalNumber);
        Task<Account?> GetAccountByPersonalNumber(string personalNumber);
        Task<List<AccountReport>> GetAccountReports(AccountReportFilter accountReportFilter);
    }
}
