using System.Text.Json.Serialization;

namespace WebAPI.Models.Paypal
{
    public class PaypalAuthResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = null!;
    }
}
