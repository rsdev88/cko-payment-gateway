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

        public enum PaymentStatus
        {
            Success,
            Pending,
            FailedInsufficientFunds,
            FailedIncorrectCardDetails,
            FailedCardFrozen
        }
    }
}
