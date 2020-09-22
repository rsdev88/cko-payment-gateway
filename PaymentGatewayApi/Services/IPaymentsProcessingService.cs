using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Services
{
    public interface IPaymentsProcessingService
    {
        public Task<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequestDto model);
    }
}
