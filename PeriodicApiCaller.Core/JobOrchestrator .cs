using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeriodicApiCaller.ApiFetcher.Interfaces;
using PeriodicApiCaller.Core.Extensions;
using PeriodicApiCaller.Core.Interfaces;
using PeriodicApiCaller.Persistence.Repositories.Interfaces;

namespace PeriodicApiCaller.Core;

public class JobOrchestrator : IJobOrchestrator
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<JobOrchestrator> _logger;
    private readonly List<Task> _fetchingTasks;
    private const int FetchInterval = 15;

    public JobOrchestrator(
        IServiceScopeFactory scopeFactory,
        ILogger<JobOrchestrator> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _fetchingTasks = new List<Task>();
    }

    public async Task StartFetchingForCities(IEnumerable<string> cities, CancellationToken cts)
    {
        _logger.LogInformation($"Starting data fetching..");

        _fetchingTasks.Clear();

        foreach (var city in cities)
        {
            var task = StartFetchingForCity(city, cts);
            _fetchingTasks.Add(task);
        }

        try
        {
            await Task.WhenAll(_fetchingTasks);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Data fetching was canceled.");
        }
    }

    private async Task StartFetchingForCity(string city, CancellationToken cts)
    {
        try
        {
            while (!cts.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var apiService = scope.ServiceProvider.GetRequiredService<IApiService>();
                    var repository = scope.ServiceProvider.GetRequiredService<IWeatherInfoRepository>();

                    var result = await apiService.GetCityWeather(city);

                    await repository.SaveWeatherInfoAsync(result.ToEntity(), cts);

                    _logger.LogInformation($"Fetched and saved weather data - " +
                        $"City: {result.City}, Temperature: {result.Temperature}, " +
                        $"Precipitation: {result.Precipitation}, WindSpeed: {result.WindSpeed}");
                }

                await Task.Delay(TimeSpan.FromSeconds(FetchInterval), cts);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation($"Fetching for city {city} was canceled.");
        }
    }
}
