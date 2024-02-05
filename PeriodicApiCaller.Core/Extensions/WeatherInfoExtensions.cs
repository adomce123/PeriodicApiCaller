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
                Temperature = dto.Temperature,
                Precipitation = dto.Precipitation,
                WindSpeed = dto.WindSpeed,
                Summary = dto.Summary,
                Inserted = DateTime.UtcNow
            };
        }
    }
}
