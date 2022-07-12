using CurrencyExchangeApp.Models;

namespace CurrencyExchangeApp.Repositories
{
    public interface ICurrencyExchangeRepository
    {
        Task<IEnumerable<CurrencyExchange>> GetCurrencyExchanges();
        Task AddCurrencyExchanges(CurrencyExchange currencyExchange);
    }
}
