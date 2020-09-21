using Microsoft.AspNetCore.Mvc;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using PaymentGatewayApi.Services;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsProcessingService _paymentsProcessingService;

        public PaymentsController(IPaymentsProcessingService paymentsProcessingService)
        {
            this._paymentsProcessingService = paymentsProcessingService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequestDto paymentDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseBaseDto() 
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = new ValidationErrorResponse(ModelState)
                });
            }

            var responseData = await this._paymentsProcessingService.ProcessPayment(paymentDetails);

            return Ok(new ResponseBaseDto()
            {
                StatusCode = HttpStatusCode.OK,
                Data = responseData
            });
        }
    }
}
