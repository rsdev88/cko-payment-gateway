namespace PaymentGatewayApi.Models.Enums
{
    public class PaymentEnums
    {
        public enum SupportedCurrencies
        {
            GBP,
            USD,
            EUR,
            AUD,
            NZD,
            ZAR,
            CAD,
            HKD,
        }

        public enum CardType
        {
            MasterCard,
            Visa,
            AmericanExpress
        }
    }
}
