using PaymentGatewayApi.Models.BankingDTOs.v1;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using System.Collections.Generic;

namespace PaymentGatewayApi.Mappers
{
    public interface IDtoMapper
    {
        BankProcessPaymentRequestDto MapProcessPaymentRequestModelToBankDto(ProcessPaymentRequestDto model);

        BankRetrievePaymentsRequestDto MapRetrievePaymentsRequestModelToBankDto(RetrievePaymentsRequestDto model);

        ProcessPaymentResponse MapBankApiPostResponseToDomainResponse(BankProcessPaymentResponseDto bankResponseDto);

        RetrievePaymentsResponse MapBankApiGetResponseToDomainResponse(BankRetrievePaymentsResponseDto bankResponseDto);
    }
}
