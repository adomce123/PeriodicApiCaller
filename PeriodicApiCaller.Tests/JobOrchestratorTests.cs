using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PeriodicApiCaller.ApiFetcher.Interfaces;
using PeriodicApiCaller.ApiFetcher.Models;
using PeriodicApiCaller.Persistence.Entities;
using PeriodicApiCaller.Persistence.Repositories.Interfaces;

namespace PeriodicApiCaller.Core.Tests;

public class JobOrchestratorTests
{
    private readonly Mock<IServiceScopeFactory> _mockScopeFactory;
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<IApiService> _mockApiService;
    private readonly Mock<IWeatherInfoRepository> _mockRepository;
    private readonly Mock<ILogger<JobOrchestrator>> _mockLogger;
    private readonly JobOrchestrator _jobOrchestrator;

    public JobOrchestratorTests()
    {
        _mockScopeFactory = new Mock<IServiceScopeFactory>();
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockApiService = new Mock<IApiService>();
        _mockRepository = new Mock<IWeatherInfoRepository>();
        _mockLogger = new Mock<ILogger<JobOrchestrator>>();

        var mockScope = new Mock<IServiceScope>();
        mockScope.Setup(x => x.ServiceProvider).Returns(_mockServiceProvider.Object);
        _mockScopeFactory.Setup(x => x.CreateScope()).Returns(mockScope.Object);

        _mockServiceProvider.Setup(x => x.GetService(typeof(IApiService))).Returns(_mockApiService.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IWeatherInfoRepository))).Returns(_mockRepository.Object);

        _jobOrchestrator = new JobOrchestrator(_mockScopeFactory.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task StartFetchingForCities_FetchesAndSavesDataSuccessfully()
    {
        // Arrange
        var cities = new List<string> { "City1", "City2" };
        var cts = new CancellationTokenSource();

        _mockApiService.Setup(api => api.GetCityWeather(It.IsAny<string>()))
            .ReturnsAsync(new WeatherInfoDto());

        _mockRepository
            .Setup(repo => repo.SaveWeatherInfoAsync(It.IsAny<WeatherInfo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var task = _jobOrchestrator.StartFetchingForCities(cities, cts.Token);

        cts.Cancel(); // Exit loop

        await task;

        // Assert
        _mockApiService.Verify(api => api.GetCityWeather(It.IsAny<string>()), Times.AtLeastOnce());

        _mockRepository.Verify(repo => repo.SaveWeatherInfoAsync(
            It.IsAny<WeatherInfo>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
    }

    [Fact]
    public async Task StartFetchingForCities_CancelsGracefully()
    {
        // Arrange
        var cities = new List<string> { "City1", "City2" };
        var cancellationTokenSource = new CancellationTokenSource();

        _mockApiService
            .Setup(api => api.GetCityWeather(It.IsAny<string>()))
            .ReturnsAsync(new WeatherInfoDto());

        // Act
        var fetchTask = _jobOrchestrator
            .StartFetchingForCities(cities, cancellationTokenSource.Token);

        // Cancel after a short delay to simulate user cancellation
        cancellationTokenSource.CancelAfter(100);

        await fetchTask;

        // Assert
        _mockApiService.Verify(api => api.GetCityWeather(It.IsAny<string>()), Times.AtLeastOnce());
        _mockLogger.Verify(log => log.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("was canceled")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.AtLeastOnce());
    }
}
