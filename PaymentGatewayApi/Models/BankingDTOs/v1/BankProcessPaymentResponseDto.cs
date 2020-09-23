using Newtonsoft.Json;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;

namespace PaymentGatewayApi.Models.BankingDTOs.v1
{
    public class BankProcessPaymentResponseDto
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("paymentStatus")]
        public PaymentStatus PaymentStatus { get; set; }
    }
}
