using System;

namespace PaymentGatewayApi.Models.BankingDTOs.v1
{
    public class BankRetrievePaymentsRequestDto
    {
        public Guid TransactionId { get; set; }
    }
}
