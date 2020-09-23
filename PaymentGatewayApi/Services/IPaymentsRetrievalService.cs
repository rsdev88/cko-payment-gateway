﻿using PaymentGatewayApi.Models.RequestEntities;
using PaymentGatewayApi.Models.ResponseEntities;
using System.Threading.Tasks;

namespace PaymentGatewayApi.Services
{
    public interface IPaymentsRetrievalService
    {
        public Task<RetrievePaymentsResponse> RetrievePayments(RetrievePaymentsRequestDto model);
    }
}
