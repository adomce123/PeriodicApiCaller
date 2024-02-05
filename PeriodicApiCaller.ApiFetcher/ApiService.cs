using Microsoft.Extensions.Options;
using PeriodicApiCaller.ApiFetcher.Interfaces;
using PeriodicApiCaller.ApiFetcher.Models;
using PeriodicApiCaller.ApiFetcher.Models.SerializerPolicies;
using PeriodicApiCaller.Configuration;
using System.Text.Json;

namespace PeriodicApiCaller.ApiFetcher
{
    public class ApiService : IApiService
    {
        private readonly ApiServiceSettings _configuration;
        private readonly IApiFetcher _apiFetcher;

        public ApiService(IOptions<ApiServiceSettings> options, IApiFetcher apiFetcher)
        {
            _configuration = options.Value;
            _apiFetcher = apiFetcher;
        }

        public async Task<IEnumerable<string>> GetAllCities()
        {
            var url = _configuration.BaseUrl + _configuration.CitiesEndpoint;

            await using var responseStream = await _apiFetcher.AttemptFetchWeatherData(url);

            var cities = await JsonSerializer
                .DeserializeAsync<IEnumerable<string>>(responseStream, JsonSerializerSettings.CaseInsensitive)
                ?? throw new FormatException(
                    "Was not able to deserialize response to list of cities");

            return cities;
        }

        public async Task<WeatherInfoDto> GetCityWeather(string city)
        {
            var url = _configuration.BaseUrl + _configuration.CityWeatherEndpoint + $"/{city}";

            await using var responseStream = await _apiFetcher.AttemptFetchWeatherData(url);

            var weatherInfoDto = await JsonSerializer
                .DeserializeAsync<WeatherInfoDto>(responseStream, JsonSerializerSettings.CaseInsensitive)
                ?? throw new FormatException(
                    $"Was not able to deserialize response to {typeof(WeatherInfoDto)}");

            return weatherInfoDto;
        }
    }
}
