using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PeriodicApiCaller.ApiFetcher.Interfaces;
using PeriodicApiCaller.Core.Extensions;
using PeriodicApiCaller.Core.Interfaces;
using PeriodicApiCaller.Persistence.Repositories.Interfaces;

namespace PeriodicApiCaller.Core;

public class JobOrchestrator : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<JobOrchestrator> _logger;
    private readonly IValidatedCitiesProvider _validatedCitiesProvider;
    private readonly List<Task> _fetchingTasks;
    private const int FetchInterval = 15;

    public JobOrchestrator(
        IServiceScopeFactory scopeFactory,
        ILogger<JobOrchestrator> logger,
        IValidatedCitiesProvider validatedCitiesProvider)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _validatedCitiesProvider = validatedCitiesProvider;
        _fetchingTasks = new List<Task>();
    }

    public async Task StartFetching(IEnumerable<string> cities, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting data fetching..");

        var validatedCities = await _validatedCitiesProvider.GetValidatedCitiesAsync();
        foreach (var city in validatedCities)
        {
            _fetchingTasks.Add(StartFetchingForCity(city, stoppingToken));
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

    private async Task StartFetchingForCity(string city, CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var apiService = scope.ServiceProvider.GetRequiredService<IApiService>();
                    var repository = scope.ServiceProvider.GetRequiredService<IWeatherInfoRepository>();

                    var result = await apiService.GetCityWeather(city);

                    await repository.SaveWeatherInfoAsync(result.ToEntity(), stoppingToken);

                    _logger.LogInformation($"Fetched and saved weather data - " +
                        $"City: {result.City}, Temperature: {result.TemperatureC}");
                }

                await Task.Delay(TimeSpan.FromSeconds(FetchInterval), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation($"Fetching for city {city} was canceled.");
        }
    }
}
