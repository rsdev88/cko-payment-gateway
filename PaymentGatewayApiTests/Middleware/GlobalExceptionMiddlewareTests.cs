using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NUnit.Framework;
using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Middleware;
using PaymentGatewayApi.Models.ResponseEntities;
using PaymentGatewayApi.Resources;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGatewayApiTests.Middleware
{
    [TestFixture]
    public class GlobalExceptionMiddlewareTests
    {
        [Test]
        public async Task MiddlewareShouldWriteA500ErrorToResponseWhenUnhandledExceptionOccurrs()
        {
            //Arrange
            var middleware = new GlobalExceptionMiddleware(next: (innerHttpContext) =>
            {
                throw new Exception();
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.InvokeAsync(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var result = JsonConvert.DeserializeObject<ResponseBaseDto>(streamText);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ResponseBaseDto>(result);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.IsNotNull(result.Data);

            var resultError = JsonConvert.DeserializeObject<ErrorResponse>(result.Data.ToString());
            Assert.IsInstanceOf<ErrorResponse>(resultError);
            Assert.AreEqual(Resources.ErrorCode_InternalServerErrorCatchAll, resultError.ErrorCode);
            Assert.AreEqual(Resources.ErrorMessage_InternalServerErrorCatchAll, resultError.ErrorMessage);
            Assert.AreEqual(Resources.ErrorDescription_Generic, resultError.ErrorDescription);
        }

        [Test]
        public async Task MiddlewareShouldWriteACustomErrorToResponseWhenAnHttpExceptionIsThrown()
        {
            //Arrange
            var errorMessage = "Test error";
            var errorCode = "1234";
            var statusCode = HttpStatusCode.NotFound;

            var middleware = new GlobalExceptionMiddleware(next: (innerHttpContext) =>
            {
                throw new HttpException(statusCode, errorCode, errorMessage);
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.InvokeAsync(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var result = JsonConvert.DeserializeObject<ResponseBaseDto>(streamText);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ResponseBaseDto>(result);
            Assert.AreEqual(statusCode, result.StatusCode);
            Assert.IsNotNull(result.Data);

            var resultError = JsonConvert.DeserializeObject<ErrorResponse>(result.Data.ToString());
            Assert.IsInstanceOf<ErrorResponse>(resultError);
            Assert.AreEqual(errorCode, resultError.ErrorCode);
            Assert.AreEqual(errorMessage, resultError.ErrorMessage);
            Assert.AreEqual(Resources.ErrorDescription_Generic, resultError.ErrorDescription);
        }
    }
}
