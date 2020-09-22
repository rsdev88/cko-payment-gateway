using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Models.BankingDTOs;
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
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public BankingService(IConfiguration configuration, HttpClient httpClient)
        {
            this._configuration = configuration;
            this._httpClient = httpClient;
        }

        public async Task<BankProcessPaymentResponseDto> ProcessPayment(BankProcessPaymentRequestDto bankDto)
        {
            this._httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StringContent content = new StringContent(JsonConvert.SerializeObject(bankDto), Encoding.UTF8, "application/json");

            try
            {
                var response = this._httpClient.PostAsync(this._configuration["bankingApi.paymentsEndpoint"], content, new CancellationToken()).Result;

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
            catch (HttpException)
            {
                throw;
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
                this._httpClient.Dispose();
            }
        }
    }
}
