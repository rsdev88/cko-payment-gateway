using PaymentGatewayApi.Models.RequestEntities;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGatewayApi.Models.CustomAttributes
{
    public class ExpirationMonthAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object expirationMonth, ValidationContext validationContext)
        {
            //We need the expirationYear value from the request to evaluate if the expirationMonth is in the past.
            var model = (PaymentDetailsPost)validationContext.ObjectInstance;
            var expirationYear = model?.ExpirationYear;

            if(string.IsNullOrEmpty(expirationMonth as string) || string.IsNullOrEmpty(expirationYear))
            {
                return new ValidationResult(Resources.Resources.Validation_ExpirationMonth);
            }
                    
            var minimumMonth = expirationYear == DateTime.Now.ToString("yy") ? DateTime.Now.Month: 1;
            var maximumMonth = 12;

            int expirationMonthAsInt;
            int.TryParse(expirationMonth.ToString(), out expirationMonthAsInt);

            if(expirationMonthAsInt >= minimumMonth && expirationMonthAsInt <= maximumMonth)
            {
                return ValidationResult.Success;
            }
            
            return new ValidationResult(Resources.Resources.Validation_ExpirationMonth);
        }
    }
}
