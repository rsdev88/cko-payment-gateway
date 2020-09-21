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

            if (ex is HttpException httpException)
            {
                statusCode = httpException.StatusCode;
                errorCode = httpException.ErrorCode;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                errorCode = Resources.Resources.ErrorCode_InternalServerErrorCatchAll;
            }

            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseBaseDto()
            {
                StatusCode = statusCode,
                Data = new ErrorResponse()
                {
                    ErrorCode = errorCode, 
                    ErrorMessage = ex.Message ?? Resources.Resources.ErrorMessage_InternalServerErrorCatchAll,
                    ErrorDescription = ex.InnerException?.Message ?? Resources.Resources.ErrorDescription_Generic
                }
            }));
        }
    }
}
