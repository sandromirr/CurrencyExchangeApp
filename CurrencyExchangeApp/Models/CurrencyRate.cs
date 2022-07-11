namespace CurrencyExchangeApp.Models
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SoldRate { get; set; }
    }
}
