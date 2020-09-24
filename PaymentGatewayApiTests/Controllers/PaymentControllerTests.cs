using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PaymentGatewayApi.Controllers;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using PaymentGatewayApi.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using PaymentGatewayApi.Resources;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;
using System.Collections.Generic;

namespace PaymentGatewayApiTests.Controllers
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private PaymentsController _controller;
        private Mock<IPaymentsProcessingService> _paymentProcessingService;
        private Mock<IPaymentsRetrievalService> _paymentRetrievalService;

        [SetUp]
        public void SetupTests()
        {
            this._paymentProcessingService = new Mock<IPaymentsProcessingService>();
            this._paymentRetrievalService = new Mock<IPaymentsRetrievalService>();
            this._controller = new PaymentsController(this._paymentProcessingService.Object, this._paymentRetrievalService.Object);
        }

        [Test]
        public async Task ProcessPaymentShouldReturn200()
        {
            //Arrange
            var model = new ProcessPaymentRequestDto();
            var guid = new Guid();
            var serviceResponse = new ProcessPaymentResponse()
            {
                TransactionId = guid,
                PaymentStatus = PaymentStatus.Success
            };

            this._paymentProcessingService.Setup(x => x.ProcessPayment(It.IsAny<ProcessPaymentRequestDto>()))
                                            .ReturnsAsync(serviceResponse);

            //Act
            var result = await this._controller.ProcessPayment(model);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var resultAsOkObject = result as OkObjectResult;
            Assert.IsTrue(resultAsOkObject.StatusCode == 200);
            Assert.IsNotNull(resultAsOkObject.Value);
            Assert.IsInstanceOf<ResponseBaseDto>(resultAsOkObject.Value);

            var resultValue = resultAsOkObject.Value as ResponseBaseDto;
            Assert.IsTrue(resultValue.StatusCode == HttpStatusCode.OK);
            Assert.IsNotNull(resultValue.Data);
            Assert.IsInstanceOf<ProcessPaymentResponse>(resultValue.Data);

            var resultData = resultValue.Data as ProcessPaymentResponse;
            Assert.AreEqual(guid, resultData.TransactionId);
            Assert.AreEqual(PaymentStatus.Success, resultData.PaymentStatus);
        }

        [Test]
        public async Task RetrievePaymentsShouldReturn200()
        {
            //Arrange
            var model = new RetrievePaymentsRequestDto();
            var serviceResponse = new RetrievePaymentsResponse()
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

            this._paymentRetrievalService.Setup(x => x.RetrievePayments(It.IsAny<RetrievePaymentsRequestDto>()))
                                            .ReturnsAsync(serviceResponse);

            //Act
            var result = await this._controller.RetrievePayments(model);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var resultAsOkObject = result as OkObjectResult;
            Assert.IsTrue(resultAsOkObject.StatusCode == 200);
            Assert.IsNotNull(resultAsOkObject.Value);
            Assert.IsInstanceOf<ResponseBaseDto>(resultAsOkObject.Value);

            var resultValue = resultAsOkObject.Value as ResponseBaseDto;
            Assert.IsTrue(resultValue.StatusCode == HttpStatusCode.OK);
            Assert.IsNotNull(resultValue.Data);
            Assert.IsInstanceOf<RetrievePaymentsResponse>(resultValue.Data);

            var resultData = resultValue.Data as RetrievePaymentsResponse;
            Assert.IsNotNull(resultData.Payments);
            Assert.IsInstanceOf <List<RetrievedPaymentDetails>>(resultData.Payments);
            Assert.AreEqual(1, resultData.Payments.Count);

            var resultPayment = resultData.Payments[0];
            Assert.AreEqual(serviceResponse.Payments[0].PaymentStatus, resultPayment.PaymentStatus);
            Assert.AreEqual(serviceResponse.Payments[0].PaymentDateTime, resultPayment.PaymentDateTime);
            Assert.AreEqual(serviceResponse.Payments[0].PaymentAmount, resultPayment.PaymentAmount);
            Assert.AreEqual(serviceResponse.Payments[0].Currency, resultPayment.Currency);
            Assert.AreEqual(serviceResponse.Payments[0].CardNumber, resultPayment.CardNumber);
            Assert.AreEqual(serviceResponse.Payments[0].CardHolder, resultPayment.CardHolder);
            Assert.AreEqual(serviceResponse.Payments[0].CardType, resultPayment.CardType);
            Assert.AreEqual(serviceResponse.Payments[0].ExpirationMonth, resultPayment.ExpirationMonth);
            Assert.AreEqual(serviceResponse.Payments[0].ExpirationYear, resultPayment.ExpirationYear);
        }
    }
}
