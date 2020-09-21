using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using System.Net;

namespace PaymentGatewayApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PaymentsController : ControllerBase
    {
        [HttpPost]
        public IActionResult ProcessPayment([FromBody] ProcessPaymentPostDto paymentDetails)
        {
            if(!ModelState.IsValid)
            {
                var response = new ResponseBaseDto();
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Data = new ValidationErrorResponse(ModelState);

                return BadRequest(response);
            }

            //Todo - process valid request.
            return Ok(new ResponseBaseDto() 
            { 
                StatusCode = HttpStatusCode.OK,
                Data = "Work in progress..."
            });
        }
    }
}
