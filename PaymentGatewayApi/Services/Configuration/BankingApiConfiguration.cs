namespace PaymentGatewayApi.Services.Configuration
{
    public class BankingApiConfiguration : IBankingApiConfiguration
    {
        public string BaseUrl { get; }

        public string PaymentsEndpoint { get; }
    }
}
