using Moq;
using NUnit.Framework;
using PaymentGatewayApi.Mappers;
using PaymentGatewayApi.Models.BankingDTOs;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using PaymentGatewayApi.Services;
using PaymentGatewayApi.Services.Banking;
using System;
using System.Threading.Tasks;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;

namespace PaymentGatewayApiTests.Services
{
    [TestFixture]
    class DefaultPaymentProcessingServiceTests
    {
        private Mock<IBankingService> _bankingService;
        private Mock<IDtoMapper> _dtoMapper;
        private DefaultPaymentsProcessingService _paymentProcessingService;

        [SetUp]
        public void DefaultPaymentProcessingServiceTests_Setup()
        {
            this._bankingService = new Mock<IBankingService>();
            this._dtoMapper = new Mock<IDtoMapper>();
            this._paymentProcessingService = new DefaultPaymentsProcessingService(this._dtoMapper.Object, this._bankingService.Object);
        }

        [Test]
        public async Task ProcessPaymentReturnsProcessPaymentResponse()
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

            var bankingRequestDto = new BankProcessPaymentRequestDto()
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

            var transactionId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
            var bankingResponseDto = new BankProcessPaymentResponseDto()
            {
                TransactionId = transactionId
            };

            var guid = new Guid(transactionId);
            var processPaymentResponse = new ProcessPaymentResponse()
            {
                TransactionId = guid
            };

            this._dtoMapper.Setup(x => x.MapProcessPaymentRequestModelToBankDto(It.IsAny<ProcessPaymentRequestDto>())).Returns(bankingRequestDto);
            this._bankingService.Setup(x => x.ProcessPayment(It.IsAny<BankProcessPaymentRequestDto>())).ReturnsAsync(bankingResponseDto);
            this._dtoMapper.Setup(x => x.MapBankApiResponseToDomainResponse(It.IsAny<BankProcessPaymentResponseDto>())).Returns(processPaymentResponse);

            //Act
            var result = await this._paymentProcessingService.ProcessPayment(model);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ProcessPaymentResponse>(result);
            Assert.AreEqual(guid, result.TransactionId);

        }
    }
}
