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
        _logger.LogInformation($"Starting....");

        _fetchingTasks.Clear();

        foreach (var city in cities)
        {
            var task = StartFetchingForCity(city, cts);
            _fetchingTasks.Add(task);
        }

        await Task.WhenAll(_fetchingTasks);
    }

    private async Task StartFetchingForCity(string city, CancellationToken cts)
    {
        while (!cts.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var apiService = scope.ServiceProvider.GetRequiredService<IApiService>();
                var repository = scope.ServiceProvider.GetRequiredService<IWeatherInfoRepository>();

                try
                {
                    var result = await apiService.GetCityWeather(city);

                    await repository.SaveWeatherInfoAsync(result.ToEntity());

                    _logger.LogInformation($"Fetched and saved data for: {result.City}.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error starting job for {city}.");
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(FetchInterval), cts);
        }
    }
}
