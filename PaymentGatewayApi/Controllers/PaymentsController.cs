using Microsoft.AspNetCore.Mvc;
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

        public PaymentsController(IPaymentsProcessingService paymentsProcessingService, IPaymentsRetrievalService paymentsRetrievalService)
        {
            this._paymentsProcessingService = paymentsProcessingService;
            this._paymentsRetrievalService = paymentsRetrievalService;
        }

        [HttpPost]
        [ModelValidation]
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
        [ModelValidation]
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
