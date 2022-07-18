using CurrencyExchangeApp.Repositories;

namespace CurrencyExchangeApp.Database
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CurrencyExchangeDbContext _context;

        public UnitOfWork(CurrencyExchangeDbContext _context)
        {
            this._context = _context;

            Account = new AccountRepository(_context);
            Currency = new CurrencyRepository(_context);
        }

        public IAccountRepository Account { get; private set; }

        public ICurrencyRepository Currency { get; private set; }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
