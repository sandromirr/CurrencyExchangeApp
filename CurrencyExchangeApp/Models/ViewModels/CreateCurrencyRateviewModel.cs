namespace CurrencyExchangeApp.Models.ViewModels
{
    public class CreateCurrencyRateviewModel
    {
        public int CurrencyId { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SoldRate { get; set; }
    }
}
