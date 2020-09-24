using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Models.ResponseEntities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                this._logger.LogError(Resources.Resources.Logging_GlobalExceptionHandler, ex.Message);
                await HandleException(httpContext, ex);
            }
        }

        private Task HandleException(HttpContext httpContext, Exception ex)
        {
            HttpStatusCode statusCode;
            string errorCode;
            string errorMessage;

            if (ex is HttpException httpException)
            {
                statusCode = httpException.StatusCode;
                errorCode = httpException.ErrorCode;
                errorMessage = httpException.Message;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                errorCode = Resources.Resources.ErrorCode_InternalServerErrorCatchAll;
                errorMessage = Resources.Resources.ErrorMessage_InternalServerErrorCatchAll;
            }

            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseBaseDto()
            {
                StatusCode = statusCode,
                Data = new ErrorResponse()
                {
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage,
                    ErrorDescription = Resources.Resources.ErrorDescription_Generic
                }
            }));
        }
    }
}
