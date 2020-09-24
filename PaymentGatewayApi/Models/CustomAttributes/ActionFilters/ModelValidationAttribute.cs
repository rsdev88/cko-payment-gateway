using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentGatewayApi.Models.ResponseEntities;
using System.Net;

namespace PaymentGatewayApi.Models.CustomAttributes.ActionFilters
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ResponseBaseDto()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = new ValidationErrorResponse(context.ModelState)
                });
            }
        }
    }
}
