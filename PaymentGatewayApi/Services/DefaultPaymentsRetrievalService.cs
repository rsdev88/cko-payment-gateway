using PaymentGatewayApi.Mappers;
using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using PaymentGatewayApi.Services.Banking;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Services
{
    public class DefaultPaymentsRetrievalService : IPaymentsRetrievalService
    {
        private readonly IDtoMapper _dtoMapper;
        private readonly IBankingService _bankingService;

        public DefaultPaymentsRetrievalService(IDtoMapper dtoMapper, IBankingService bankingService)
        {
            this._dtoMapper = dtoMapper;
            this._bankingService = bankingService;
        }

        public async Task<RetrievePaymentsResponse> RetrievePayments(RetrievePaymentsRequestDto model)
        {
            var bankRequestDto = this._dtoMapper.MapRetrievePaymentsRequestModelToBankDto(model);
            var bankResponseDto = await this._bankingService.RetrievePayments(bankRequestDto);
            var retrievePaymentResponse = this._dtoMapper.MapBankApiGetResponseToDomainResponse(bankResponseDto);

            return retrievePaymentResponse;
        }
    }
}
