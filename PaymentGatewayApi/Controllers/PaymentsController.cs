using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGatewayApi.Models.CustomAttributes.ActionFilters;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using PaymentGatewayApi.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsProcessingService _paymentsProcessingService;
        private readonly IPaymentsRetrievalService _paymentsRetrievalService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentsProcessingService paymentsProcessingService, 
                                  IPaymentsRetrievalService paymentsRetrievalService,
                                  ILogger<PaymentsController> logger)
        {
            this._paymentsProcessingService = paymentsProcessingService;
            this._paymentsRetrievalService = paymentsRetrievalService;
            this._logger = logger;
        }

        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequestDto paymentDetails)
        {
            var responseData = await this._paymentsProcessingService.ProcessPayment(paymentDetails);

            return Ok(new ResponseBaseDto()
            {
                StatusCode = HttpStatusCode.OK,
                Data = responseData
            });
        }

        [HttpGet("{transactionid}")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<IActionResult> RetrievePayments(RetrievePaymentsRequestDto model)
        {
            var responseData = await this._paymentsRetrievalService.RetrievePayments(model);

            return Ok(new ResponseBaseDto()
            {
                StatusCode = HttpStatusCode.OK,
                Data = responseData
            });
        }
    }
}
