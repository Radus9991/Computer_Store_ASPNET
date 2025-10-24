using ApplicationCore.Models;
using System.Net.Http.Headers;
using WebAPI.Models.Paypal;

namespace WebAPI.Services
{
    public class PaypalService : IPaypalService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public PaypalService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            this.clientFactory = clientFactory;
            this.configuration = configuration;
            httpClient = clientFactory.CreateClient();
        }

        public async Task<string> CreateOrder(List<Computer> computers)
        {
            if(computers == null || computers.Count == 0)
            {
                return null;
            }

            var client = await GetAuthorizedHttpClient();
            var response = await client.PostAsJsonAsync("https://api-m.sandbox.paypal.com/v2/checkout/orders", new
            {
                purchase_units = computers.Select(c => new
                {
                    amount = new
                    {
                        currency_code = "EUR",
                        value = c.Price
                    },
                    reference_id = Guid.NewGuid().ToString()
                }).ToArray(),
                intent = "CAPTURE",
            });

            var responseBody = await response.Content.ReadFromJsonAsync<PaypalResponse>();

            return responseBody.Id;
        }

        public async Task<bool> IsPaymentFinished(string paypalId)
        {
            var client = await GetAuthorizedHttpClient();
            var response = await client.GetAsync($"https://api-m.sandbox.paypal.com/v2/checkout/orders/{paypalId}");
            var responseBody = await response.Content.ReadFromJsonAsync<PaypalResponse>();
            if(responseBody == null)
            {
                return false;
            }
            return responseBody.Status == "APPROVED";
        }

        private async Task<HttpClient> GetAuthorizedHttpClient()
        {
            string? accessToken = await GenerateOAuthToken(configuration["Paypal:ClientId"], configuration["Paypal:ClientSecret"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return httpClient;
        }

        private async Task<string?> GenerateOAuthToken(string? clientId, string? clientSecret)
        {
            var url = "https://api-m.sandbox.paypal.com/v1/oauth2/token";
            var authToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            var body = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await httpClient.PostAsync(url, body);

            if(response.IsSuccessStatusCode)
            {
                var paypalAuth = await response.Content.ReadFromJsonAsync<PaypalAuthResponse>();
                return paypalAuth.AccessToken;
            } else
            {
                return null;
            }
        }
    }
}
