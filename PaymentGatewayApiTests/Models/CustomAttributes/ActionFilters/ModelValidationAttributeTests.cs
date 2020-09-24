using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using PaymentGatewayApi.Models.CustomAttributes.ActionFilters;
using PaymentGatewayApi.Models.ResponseEntities;
using PaymentGatewayApi.Resources;
using System.Collections.Generic;
using System.Net;

namespace PaymentGatewayApiTests.Models.CustomAttributes.ActionFilters
{
    [TestFixture]
    public class ModelValidationAttributeTests
    {
        [Test]
        public void FailedModelValidationShouldResultInBadRequestStatus()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("fieldName", "error");

            var httpContext = new DefaultHttpContext();

            var actionExecutingContext = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new Microsoft.AspNetCore.Routing.RouteData(),
                    actionDescriptor: new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
                    modelState: modelState),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            var actionFilter = new ModelValidationAttribute();

            //Act
            actionFilter.OnActionExecuting(actionExecutingContext);

            //Assert
            Assert.IsNotNull(actionExecutingContext.Result);
            Assert.IsInstanceOf<BadRequestObjectResult>(actionExecutingContext.Result);

            var resultAsBadRequestObject = actionExecutingContext.Result as BadRequestObjectResult;
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
            Assert.IsTrue(resultError.ValidationErrors[0].FieldName == "fieldName");
            Assert.IsNotNull(resultError.ValidationErrors[0].ErrorMessages);
            Assert.IsTrue(resultError.ValidationErrors[0].ErrorMessages.Length == 1);
            Assert.IsTrue(resultError.ValidationErrors[0].ErrorMessages[0] == "error");
        }

        [Test]
        public void SuccessfulModelValidationShouldNotUpdateContextResult()
        {
            var modelState = new ModelStateDictionary();

            var httpContext = new DefaultHttpContext();

            var actionExecutingContext = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new Microsoft.AspNetCore.Routing.RouteData(),
                    actionDescriptor: new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
                    modelState: modelState),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            var actionFilter = new ModelValidationAttribute();

            //Act
            actionFilter.OnActionExecuting(actionExecutingContext);

            //Assert
            Assert.IsNull(actionExecutingContext.Result);
        }
    }
}
