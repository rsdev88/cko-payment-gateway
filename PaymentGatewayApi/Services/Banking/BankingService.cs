using Newtonsoft.Json;
using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Models.BankingDTOs;
using PaymentGatewayApi.Services.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Services.Banking
{
    public class BankingService : IBankingService
    {
        private readonly IBankingApiConfiguration _configuration;

        public BankingService(IBankingApiConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<BankProcessPaymentResponseDto> ProcessPayment(BankProcessPaymentRequestDto bankDto)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(this._configuration.BaseUrl),                
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StringContent content = new StringContent(JsonConvert.SerializeObject(bankDto), Encoding.UTF8, "application/json");

            try
            {
                var response = client.PostAsync(this._configuration.PaymentsEndpoint, content, new CancellationToken()).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var resultDto = JsonConvert.DeserializeObject<BankProcessPaymentResponseDto>(jsonResult);
                    return resultDto;
                }

                throw new HttpException(HttpStatusCode.BadGateway, 
                                        Resources.Resources.ErrorCode_BankingApiUnsuccesfulResponse, 
                                        string.Format(Resources.Resources.ErrorMessage_BankingApiUnsuccesfulResponse, response.ReasonPhrase));
                                
            }
            catch (Exception ex)
            {
                //Todo: logging with ex
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        Resources.Resources.ErrorCode_BankingApiUnexpectedError,
                                        Resources.Resources.ErrorMessage_BankingApiUnexpectedError);
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
