using CurrencyExchangeApp.Models;

namespace CurrencyExchangeApp.Repositories
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetAll();
        Task Create(Currency currency);
    }
}
