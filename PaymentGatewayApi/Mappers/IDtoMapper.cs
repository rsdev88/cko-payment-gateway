using PaymentGatewayApi.Models.BankingDTOs;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;

namespace PaymentGatewayApi.Mappers
{
    public interface IDtoMapper
    {
        BankProcessPaymentRequestDto MapProcessPaymentRequestModelToBankDto(ProcessPaymentRequestDto model);

        ProcessPaymentResponse MapBankApiResponseToDomainResponse(BankProcessPaymentResponseDto bankResponseDto);
    }
}
