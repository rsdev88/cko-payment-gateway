using Microsoft.AspNetCore.Http;
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

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //Todo: logging
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
