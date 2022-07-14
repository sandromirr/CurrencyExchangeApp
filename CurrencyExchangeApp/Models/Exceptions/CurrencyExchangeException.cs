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
        AnnonymousExchangeAmountExceeded = 1,
        CurrencyExchangeDailyLimitExceeded = 2,


        CurrencyDoesNotExists = 3,
        CanNotConvertSameCurrencies = 4,
        CurrencyExchangeBalanceIsNegative = 5,

        AccountExists = 6,
        NotFoundRecomderAccount = 7,
    }
}
