using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentGatewayApi.Exceptions;
using PaymentGatewayApi.Models.BankingDTOs.v1;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            var endpoint = this._configuration["bankingApi:paymentsEndpointPost"];
            StringContent content = new StringContent(JsonConvert.SerializeObject(bankDto), Encoding.UTF8, "application/json");

            return await this.SendHttpRequest<BankProcessPaymentResponseDto>(HttpVerbs.Post, endpoint, content);
        }

        public async Task<BankRetrievePaymentsResponseDto> RetrievePayments(BankRetrievePaymentsRequestDto bankDto)
        {
            var endpoint = string.Format(this._configuration["bankingApi:paymentsEndpointGet"], bankDto.TransactionId.ToString());

            return await this.SendHttpRequest<BankRetrievePaymentsResponseDto>(HttpVerbs.Get, endpoint);
        }

        private async Task<T> SendHttpRequest<T>(HttpVerbs callType, string endpoint, StringContent content = null)
        {
            this._httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage response;

                switch (callType)
                {
                    case HttpVerbs.Get:
                        response = this._httpClient.GetAsync(endpoint, new CancellationToken()).Result;
                        break;
                    case HttpVerbs.Post:
                        response = this._httpClient.PostAsync(endpoint, content, new CancellationToken()).Result;
                        break;
                    default:
                        throw new Exception();
                }

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var resultDto = JsonConvert.DeserializeObject<T>(jsonResult);
                    return resultDto;
                }

                throw new HttpException(HttpStatusCode.BadGateway,
                                        Resources.Resources.ErrorCode_BankingApiUnsuccesfulResponse,
                                        string.Format(Resources.Resources.ErrorMessage_BankingApiUnsuccesfulResponse, ((int)response.StatusCode).ToString(), response.ReasonPhrase));
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
