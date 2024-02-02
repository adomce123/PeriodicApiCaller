using PeriodicApiCaller.ApiFetcher.Models.SerializerPolicies;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PeriodicApiCaller.ApiFetcher
{
    public partial class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _serializerOptions;
        private string? _currentToken;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowerCaseNamingPolicy(),
            };
        }

        public async Task<string> FetchDataAsync()
        {
            if (string.IsNullOrEmpty(_currentToken))
            {
                await FetchAuthTokenAsync();
            }

            try
            {
                return await AttemptFetchWeatherDataAsync();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine($"Failed to authenticate, trying to refresh token");
                await FetchAuthTokenAsync();
                return await AttemptFetchWeatherDataAsync();
            }
        }

        private async Task<string> AttemptFetchWeatherDataAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _currentToken);

            var response = await client.GetAsync("https://xxx/api/weathers/Vilnius");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        private async Task<string> FetchAuthTokenAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var credentials = new
            {
                //CONFIG
                username = "xxx",
                password = "passwrod"
            };

            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");
            var response = new HttpResponseMessage();

            try
            {
                response = await client.PostAsync("https://xxx/api/authorize", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Failed to authenticate: {ex.Message}");
            }

            var responseStream = await response.Content.ReadAsStreamAsync();
            var tokenResponse = await JsonSerializer
                .DeserializeAsync<TokenResponse>(responseStream, _serializerOptions);

            _currentToken = tokenResponse?.Token ??
                throw new UnauthorizedAccessException("Failed to retrieve authentication token");

            return _currentToken;
        }
    }
}
