using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PeriodicApiCaller.ApiFetcher.Models.SerializerPolicies;
using PeriodicApiCaller.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PeriodicApiCaller.ApiFetcher
{
    public partial class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiService> _logger;

        private readonly ApiServiceSettings _configuration;
        private readonly JsonSerializerOptions _serializerOptions;
        private string? _currentToken;

        public ApiService(
            IHttpClientFactory httpClientFactory,
            IOptions<ApiServiceSettings> options,
            ILogger<ApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = options.Value;

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowerCaseNamingPolicy(),
            };
        }

        public async Task<string> GetAllCities()
        {
            await CheckAndGetToken();

            var url = _configuration.BaseUrl + _configuration.CitiesEndpoint;

            try
            {
                return await AttemptFetchWeatherDataAsync(url);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError($"Failed to authenticate, trying to refresh token");
                await GetAuthToken();
                return await AttemptFetchWeatherDataAsync(url);
            }
        }

        public async Task<string> GetCityWeather(string city)
        {
            await CheckAndGetToken();

            var url = _configuration.BaseUrl + _configuration.CityWeatherEndpoint + $"/{city}";

            try
            {
                return await AttemptFetchWeatherDataAsync(url);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError($"Failed to authenticate, trying to refresh token");
                await GetAuthToken();
                return await AttemptFetchWeatherDataAsync(url);
            }
        }

        private async Task<string> AttemptFetchWeatherDataAsync(string url)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _currentToken);

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        private async Task CheckAndGetToken()
        {
            if (string.IsNullOrEmpty(_currentToken))
            {
                await GetAuthToken();
            }
        }

        private async Task<string> GetAuthToken()
        {
            var client = _httpClientFactory.CreateClient();
            var credentials = new
            {
                username = _configuration.Username,
                password = _configuration.Password
            };

            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");
            var response = new HttpResponseMessage();

            try
            {
                response = await client.PostAsync(
                    _configuration.BaseUrl + _configuration.AuthEndpoint, content);

                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Failed to authenticate: {ex.Message}");
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
