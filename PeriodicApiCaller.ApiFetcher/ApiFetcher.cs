using Microsoft.Extensions.Logging;
using PeriodicApiCaller.ApiFetcher.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace PeriodicApiCaller.ApiFetcher
{
    public partial class ApiFetcher : IApiFetcher
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthTokenService _authTokenService;
        private readonly ILogger<ApiFetcher> _logger;
        private string? _currentToken;

        public ApiFetcher(
            IHttpClientFactory httpClientFactory,
            IAuthTokenService authTokenService,
            ILogger<ApiFetcher> logger)
        {
            _httpClientFactory = httpClientFactory;
            _authTokenService = authTokenService;
            _logger = logger;
        }

        public async Task<string> AttemptFetchWeatherData(string url)
        {
            _currentToken = await _authTokenService.GetToken();

            try
            {
                return await FetchWeatherData(url);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError($"Failed to authenticate, trying to refresh token");

                _currentToken = await _authTokenService.GetToken();
                return await FetchWeatherData(url);
            }
        }

        private async Task<string> FetchWeatherData(string url)
        {
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _currentToken);

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
