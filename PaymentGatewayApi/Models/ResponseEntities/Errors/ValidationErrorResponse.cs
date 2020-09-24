using ApiSharedLibrary.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace PaymentGatewayApi.Models.ResponseEntities
{
    public class ValidationErrorResponse : ErrorResponse
    {
        public ValidationErrorResponse(ModelStateDictionary model)
        {
            this.ErrorMessage = Resources.ErrorMessage_Validation;
            this.ErrorDescription = Resources.ErrorDescription_Validation;
            this.ErrorCode = Resources.ErrorCode_Validation;

            this.ValidationErrors = new List<ValidationError>();
            this.ValidationErrors = model.Select(item => new ValidationError(item.Key, item.Value.Errors)).ToList();
        }

        [JsonProperty("validationErrors")]
        public List<ValidationError> ValidationErrors { get; set; }

        public class ValidationError
        {
            public ValidationError(string fieldName, ModelErrorCollection errorMessages)
            {
                this.FieldName = fieldName;
                this.ErrorMessages = errorMessages.Select(error => error.ErrorMessage).ToArray();
            }

            [JsonProperty("fieldName")]
            public string FieldName { get; }

            [JsonProperty("errorMessages")]
            public string[] ErrorMessages { get; }
        }
    }
}
