
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Models.BankingDTOs.v1;
using PaymentGatewayApi.Resources;
using PaymentGatewayApi.Services.Banking;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;

namespace PaymentGatewayApiTests.Services.Banking
{
    [TestFixture]
    public class BankingServiceTests
    {
        private Mock<HttpMessageHandler> _handler;
        private Mock<IConfiguration> _bankingApiConfiguration;
        private Mock<ILogger<BankingService>> _logger;
        private BankingService _bankingService;

        [SetUp]
        public void BankingServiceTests_Setup()
        {
            this._handler = new Mock<HttpMessageHandler>();
            this._bankingApiConfiguration = new Mock<IConfiguration>();
            this._bankingApiConfiguration.SetupGet(x => x["bankingApi:paymentsEndpointPost"]).Returns("fakeendpointPost");
            this._bankingApiConfiguration.SetupGet(x => x["bankingApi:paymentsEndpointGet"]).Returns("fakeendpointGet");
            this._logger = new Mock<ILogger<BankingService>>();

            var httpClient = new HttpClient(this._handler.Object)
            {
                BaseAddress = new Uri("http://fakeapi.com/")
            };

            this._bankingService = new BankingService(this._bankingApiConfiguration.Object, httpClient, this._logger.Object);
        }

        [Test]
        public async Task ProcessPaymentReturnsResponseWhenBankingApiRespondsSuccessfully()
        {
            //Arrange
            var transactionId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var data = new BankProcessPaymentResponseDto()
            {
                TransactionId = transactionId,
                PaymentStatus = PaymentStatus.Success
            };
            httpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            this._handler.Protected()
                         .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                         .ReturnsAsync(httpResponseMessage);

            var bankDto = new BankProcessPaymentRequestDto()
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
            var result = await this._bankingService.ProcessPayment(bankDto);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BankProcessPaymentResponseDto>(result);
            Assert.AreEqual(transactionId, result.TransactionId);
            Assert.AreEqual(PaymentStatus.Success, result.PaymentStatus);
        }

        [Test]
        public void ProcessPaymentThrowsHttpExceptionWhenBankingApiRespondsUnsuccessfully()
        {
            //Arrange
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            this._handler.Protected()
                         .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                         .ReturnsAsync(httpResponseMessage);

            var bankDto = new BankProcessPaymentRequestDto()
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

            //Act - see assertion.
            
            //Assert
            var ex = Assert.ThrowsAsync<HttpException>(() => this._bankingService.ProcessPayment(bankDto));
            Assert.AreEqual(HttpStatusCode.BadGateway, ex.StatusCode);
            Assert.AreEqual(string.Format(Resources.ErrorMessage_BankingApiUnsuccesfulResponse, "500", "Internal Server Error"), ex.Message);
            Assert.AreEqual(Resources.ErrorCode_BankingApiUnsuccesfulResponse, ex.ErrorCode);
        }

        [Test]
        public void ProcessPaymentThrowsHttpExceptionWhenBankingApiCallEncountersAnUnpectedError()
        {
            //Arrange
            var bankDto = new BankProcessPaymentRequestDto()
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

            var httpClient = new HttpClient(this._handler.Object)
            {
                BaseAddress = null
            };
            this._bankingService = new BankingService(this._bankingApiConfiguration.Object, httpClient, this._logger.Object);

            //Act - see assertion.

            //Assert
            var ex = Assert.ThrowsAsync<HttpException>(() => this._bankingService.ProcessPayment(bankDto));
            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_BankingApiUnexpectedError, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_BankingApiUnexpectedError, ex.ErrorCode);

            //Verify logging took place
            this._logger.Verify(x => x.Log(LogLevel.Error,
                                            It.IsAny<EventId>(),
                                            It.IsAny<It.IsAnyType>(),
                                            It.IsAny<Exception>(),
                                            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }


        [Test]
        public async Task RetrievePaymentReturnsResponseWhenBankingApiRespondsSuccessfully()
        {
            //Arrange
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var data = new BankRetrievePaymentsResponseDto()
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
            httpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            this._handler.Protected()
                         .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                         .ReturnsAsync(httpResponseMessage);

            var bankDto = new BankRetrievePaymentsRequestDto()
            {
                TransactionId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };

            //Act
            var result = await this._bankingService.RetrievePayments(bankDto);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BankRetrievePaymentsResponseDto>(result);
            Assert.IsNotNull(result.Payments);
            Assert.IsInstanceOf<List<BankRetrievedPaymentDetails>>(result.Payments);
            Assert.AreEqual(1, result.Payments.Count);

            var payment = result.Payments[0];
            Assert.AreEqual(PaymentStatus.Success, payment.PaymentStatus);
            Assert.AreEqual(new DateTime(2020, 12, 01, 12, 0, 0), payment.PaymentDateTime);
            Assert.AreEqual(100.00M, payment.PaymentAmount);
            Assert.AreEqual(SupportedCurrencies.GBP, payment.Currency);
            Assert.AreEqual("5500000000000004", payment.CardNumber);
            Assert.AreEqual("Test Account", payment.CardHolder);
            Assert.AreEqual(CardType.MasterCard, payment.CardType);
            Assert.AreEqual("12", payment.ExpirationMonth);
            Assert.AreEqual("21", payment.ExpirationYear);
        }

        [Test]
        public void RetrievePaymentThrowsHttpExceptionWhenBankingApiRespondsUnsuccessfully()
        {
            //Arrange
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            this._handler.Protected()
                         .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                         .ReturnsAsync(httpResponseMessage);

            var bankDto = new BankRetrievePaymentsRequestDto()
            {
                TransactionId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };

            //Act - see assertion.

            //Assert
            var ex = Assert.ThrowsAsync<HttpException>(() => this._bankingService.RetrievePayments(bankDto));
            Assert.AreEqual(HttpStatusCode.BadGateway, ex.StatusCode);
            Assert.AreEqual(string.Format(Resources.ErrorMessage_BankingApiUnsuccesfulResponse, "500", "Internal Server Error"), ex.Message);
            Assert.AreEqual(Resources.ErrorCode_BankingApiUnsuccesfulResponse, ex.ErrorCode);
        }

        [Test]
        public void RetrievePaymentThrowsHttpExceptionWhenBankingApiCallEncountersAnUnpectedError()
        {
            //Arrange
            var bankDto = new BankRetrievePaymentsRequestDto()
            {
                TransactionId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };

            var httpClient = new HttpClient(this._handler.Object)
            {
                BaseAddress = null
            };
            this._bankingService = new BankingService(this._bankingApiConfiguration.Object, httpClient, this._logger.Object);

            //Act - see assertion.

            //Assert
            var ex = Assert.ThrowsAsync<HttpException>(() => this._bankingService.RetrievePayments(bankDto));
            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_BankingApiUnexpectedError, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_BankingApiUnexpectedError, ex.ErrorCode);

            //Verify logging took place
            this._logger.Verify(x => x.Log(LogLevel.Error,
                                            It.IsAny<EventId>(),
                                            It.IsAny<It.IsAnyType>(),
                                            It.IsAny<Exception>(),
                                            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }
    }
}
