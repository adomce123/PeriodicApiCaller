using PeriodicApiCaller.Persistence.Entities;

namespace PeriodicApiCaller.Persistence.Repositories.Interfaces
{
    public interface IWeatherInfoRepository
    {
        Task SaveWeatherInfoAsync(WeatherInfo weatherInfo, CancellationToken cts);
    }
}