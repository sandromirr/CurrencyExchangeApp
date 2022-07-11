namespace CurrencyExchangeApp.Models.ViewModels
{
    public class CurrencyRateResultViewModel
    {
        public Currency? CurrencyFrom { get; set; }
        public Currency? CurrencyTo { get; set; }
        public decimal Rate { get; set; }
    }
}
