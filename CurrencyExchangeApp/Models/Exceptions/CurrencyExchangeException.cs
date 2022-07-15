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
        CurrencyExists = 6,

        AccountExists = 7,
        NotFoundRecomderAccount = 8,
        AccountDoesNotExists = 9,

        InvalidFieldValue = 10,
    }
}
