namespace CurrencyExchangeApp.Models.ViewModels
{
    public class CurrencyExchangeViewModel
    {
        public int CurrencyFromId { get; set; }
        public int CurrencyToId { get; set; }
        public decimal Amount { get; set; }
        //public Account? Account { get; set; }
    }
}
