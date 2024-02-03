using Microsoft.Extensions.Logging;
using PeriodicApiCaller.ApiFetcher;

namespace PeriodicApiCaller.Core
{
    public class CityValidatorService : ICityValidatorService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<CityValidatorService> _logger;

        public CityValidatorService(IApiService apiService, ILogger<CityValidatorService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> ValidateCities(IList<string> cities)
        {
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
