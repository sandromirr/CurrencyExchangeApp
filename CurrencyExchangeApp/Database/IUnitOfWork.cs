using CurrencyExchangeApp.Repositories;

namespace CurrencyExchangeApp.Database
{
    public interface IUnitOfWork
    {
        IAccountRepository Account { get; }
        ICurrencyRepository Currency { get; }
        Task CompleteAsync();
        void Dispose();
    }
}
