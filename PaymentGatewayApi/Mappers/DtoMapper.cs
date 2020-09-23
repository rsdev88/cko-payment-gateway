﻿using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Models.BankingDTOs.v1;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using PaymentGatewayApi.Services.Banking;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace PaymentGatewayApi.Mappers
{
    /// <summary>
    /// Maps the Payment Gateway API DTOs to the Bank API DTOs - useful in case the Bank API ever changes, it won't break the Payment Gateway DTOs (and therefore won't require client changes).
    /// </summary>
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

        public BankRetrievePaymentsRequestDto MapRetrievePaymentsRequestModelToBankDto(RetrievePaymentsRequestDto model)
        {
            if (model == null)
            {
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        Resources.Resources.ErrorCode_MappingError_PaymentApiToBankApi,
                                        Resources.Resources.ErrorMessage_MappingError_PaymentApiToBankApi);
            }

            var bankDto = new BankRetrievePaymentsRequestDto()
            {
                TransactionId = model.TransactionId.Value
            };

            return bankDto;
        }

        public ProcessPaymentResponse MapBankApiPostResponseToDomainResponse(BankProcessPaymentResponseDto bankResponseDto)
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

        public RetrievePaymentsResponse MapBankApiGetResponseToDomainResponse(BankRetrievePaymentsResponseDto bankResponseDto)
        {
            if (bankResponseDto == null || bankResponseDto.Payments == null)
            {
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        Resources.Resources.ErrorCode_MappingError_BankApiToPaymentApi,
                                        Resources.Resources.ErrorMessage_MappingError_BankApiToPaymentApi);
            }

            var retrievePaymentResponse = new RetrievePaymentsResponse()
            {
                Payments = bankResponseDto.Payments.Select(payment => new RetrievedPaymentDetails()
                {
                    PaymentStatus = payment.PaymentStatus,
                    PaymentDateTime = payment.PaymentDateTime,
                    PaymentAmount = payment.PaymentAmount,
                    Currency = payment.Currency,
                    CardNumber = this.MaskString(payment.CardNumber, 4),
                    CardHolder = payment.CardHolder,
                    CardType = payment.CardType,
                    ExpirationMonth = payment.ExpirationMonth,
                    ExpirationYear = payment.ExpirationYear
                }).ToList()   
            };

            return retrievePaymentResponse;
        }

        //Replaces all characters in a string with 'X', except for the final specified number of digits.
        //e.g.  For a card number
        //      MaskString("5500000000000004", 4) 
        //      Returns "XXXXXXXXXXXX0004"
        public string MaskString(string value, int numberOfCharactersToLeaveUnchanged)
        {
            var newCardNumber = new StringBuilder();

            for(int i = 0; i < value.Length; i++)
            {
                if(i >= value.Length - numberOfCharactersToLeaveUnchanged)
                {
                    newCardNumber.Append(value[i]);
                }
                else
                {
                    newCardNumber.Append("X");
                }
            }

            return newCardNumber.ToString();
        }
    }
}
