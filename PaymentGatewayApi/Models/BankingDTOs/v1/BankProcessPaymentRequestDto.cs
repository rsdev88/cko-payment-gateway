using Newtonsoft.Json;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;

namespace PaymentGatewayApi.Models.BankingDTOs.v1
{
    public class BankProcessPaymentRequestDto
    {
        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("cardHolder")]
        public string CardHolder { get; set; }

        [JsonProperty("cardType")]
        public CardType? CardType { get; set; }

        [JsonProperty("expirationMonth")]
        public string ExpirationMonth { get; set; }

        [JsonProperty("expirationYear")]
        public string ExpirationYear { get; set; }

        [JsonProperty("paymentAmount")]
        public decimal? PaymentAmount { get; set; }

        [JsonProperty("currency")]
        public SupportedCurrencies? Currency { get; set; }

        [JsonProperty("cvv")]
        public string Cvv { get; set; }
    }
}
