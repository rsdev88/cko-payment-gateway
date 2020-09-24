using NUnit.Framework;
using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Mappers;
using PaymentGatewayApi.Models.BankingDTOs.v1;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using System;
using System.Net;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;
using PaymentGatewayApi.Resources;
using System.Collections.Generic;
using Moq;
using Microsoft.Extensions.Logging;

namespace PaymentGatewayApiTests.Mappers
{
    [TestFixture]
    public class DtoMapperTests
    {
        private DtoMapper _mapper;
        private Mock<ILogger<DtoMapper>> _logger;

        [SetUp]
        public void DtoMapperTests_Setup()
        {
            this._logger = new Mock<ILogger<DtoMapper>>();
            this._mapper = new DtoMapper(_logger.Object);
        }

        [Test]
        public void SuccessfullyMapsPaymentProcessingRequestModelToBankingApiRequestDto()
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
        public void MappingPaymentProcessRequestModelToBankingApiRequestDtoThrowsErrorIfModelIsNull()
        {
            //Arrange - none needed.
            //Act - see assertion.

            //Assert
            var ex = Assert.Throws<HttpException>(() => this._mapper.MapProcessPaymentRequestModelToBankDto(null));
            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_MappingError_PaymentApiToBankApi, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_MappingError_PaymentApiToBankApi, ex.ErrorCode);

            //Verify logging took place
            this._logger.Verify(x => x.Log(LogLevel.Error,
                                            It.IsAny<EventId>(),
                                            It.IsAny<It.IsAnyType>(),
                                            It.IsAny<Exception>(),
                                            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Test]
        public void SuccessfullyMapsBankingApiResponseDtoToProcessPaymentApiResponse()
        {
            //Arrange
            var transactionId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
            var bankingApiResponseDto = new BankProcessPaymentResponseDto()
            {
                TransactionId = transactionId,
                PaymentStatus = PaymentStatus.Success
            };

            //Act
            var result = this._mapper.MapBankApiPostResponseToDomainResponse(bankingApiResponseDto);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ProcessPaymentResponse>(result);
            Assert.AreEqual(new Guid(transactionId), result.TransactionId);
            Assert.AreEqual(PaymentStatus.Success, result.PaymentStatus);
        }

        [Test]
        public void MappingBankingApiResponseDtoToProcessPaymentResponseThrowsErrorIfDtoIsNull()
        {
            //Arrange - none needed.
            //Act - see assertion.

            //Assert
            var ex = Assert.Throws<HttpException>(() => this._mapper.MapBankApiPostResponseToDomainResponse(null));
            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_MappingError_BankApiToPaymentApi, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_MappingError_BankApiToPaymentApi, ex.ErrorCode);

            //Verify logging took place
            this._logger.Verify(x => x.Log(LogLevel.Error,
                                            It.IsAny<EventId>(),
                                            It.IsAny<It.IsAnyType>(),
                                            It.IsAny<Exception>(),
                                            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }


        [Test]
        public void SuccessfullyMapsPaymentRetrievalRequestModelToBankingApiRequestDto()
        {
            //Arrange
            var model = new RetrievePaymentsRequestDto()
            {
                TransactionId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };

            //Act
            var result = this._mapper.MapRetrievePaymentsRequestModelToBankDto(model);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BankRetrievePaymentsRequestDto>(result);
            Assert.AreEqual(result.TransactionId, model.TransactionId);
        }

        [Test]
        public void MappingPaymentRetrievalRequestModelToBankingApiRequestDtoThrowsErrorIfModelIsNull()
        {
            //Arrange - none needed.
            //Act - see assertion.

            //Assert
            var ex = Assert.Throws<HttpException>(() => this._mapper.MapRetrievePaymentsRequestModelToBankDto(null));
            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_MappingError_PaymentApiToBankApi, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_MappingError_PaymentApiToBankApi, ex.ErrorCode);

            //Verify logging took place
            this._logger.Verify(x => x.Log(LogLevel.Error,
                                            It.IsAny<EventId>(),
                                            It.IsAny<It.IsAnyType>(),
                                            It.IsAny<Exception>(),
                                            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Test]
        public void SuccessfullyMapsBankingApiResponseDtoToRetrievePaymentApiResponse()
        {
            //Arrange
            var bankingApiResponseDto = new BankRetrievePaymentsResponseDto()
            {
                Payments = new List<BankRetrievedPaymentDetails>()
                {
                    new BankRetrievedPaymentDetails()
                    {
                        PaymentStatus = PaymentStatus.Success,
                        PaymentDateTime = new DateTime(2020, 12, 01, 12, 0, 0),
                        PaymentAmount = 100.00M,
                        Currency = SupportedCurrencies.GBP,
                        CardNumber = "5500000000000004",
                        CardHolder = "Test Account",
                        CardType = CardType.MasterCard,
                        ExpirationMonth = "12",
                        ExpirationYear = "21"
                    }
                }
            };

            //Act
            var result = this._mapper.MapBankApiGetResponseToDomainResponse(bankingApiResponseDto);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RetrievePaymentsResponse>(result);
            Assert.IsNotNull(result.Payments);
            Assert.IsInstanceOf<List<RetrievedPaymentDetails>>(result.Payments);
            Assert.AreEqual(1, result.Payments.Count);

            var payment = result.Payments[0];
            Assert.AreEqual(PaymentStatus.Success, payment.PaymentStatus);
            Assert.AreEqual(new DateTime(2020, 12, 01, 12, 0, 0), payment.PaymentDateTime);
            Assert.AreEqual(100.00M, payment.PaymentAmount);
            Assert.AreEqual(SupportedCurrencies.GBP, payment.Currency);
            Assert.AreEqual("XXXXXXXXXXXX0004", payment.CardNumber);
            Assert.AreEqual("Test Account", payment.CardHolder);
            Assert.AreEqual(CardType.MasterCard, payment.CardType);
            Assert.AreEqual("12", payment.ExpirationMonth);
            Assert.AreEqual("21", payment.ExpirationYear);
        }

        [Test]
        public void SuccessfullyMapsBankingApiResponseDtoToRetrievePaymentApiResponseWhenMoreThanOnePaymentWasFound()
        {
            //Arrange
            var bankingApiResponseDto = new BankRetrievePaymentsResponseDto()
            {
                Payments = new List<BankRetrievedPaymentDetails>()
                {
                    new BankRetrievedPaymentDetails()
                    {
                        PaymentStatus = PaymentStatus.Success,
                        PaymentDateTime = new DateTime(2020, 12, 01, 12, 0, 0),
                        PaymentAmount = 100.00M,
                        Currency = SupportedCurrencies.GBP,
                        CardNumber = "5500000000000004",
                        CardHolder = "Test Account",
                        CardType = CardType.MasterCard,
                        ExpirationMonth = "12",
                        ExpirationYear = "21"
                    },
                    
                    new BankRetrievedPaymentDetails()
                    {
                        PaymentStatus = PaymentStatus.Pending,
                        PaymentDateTime = new DateTime(2020, 11, 02, 11, 0, 0),
                        PaymentAmount = 200.00M,
                        Currency = SupportedCurrencies.EUR,
                        CardNumber = "4111111111111111",
                        CardHolder = "Test Account",
                        CardType = CardType.Visa,
                        ExpirationMonth = "11",
                        ExpirationYear = "22"
                    }
                }
            };

            //Act
            var result = this._mapper.MapBankApiGetResponseToDomainResponse(bankingApiResponseDto);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RetrievePaymentsResponse>(result);
            Assert.IsNotNull(result.Payments);
            Assert.IsInstanceOf<List<RetrievedPaymentDetails>>(result.Payments);
            Assert.AreEqual(2, result.Payments.Count);

            var payment1 = result.Payments[0];
            Assert.AreEqual(PaymentStatus.Success, payment1.PaymentStatus);
            Assert.AreEqual(new DateTime(2020, 12, 01, 12, 0, 0), payment1.PaymentDateTime);
            Assert.AreEqual(100.00M, payment1.PaymentAmount);
            Assert.AreEqual(SupportedCurrencies.GBP, payment1.Currency);
            Assert.AreEqual("XXXXXXXXXXXX0004", payment1.CardNumber);
            Assert.AreEqual("Test Account", payment1.CardHolder);
            Assert.AreEqual(CardType.MasterCard, payment1.CardType);
            Assert.AreEqual("12", payment1.ExpirationMonth);
            Assert.AreEqual("21", payment1.ExpirationYear);

            var payment2 = result.Payments[1];
            Assert.AreEqual(PaymentStatus.Pending, payment2.PaymentStatus);
            Assert.AreEqual(new DateTime(2020, 11, 02, 11, 0, 0), payment2.PaymentDateTime);
            Assert.AreEqual(200.00M, payment2.PaymentAmount);
            Assert.AreEqual(SupportedCurrencies.EUR, payment2.Currency);
            Assert.AreEqual("XXXXXXXXXXXX1111", payment2.CardNumber);
            Assert.AreEqual("Test Account", payment2.CardHolder);
            Assert.AreEqual(CardType.Visa, payment2.CardType);
            Assert.AreEqual("11", payment2.ExpirationMonth);
            Assert.AreEqual("22", payment2.ExpirationYear);
        }

        [Test]
        public void MappingBankingApiResponseDtoToRetrievePaymentResponseThrowsErrorIfDtoIsNull()
        {
            //Arrange - none needed.
            //Act - see assertion.

            //Assert
            var ex = Assert.Throws<HttpException>(() => this._mapper.MapBankApiGetResponseToDomainResponse(null));
            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_MappingError_BankApiToPaymentApi, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_MappingError_BankApiToPaymentApi, ex.ErrorCode);

            //Verify logging took place
            this._logger.Verify(x => x.Log(LogLevel.Error,
                                            It.IsAny<EventId>(),
                                            It.IsAny<It.IsAnyType>(),
                                            It.IsAny<Exception>(),
                                            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Test]
        public void MappingBankingApiResponseDtoToRetrievePaymentResponseThrowsErrorIfDtoPaymentsCollectionIsNull()
        {
            
            //Arrange
            var bankingApiResponseDto = new BankRetrievePaymentsResponseDto()
            {
                Payments = null
            };

            //Act - see assertion.

            //Assert
            var ex = Assert.Throws<HttpException>(() => this._mapper.MapBankApiGetResponseToDomainResponse(bankingApiResponseDto));
            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_MappingError_BankApiToPaymentApi, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_MappingError_BankApiToPaymentApi, ex.ErrorCode);

            //Verify logging took place
            this._logger.Verify(x => x.Log(LogLevel.Error,
                                            It.IsAny<EventId>(),
                                            It.IsAny<It.IsAnyType>(),
                                            It.IsAny<Exception>(),
                                            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [TestCase("5500000000000004", 4, "XXXXXXXXXXXX0004")] //MasterCard
        [TestCase("5500000000000004", 3, "XXXXXXXXXXXXX004")]
        [TestCase("4111111111111111", 4, "XXXXXXXXXXXX1111")] //Visa
        [TestCase("4111111111111111", 3, "XXXXXXXXXXXXX111")]
        [TestCase("340000000000009", 4, "XXXXXXXXXXX0009")] //Amex
        [TestCase("340000000000009", 5, "XXXXXXXXXX00009")]
        public void MaskStringCorrectlyMasksInputAndLeavesRequiredNumberOfCharactersUnmasked(string unmaskedInput, int numberOfCharactersToLeaveUnmasked, string expectedOutput)
        {
            //Arrange - none needed.

            //Act
            var result = this._mapper.MaskString(unmaskedInput, numberOfCharactersToLeaveUnmasked);

            //Assert
            Assert.AreEqual(expectedOutput, result);
        }
    }
}
