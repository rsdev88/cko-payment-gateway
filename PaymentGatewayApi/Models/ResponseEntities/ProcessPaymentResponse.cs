using Newtonsoft.Json;
using System;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;

namespace PaymentGatewayApi.Models.ResponseEntities
{
    public class ProcessPaymentResponse
    {
        [JsonProperty("transactionId")]
        public Guid TransactionId { get; set; }

        [JsonProperty("paymentStatus")]
        public PaymentStatus PaymentStatus { get; set; }
    }
}
