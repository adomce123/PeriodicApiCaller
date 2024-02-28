using PeriodicApiCaller.ApiFetcher.Models;
using PeriodicApiCaller.Persistence.Entities;

namespace PeriodicApiCaller.Core.Extensions
{
    public static class WeatherInfoExtensions
    {
        public static WeatherInfo ToEntity(this WeatherInfoDto dto)
        {
            return new WeatherInfo
            {
                City = dto.City,
                TemperatureC = dto.TemperatureC,
                Inserted = DateTime.UtcNow
            };
        }
    }
}
