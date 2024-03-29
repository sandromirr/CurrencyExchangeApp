﻿using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Models.ViewModels;

namespace CurrencyExchangeApp.Repositories
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetAll();
        Task Create(CreateCurrencyViewModel currency);
        Task<CurrencyRateResultViewModel> GetCurrencyRate(CurrencyRateViewModel currencyRateViewModel);
        Task<CurrencyExchangeResultViewModel> ExchangeCurrency(CurrencyExchangeViewModel currencyExchangeViewModel);
        Task CreateCurrencyRate(CreateCurrencyRateviewModel createCurrencyRateViewModel);
    }
}
