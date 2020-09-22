using PaymentGatewayApi.Models.BankingDTOs;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Services.Banking
{
    public interface IBankingService
    {
        public Task<BankProcessPaymentResponseDto> ProcessPayment(BankProcessPaymentRequestDto bankDto);
    }
}
