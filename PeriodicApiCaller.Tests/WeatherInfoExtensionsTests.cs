using PeriodicApiCaller.ApiFetcher.Models;
using PeriodicApiCaller.Core.Extensions;

namespace PeriodicApiCaller.Core.Tests.Extensions;

public class WeatherInfoExtensionsTests
{
    [Fact]
    public void ToEntity_ConvertsDtoToEntityCorrectly()
    {
        // Arrange
        var dto = new WeatherInfoDto
        {
            City = "TestCity",
            TemperatureC = 25.5m
        };

        // Act
        var entity = dto.ToEntity();

        // Assert
        Assert.Equal(dto.City, entity.City);
        Assert.Equal(dto.TemperatureC, entity.TemperatureC);

        var utcNow = DateTime.UtcNow;
        Assert.True((utcNow - entity.Inserted).TotalSeconds < 1);
    }
}
