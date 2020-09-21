using Newtonsoft.Json;
using System;
using System.Net;

namespace PaymentGatewayApi.Models.ResponseEntities
{
    public class ResponseBaseDto
    {
        [JsonProperty("statusCode")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty("data")]
        public Object Data { get; set; }
    }
}
