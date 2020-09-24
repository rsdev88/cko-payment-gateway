using ApiSharedLibrary.Resources;
using Newtonsoft.Json;

namespace PaymentGatewayApi.Models.ResponseEntities
{
    public class ErrorResponse
    {
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; set; } = Resources.ErrorDescription_Generic;

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }
    }
}
