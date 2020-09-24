using PaymentGatewayApi.Mappers;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using PaymentGatewayApi.Services.Banking;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Services
{
    public class DefaultPaymentsProcessingService : IPaymentsProcessingService
    {
        private readonly IDtoMapper _dtoMapper;
        private readonly IBankingService _bankingService;

        public DefaultPaymentsProcessingService(IDtoMapper dtoMapper, IBankingService bankingService)
        {
            this._dtoMapper = dtoMapper;
            this._bankingService = bankingService;
        }

        public async Task<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequestDto model)
        {
            var bankRequestDto = this._dtoMapper.MapProcessPaymentRequestModelToBankDto(model);
            var bankResponseDto = await this._bankingService.ProcessPayment(bankRequestDto);
            var processPaymentResponse = this._dtoMapper.MapBankApiPostResponseToDomainResponse(bankResponseDto);

            return processPaymentResponse;

        }
    }
}
