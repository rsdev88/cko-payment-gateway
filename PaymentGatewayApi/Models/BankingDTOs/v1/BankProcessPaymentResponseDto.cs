using Newtonsoft.Json;

namespace PaymentGatewayApi.Models.BankingDTOs.v1
{
    public class BankProcessPaymentResponseDto
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
    }
}
