using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;

namespace PaymentGatewayApi.Models.BankingDTOs.v1
{
    public class BankRetrievePaymentsResponseDto
    {
        [JsonProperty("payments")]
        public List<BankRetrievedPaymentDetails> Payments { get; set; }
    }
    public class BankRetrievedPaymentDetails
    {
        [JsonProperty("paymentStatus")]
        public PaymentStatus PaymentStatus { get; set; }

        [JsonProperty("paymentDateTime")]
        public DateTime PaymentDateTime { get; set; }

        [JsonProperty("paymentAmount")]
        public decimal PaymentAmount { get; set; }

        [JsonProperty("currency")]
        public SupportedCurrencies Currency { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("cardHolder")]
        public string CardHolder { get; set; }

        [JsonProperty("cardType")]
        public CardType CardType { get; set; }

        [JsonProperty("expirationMonth")]
        public string ExpirationMonth { get; set; }

        [JsonProperty("expirationYear")]
        public string ExpirationYear { get; set; }
    }
}
