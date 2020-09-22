using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Models.ResponseEntities
{
    public class ProcessPaymentResponse
    {
        [JsonProperty("transactionId")]
        public Guid TransactionId { get; set; }
    }
}
