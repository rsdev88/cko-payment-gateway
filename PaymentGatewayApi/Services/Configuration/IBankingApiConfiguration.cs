namespace PaymentGatewayApi.Services.Configuration
{
    public interface IBankingApiConfiguration
    {
        string BaseUrl { get; }

        string PaymentsEndpoint { get; }
    }
}
