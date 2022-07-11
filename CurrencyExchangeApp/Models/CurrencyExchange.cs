namespace CurrencyExchangeApp.Models
{
    public class CurrencyExchange
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int CurrencyFromId { get; set; }
        public int CurrencyToId { get; set; }
        public decimal Amount { get; set; }
    }
}
