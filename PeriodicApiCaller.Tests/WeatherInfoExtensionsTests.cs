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
            Temperature = 25.5,
            Precipitation = 10.2,
            WindSpeed = 5.5,
            Summary = "Sunny"
        };

        // Act
        var entity = dto.ToEntity();

        // Assert
        Assert.Equal(dto.City, entity.City);
        Assert.Equal(dto.Temperature, entity.Temperature);
        Assert.Equal(dto.Precipitation, entity.Precipitation);
        Assert.Equal(dto.WindSpeed, entity.WindSpeed);
        Assert.Equal(dto.Summary, entity.Summary);

        var utcNow = DateTime.UtcNow;
        Assert.True((utcNow - entity.Inserted).TotalSeconds < 1);
    }
}
