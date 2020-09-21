using Newtonsoft.Json;
using PaymentGatewayApi.Models.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;

namespace PaymentGatewayApi.Models.RequestEntities
{
    public class ProcessPaymentPostDto
    {
        [JsonProperty("cardNumber")]
        [Required]
        [CreditCard]
        [MinLength(15), MaxLength(16)]
        public string CardNumber { get; set; }

        [JsonProperty("cardHolder")]
        [Required]
        public string CardHolder { get; set; }

        [JsonProperty("cardType")]
        [Required]
        [Range(minimum: 0, maximum: 2)]
        public CardType? CardType { get; set; }

        [JsonProperty("expirationMonth")]
        [Required]
        [ExpirationMonth()]
        [MinLength(2), MaxLength(2)]
        public string ExpirationMonth { get; set; }

        [JsonProperty("expirationYear")]
        [Required]
        [ExpirationYear(ErrorMessageResourceName = "Validation_ExpirationYear", ErrorMessageResourceType = typeof(Resources.Resources))]
        [MinLength(2), MaxLength(2)]
        public string ExpirationYear { get; set; }

        [JsonProperty("paymentAmount")]
        [Required]
        public decimal? PaymentAmount { get; set; }

        [JsonProperty("currency")]
        [Required]
        [Range(minimum: 0, maximum: 11)]
        public SupportedCurrencies? Currency { get; set; }

        [JsonProperty("cvv")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessageResourceName = "Validation_CvvRegex", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Required]
        [MinLength(3), MaxLength(4)]
        public string Cvv { get; set; }
    }
}
