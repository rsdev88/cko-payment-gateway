using Newtonsoft.Json;
using System;
using System.Net;

namespace PaymentGatewayApi.Models.ResponseEntities
{
    public class ResponseBase
    {
        [JsonProperty("statusCode")]
        public HttpStatusCode statusCode { get; set; }

        [JsonProperty("data")]
        public Object Data { get; set; }
    }
}
