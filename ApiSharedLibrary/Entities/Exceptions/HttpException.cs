using System;
using System.Net;

namespace PaymentGatewayApi.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorCode { get; set; }

        public HttpException(HttpStatusCode statusCode, string errorCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
            this.ErrorCode = errorCode;
        }

        public HttpException(HttpStatusCode statusCode, string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            this.StatusCode = statusCode;
            this.ErrorCode = errorCode;
        }
    }
}
