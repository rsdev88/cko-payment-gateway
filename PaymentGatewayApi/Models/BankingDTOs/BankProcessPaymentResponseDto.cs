using Newtonsoft.Json;

namespace PaymentGatewayApi.Models.BankingDTOs
{
    public class BankProcessPaymentResponseDto
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
    }
}
