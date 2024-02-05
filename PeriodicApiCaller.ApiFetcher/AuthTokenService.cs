using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PeriodicApiCaller.ApiFetcher.Interfaces;
using PeriodicApiCaller.ApiFetcher.Models.SerializerPolicies;
using PeriodicApiCaller.Configuration;
using System.Text;
using System.Text.Json;

namespace PeriodicApiCaller.ApiFetcher;

public class AuthTokenService : IAuthTokenService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApiServiceSettings _configuration;
    private readonly ILogger<AuthTokenService> _logger;
    private string? _token;

    public AuthTokenService(
        IHttpClientFactory httpClientFactory,
        IOptions<ApiServiceSettings> options,
        ILogger<AuthTokenService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = options.Value;
        _logger = logger;
    }

    public async Task<string> GetToken()
    {
        if (_token == null)
        {
            _token = await RetrieveAuthToken();
        }
        return _token;
    }

    private StringContent BuildContentForRequest()
    {
        var credentials = new
        {
            username = _configuration.Username,
            password = _configuration.Password
        };

        return new StringContent(
            JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");
    }

    private async Task<string> RetrieveAuthToken()
    {
        var client = _httpClientFactory.CreateClient();
        var content = BuildContentForRequest();

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
            .DeserializeAsync<TokenResponse>(
                responseStream, JsonSerializerSettings.CaseInsensitive);

        _token = tokenResponse?.Token ??
            throw new UnauthorizedAccessException("Failed to retrieve authentication token");

        return _token;
    }
}
