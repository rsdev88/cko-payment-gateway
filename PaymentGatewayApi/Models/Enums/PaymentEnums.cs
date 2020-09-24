namespace PaymentGatewayApi.Models.Enums
{
    public class PaymentEnums
    {
        public enum SupportedCurrencies
        {
            GBP,
            USD,
            EUR,
            CHF,
            SGD,
            AED,
            HKD,
            BRL,
            MUR,
            AUD,
            NZD,
            CAD,
        }

        public enum CardType
        {
            MasterCard,
            Visa,
            AmericanExpress,
        }

        public enum PaymentStatus //Starts at 1 so as not to default to 0 when the paymentStatus field is missing from the Bank API response.
        {
            Success = 1,
            Pending = 2,
            FailedInsufficientFunds = 3,
            FailedIncorrectCardDetails = 4,
            FailedCardFrozen = 5
        }
    }
}
