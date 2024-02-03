using PeriodicApiCaller.ApiFetcher;

namespace PeriodicApiCaller.Core
{
    public class WeatherDataOrchestrator : IWeatherDataOrchestrator
    {
        private readonly IPeriodicApiFetcher _periodicApiFetcher;

        public WeatherDataOrchestrator(IPeriodicApiFetcher periodicApiFetcher)
        {
            _periodicApiFetcher = periodicApiFetcher;
        }

        public async Task Orchestrate(IEnumerable<string> validatedCities)
        {
            foreach (var city in validatedCities)
            {
                await _periodicApiFetcher.StartFetching(city);
            }
        }
    }
}
