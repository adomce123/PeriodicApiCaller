using Microsoft.Extensions.Logging;
using PeriodicApiCaller.ApiFetcher.Interfaces;
using PeriodicApiCaller.Core.Interfaces;

namespace PeriodicApiCaller.Core
{
    public class CityValidator : ICityValidator
    {
        private readonly IApiService _apiService;
        private readonly ILogger<CityValidator> _logger;

        public CityValidator(IApiService apiService, ILogger<CityValidator> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> ValidateCities(IEnumerable<string> cities)
        {
            _logger.LogInformation($"Fetching available cities..");

            var availableCities = await _apiService.GetAllCities();

            var validCities = new List<string>();

            foreach (var city in cities)
            {
                if (availableCities.Contains(city))
                {
                    validCities.Add(city);
                }
                else
                {
                    _logger.LogWarning($"Provided city {city} is not available");
                }
            }

            return validCities;
        }
    }
}
