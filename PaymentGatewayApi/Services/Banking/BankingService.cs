using ApiSharedLibrary.Resources;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BankingService> _logger;

        public BankingService(IConfiguration configuration, HttpClient httpClient, ILogger<BankingService> logger)
        {
            this._configuration = configuration;
            this._httpClient = httpClient;
            this._logger = logger;
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
                                        Resources.ErrorCode_BankingApiUnsuccesfulResponse,
                                        string.Format(Resources.ErrorMessage_BankingApiUnsuccesfulResponse, ((int)response.StatusCode).ToString(), response.ReasonPhrase));
            }
            catch (HttpException)
            {
                //Nothing to do here - the global exception middleware will catch, log and handle the error.
                throw;
            }

            catch (Exception ex)
            {
                //The global exception middleware will also catch and log this HTTP exception but we also want to log the original exception message.
                this._logger.LogError(Resources.Logging_BankingServiceUnexpected, ex.Message);
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        Resources.ErrorCode_BankingApiUnexpectedError,
                                        Resources.ErrorMessage_BankingApiUnexpectedError);
            }
            finally
            {
                this._httpClient.Dispose();
            }
        }
    }
}
