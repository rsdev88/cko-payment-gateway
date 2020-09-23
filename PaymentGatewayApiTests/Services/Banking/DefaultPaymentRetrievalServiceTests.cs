using Moq;
using NUnit.Framework;
using PaymentGatewayApi.Mappers;
using PaymentGatewayApi.Models.BankingDTOs.v1;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using PaymentGatewayApi.Services;
using PaymentGatewayApi.Services.Banking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;

namespace PaymentGatewayApiTests.Services
{
    [TestFixture]
    public class DefaultPaymentRetrievalServiceTests
    {
        private Mock<IBankingService> _bankingService;
        private Mock<IDtoMapper> _dtoMapper;
        private DefaultPaymentsRetrievalService _paymentRetrievalService;

        [SetUp]
        public void DefaultPaymentRetrievalServiceTests_Setup()
        {
            this._bankingService = new Mock<IBankingService>();
            this._dtoMapper = new Mock<IDtoMapper>();
            this._paymentRetrievalService = new DefaultPaymentsRetrievalService(this._dtoMapper.Object, this._bankingService.Object);
        }

        [Test]
        public async Task RetrievePaymentReturnsRetrievePaymentResponse()
        {
            //Arrange
            var model = new RetrievePaymentsRequestDto()
            {
                TransactionId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };

            var bankingRequestDto = new BankRetrievePaymentsRequestDto()
            {
                TransactionId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };

            var bankingResponseDto = new BankRetrievePaymentsResponseDto()
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

            var retrievePaymentsResponse = new RetrievePaymentsResponse()
            {
                Payments = new List<RetrievedPaymentDetails>()
                {
                    new RetrievedPaymentDetails()
                    {
                        PaymentStatus = PaymentStatus.Success,
                        PaymentDateTime = new DateTime(2020, 12, 01, 12, 0, 0),
                        PaymentAmount = 100.00M,
                        Currency = SupportedCurrencies.GBP,
                        CardNumber = "XXXXXXXXXXXX0004",
                        CardHolder = "Test Account",
                        CardType = CardType.MasterCard,
                        ExpirationMonth = "12",
                        ExpirationYear = "21"
                    }
                }
            };

            this._dtoMapper.Setup(x => x.MapRetrievePaymentsRequestModelToBankDto(It.IsAny<RetrievePaymentsRequestDto>())).Returns(bankingRequestDto);
            this._bankingService.Setup(x => x.RetrievePayments(It.IsAny<BankRetrievePaymentsRequestDto>())).ReturnsAsync(bankingResponseDto);
            this._dtoMapper.Setup(x => x.MapBankApiGetResponseToDomainResponse(It.IsAny<BankRetrievePaymentsResponseDto>())).Returns(retrievePaymentsResponse);

            //Act
            var result = await this._paymentRetrievalService.RetrievePayments(model);

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
    }
}
