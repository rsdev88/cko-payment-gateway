using NUnit.Framework;
using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Mappers;
using PaymentGatewayApi.Models.BankingDTOs;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using System;
using System.Net;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;
using PaymentGatewayApi.Resources;

namespace PaymentGatewayApiTests.Mappers
{
    [TestFixture]
    public class DtoMapperTests
    {
        private DtoMapper _mapper;

        [SetUp]
        public void DtoMapperTests_Setup()
        {
            this._mapper = new DtoMapper();
        }

        [Test]
        public void SuccessfullyMapsPaymentRequestModelToBankingApiRequestDto()
        {
            //Arrange
            var model = new ProcessPaymentRequestDto()
            {
                CardNumber = "5500000000000004",
                CardHolder = "Test Account",
                CardType = CardType.MasterCard,
                ExpirationMonth = DateTime.Now.ToString("MM"),
                ExpirationYear = DateTime.Now.AddYears(1).ToString("yy"),
                PaymentAmount = 100.00M,
                Currency = SupportedCurrencies.GBP,
                Cvv = "123"
            };

            //Act
            var result = this._mapper.MapProcessPaymentRequestModelToBankDto(model);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BankProcessPaymentRequestDto>(result);
            Assert.AreEqual(result.CardNumber, model.CardNumber);
            Assert.AreEqual(result.CardHolder, model.CardHolder);
            Assert.AreEqual(result.CardType, model.CardType);
            Assert.AreEqual(result.ExpirationMonth, model.ExpirationMonth);
            Assert.AreEqual(result.ExpirationYear, model.ExpirationYear);
            Assert.AreEqual(result.PaymentAmount, model.PaymentAmount);
            Assert.AreEqual(result.Currency, model.Currency);
            Assert.AreEqual(result.Cvv, model.Cvv);
        }

        [Test]
        public void MappingPaymentRequestModelToBankingApiRequestDtoThrowsErrorIfModelIsNull()
        {
            //Arrange - none needed.
            //Act - see assertion.

            //Assert
            var ex = Assert.Throws<HttpException>(() => this._mapper.MapProcessPaymentRequestModelToBankDto(null));
            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_MappingError_PaymentApiToBankApi, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_MappingError_PaymentApiToBankApi, ex.ErrorCode);
        }

        [Test]
        public void SuccessfullyMapsBankingApiResponseDtoToPaymentApiResponse()
        {
            //Arrange
            var transactionId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
            var bankingApiResponseDto = new BankProcessPaymentResponseDto()
            {
                TransactionId = transactionId
            };

            //Act
            var result = this._mapper.MapBankApiResponseToDomainResponse(bankingApiResponseDto);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ProcessPaymentResponse>(result);
            Assert.AreEqual(result.TransactionId, new Guid(transactionId));
        }

        [Test]
        public void MappingBankingApiResponseDtoToPaymentResponseThrowsErrorIfDtoIsNull()
        {
            //Arrange - none needed.
            //Act - see assertion.

            //Assert
            var ex = Assert.Throws<HttpException>(() => this._mapper.MapBankApiResponseToDomainResponse(null));
            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_MappingError_BankApiToPaymentApi, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_MappingError_BankApiToPaymentApi, ex.ErrorCode);
        }
    }
}
