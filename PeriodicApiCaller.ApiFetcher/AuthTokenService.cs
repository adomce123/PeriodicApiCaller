using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PeriodicApiCaller.ApiFetcher.Interfaces;
using PeriodicApiCaller.ApiFetcher.Models.SerializerPolicies;
using PeriodicApiCaller.Configuration;
using System.Net.Http.Headers;
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

    public async Task<string> GetToken(bool refreshToken = false)
    {
        if (_token == null || refreshToken)
        {
            _token = await RetrieveAuthToken();
        }
        return _token;
    }

    private async Task<HttpContent> BuildContentForRequest()
    {
        var credentials = new
        {
            username = _configuration.Username,
            password = _configuration.Password
        };

        var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, credentials);
        memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position to the beginning

        var httpContent = new StreamContent(memoryStream);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return httpContent;
    }

    private async Task<string> RetrieveAuthToken()
    {
        var client = _httpClientFactory.CreateClient();
        var content = await BuildContentForRequest();

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

        await using var responseStream = await response.Content.ReadAsStreamAsync();

        var tokenResponse = await JsonSerializer
            .DeserializeAsync<TokenResponse>(
                responseStream, JsonSerializerSettings.CaseInsensitive);

        _token = tokenResponse?.Token ??
            throw new UnauthorizedAccessException("Failed to retrieve authentication token");

        _logger.LogInformation("Authentication token retrieved");

        return _token;
    }
}
