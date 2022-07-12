namespace CurrencyExchangeApp.Models.ViewModels
{
    public class CurrencyExchangeResultViewModel
    {
        public Currency CurrencyFrom { get; set; }
        public Currency CurrencyTo { get; set; }
        public decimal Amount { get; set; }
    }
}
