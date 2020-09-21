using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PaymentGatewayApi.Controllers;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using System.Net;

namespace PaymentGatewayApiTests.Controllers
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private PaymentsController _controller;

        [SetUp]
        public void SetupTests()
        {
            this._controller = new PaymentsController();
        }

        [Test]
        //This particular test will evolve as this action method is developed.
        public void ProcessPaymentShouldReturn200ForValidModelState()
        {
            //Arrange
            var model = new ProcessPaymentPostDto();

            //Act
            var result = this._controller.ProcessPayment(model);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var resultAsOkObject = result as OkObjectResult;
            Assert.IsTrue(resultAsOkObject.StatusCode == 200);
            Assert.IsNotNull(resultAsOkObject.Value);
            Assert.IsInstanceOf<ResponseBaseDto>(resultAsOkObject.Value);

            var resultValue = resultAsOkObject.Value as ResponseBaseDto;
            Assert.IsTrue(resultValue.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(resultValue.Data.ToString() == "Work in progress...");
        }

        [Test]
        public void ProcessPaymentShouldReturn400ForInvalidModelState()
        {
            //Arrange
            var model = new ProcessPaymentPostDto();
            this._controller.ModelState.AddModelError("ExpirationMonth", PaymentGatewayApi.Resources.Resources.Validation_ExpirationMonth);

            //Act
            var result = this._controller.ProcessPayment(model);

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
            Assert.IsTrue(resultError.ErrorMessage == PaymentGatewayApi.Resources.Resources.ErrorMessage_Validation);
            Assert.IsTrue(resultError.ErrorDescription == PaymentGatewayApi.Resources.Resources.ErrorDescription_Validation);
            Assert.IsTrue(resultError.ErrorCode == PaymentGatewayApi.Resources.Resources.ErrorCode_Validation);
            Assert.IsNotNull(resultError.ValidationErrors);
            Assert.IsTrue(resultError.ValidationErrors.Count == 1);
            Assert.IsTrue(resultError.ValidationErrors[0].FieldName == "ExpirationMonth");
            Assert.IsNotNull(resultError.ValidationErrors[0].ErrorMessages);
            Assert.IsTrue(resultError.ValidationErrors[0].ErrorMessages.Length == 1);
            Assert.IsTrue(resultError.ValidationErrors[0].ErrorMessages[0] == PaymentGatewayApi.Resources.Resources.Validation_ExpirationMonth);
        }
    }
}
