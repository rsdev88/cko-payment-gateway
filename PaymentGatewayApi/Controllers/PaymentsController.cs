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
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello world!");
        }

        [HttpPost]
        public IActionResult ProcessPayment([FromBody] PaymentDetailsPost paymentDetails)
        {
            if(!ModelState.IsValid)
            {
                var response = new ResponseBase();
                response.statusCode = HttpStatusCode.BadRequest;
                response.Data = new ValidationErrorResponse(ModelState);

                return BadRequest(response);
            }

            //Todo - process valid request.
            return Ok("Work in progress...");
        }
    }
}
