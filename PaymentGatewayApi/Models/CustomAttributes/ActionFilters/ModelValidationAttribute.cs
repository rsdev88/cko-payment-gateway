using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PaymentGatewayApi.Models.ResponseEntities;
using System.Linq;
using System.Net;

namespace PaymentGatewayApi.Models.CustomAttributes.ActionFilters
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ModelValidationAttribute> _logger;

        public ModelValidationAttribute(ILogger<ModelValidationAttribute> logger)
        {
            this._logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                this._logger.LogError(Resources.Resources.Logging_ModelValidationError, context.ModelState.Select(item => item.Key).ToList());

                context.Result = new BadRequestObjectResult(new ResponseBaseDto()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = new ValidationErrorResponse(context.ModelState)
                });
            }
        }
    }
}
