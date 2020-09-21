using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Models.BankingDTOs;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using System;
using System.Net;

namespace PaymentGatewayApi.Mappers
{
    //Maps the Payment Gateway API DTOs to the Bank API DTOs - useful in case the Bank API ever changes, it won't break the Payment Gateway DTOs (and therefore won't require client changes).
    public class DtoMapper : IDtoMapper
    {
        public BankProcessPaymentRequestDto MapProcessPaymentRequestModelToBankDto(ProcessPaymentRequestDto model)
        {
            if (model == null)
            {
                throw new HttpException(HttpStatusCode.InternalServerError, 
                                        Resources.Resources.ErrorCode_MappingError_PaymentApiToBankApi, 
                                        Resources.Resources.ErrorMessage_MappingError_PaymentApiToBankApi);
            }

            var bankDto = new BankProcessPaymentRequestDto()
            {
                CardNumber = model.CardNumber,
                CardHolder = model.CardHolder,
                CardType = model.CardType,
                ExpirationMonth = model.ExpirationMonth,
                ExpirationYear = model.ExpirationYear,
                PaymentAmount = model.PaymentAmount,
                Currency = model.Currency,
                Cvv = model.Cvv
            };

            return bankDto;
        }

        public ProcessPaymentResponse MapBankApiResponseToDomainResponse(BankProcessPaymentResponseDto bankResponseDto)
        {
            if (bankResponseDto == null)
            {
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        Resources.Resources.ErrorCode_MappingError_BankApiToPaymentApi,
                                        Resources.Resources.ErrorMessage_MappingError_BankApiToPaymentApi);
            }

            var processPaymentResponse = new ProcessPaymentResponse()
            {
                TransactionId = new Guid(bankResponseDto.TransactionId)
            };

            return processPaymentResponse;
        }
    }
}
