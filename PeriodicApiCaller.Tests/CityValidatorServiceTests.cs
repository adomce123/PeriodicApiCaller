using Microsoft.Extensions.Logging;
using Moq;
using PeriodicApiCaller.ApiFetcher.Interfaces;
using PeriodicApiCaller.Core;

namespace PeriodicApiCaller.Tests;

public class CityValidatorServiceTests
{
    private readonly Mock<IApiService> _mockApiService;
    private readonly Mock<ILogger<CityValidatorService>> _mockLogger;
    private readonly CityValidatorService _cityValidatorService;

    public CityValidatorServiceTests()
    {
        _mockApiService = new Mock<IApiService>();
        _mockLogger = new Mock<ILogger<CityValidatorService>>();
        _cityValidatorService = new CityValidatorService(_mockApiService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ValidateCities_ReturnsOnlyAvailableCities()
    {
        // Arrange
        var inputCities = new List<string> { "City1", "City2", "City3" };
        var availableCities = new List<string> { "City1", "City3" };
        _mockApiService.Setup(api => api.GetAllCities()).ReturnsAsync(availableCities);

        // Act
        var result = await _cityValidatorService.ValidateCities(inputCities);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains("City1", result);
        Assert.Contains("City3", result);
        Assert.DoesNotContain("City2", result);

        _mockLogger.Verify(log => log.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Fetching available cities..")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

        _mockLogger.Verify(log => log.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Provided city City2 is not available")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
