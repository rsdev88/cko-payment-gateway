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

namespace PaymentGatewayApiTests.Controllers
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private PaymentsController _controller;
        private Mock<IPaymentsProcessingService> _paymentProcessingService;

        [SetUp]
        public void SetupTests()
        {
            this._paymentProcessingService = new Mock<IPaymentsProcessingService>();
            this._controller = new PaymentsController(_paymentProcessingService.Object);
        }

        [Test]
        public async Task ProcessPaymentShouldReturn200ForValidModelState()
        {
            //Arrange
            var model = new ProcessPaymentRequestDto();
            var guid = new Guid();
            var serviceResponse = new ProcessPaymentResponse()
            {
                TransactionId = guid
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
        }

        [Test]
        public void ProcessPaymentShouldReturn400ForInvalidModelState()
        {
            //Arrange
            var model = new ProcessPaymentRequestDto();
            this._controller.ModelState.AddModelError("ExpirationMonth", Resources.Validation_ExpirationMonth);

            //Act
            var result = this._controller.ProcessPayment(model).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var resultAsBadRequestObject = result as BadRequestObjectResult;
            Assert.IsTrue(resultAsBadRequestObject.StatusCode == 400);
            Assert.IsNotNull(resultAsBadRequestObject.Value);
            Assert.IsInstanceOf<ResponseBaseDto>(resultAsBadRequestObject.Value);

            var resultValue = resultAsBadRequestObject.Value as ResponseBaseDto;
            Assert.IsTrue(resultValue.StatusCode == HttpStatusCode.BadRequest);
            Assert.IsNotNull(resultValue.Data);
            Assert.IsInstanceOf<ValidationErrorResponse>(resultValue.Data);

            var resultError = resultValue.Data as ValidationErrorResponse;
            Assert.IsTrue(resultError.ErrorMessage == Resources.ErrorMessage_Validation);
            Assert.IsTrue(resultError.ErrorDescription == Resources.ErrorDescription_Validation);
            Assert.IsTrue(resultError.ErrorCode == Resources.ErrorCode_Validation);
            Assert.IsNotNull(resultError.ValidationErrors);
            Assert.IsTrue(resultError.ValidationErrors.Count == 1);
            Assert.IsTrue(resultError.ValidationErrors[0].FieldName == "ExpirationMonth");
            Assert.IsNotNull(resultError.ValidationErrors[0].ErrorMessages);
            Assert.IsTrue(resultError.ValidationErrors[0].ErrorMessages.Length == 1);
            Assert.IsTrue(resultError.ValidationErrors[0].ErrorMessages[0] == Resources.Validation_ExpirationMonth);
        }
    }
}
