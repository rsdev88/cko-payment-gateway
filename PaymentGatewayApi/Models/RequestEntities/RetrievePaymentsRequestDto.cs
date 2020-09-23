using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGatewayApi.Models.RequestEntities
{
    public class RetrievePaymentsRequestDto
    {
        [Required]
        [RegularExpression(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$",
                            ErrorMessageResourceName = "Validation_TransactionId", ErrorMessageResourceType = typeof(Resources.Resources))]
        public Guid? TransactionId { get; set; }
    }
}
