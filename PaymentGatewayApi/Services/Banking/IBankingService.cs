using PaymentGatewayApi.Models.BankingDTOs.v1;
using System;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Services.Banking
{
    public interface IBankingService
    {
        public Task<BankProcessPaymentResponseDto> ProcessPayment(BankProcessPaymentRequestDto bankDto);
        public Task<BankRetrievePaymentsResponseDto> RetrievePayments(BankRetrievePaymentsRequestDto transactionId);
    }
}
