namespace CurrencyExchangeApp.Models.Exceptions
{
    public class CurrencyExchangeException : Exception
    {
        public CurrencyExhangeExceptionEnum currencyExhangeExceptionEnum;

        public CurrencyExchangeException(string message, CurrencyExhangeExceptionEnum currencyExhangeExceptionEnum) : base(message)
        {
            this.currencyExhangeExceptionEnum = currencyExhangeExceptionEnum;
        }
    }

    public enum CurrencyExhangeExceptionEnum 
    {
        CurrencyDoesNotExists = 1,
        AnnonymousExchangeAmountExceeded = 2,
        CurrencyExchangeDailyLimitExceeded = 3,
        CanNotConvertSameCurrencies = 4,

        AccountExists = 5,
        NotFoundRecomderAccount = 6,
    }
}
