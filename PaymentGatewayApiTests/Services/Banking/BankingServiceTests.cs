using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Models.BankingDTOs;
using PaymentGatewayApi.Resources;
using PaymentGatewayApi.Services.Banking;
using System;
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
        private BankingService _bankingService;

        [SetUp]
        public void BankingServiceTests_Setup()
        {
            this._handler = new Mock<HttpMessageHandler>();
            this._bankingApiConfiguration = new Mock<IConfiguration>();
            this._bankingApiConfiguration.SetupGet(x => x["bankingApi:paymentsEndpoint"]).Returns("fakeendpoint");

            var httpClient = new HttpClient(this._handler.Object)
            {
                BaseAddress = new Uri("http://fakeapi.com/")
            };

            this._bankingService = new BankingService(this._bankingApiConfiguration.Object, httpClient);
        }

        [Test]
        public async Task ProcessPaymentReturnsResponseWhenBankingApiRespondsSuccessfully()
        {
            //Arrange
            var transactionId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var data = new BankProcessPaymentResponseDto()
            {
                TransactionId = transactionId
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
            this._bankingService = new BankingService(this._bankingApiConfiguration.Object, httpClient);

            //Act - see assertion.

            //Assert
            var ex = Assert.ThrowsAsync<HttpException>(() => this._bankingService.ProcessPayment(bankDto));
            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_BankingApiUnexpectedError, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_BankingApiUnexpectedError, ex.ErrorCode);
        }
    }
}
