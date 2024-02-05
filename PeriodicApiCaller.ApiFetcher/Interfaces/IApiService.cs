using PeriodicApiCaller.ApiFetcher.Models;

namespace PeriodicApiCaller.ApiFetcher.Interfaces
{
    public interface IApiService
    {
        Task<IEnumerable<string>> GetAllCities();
        Task<WeatherInfoDto> GetCityWeather(string city);
    }
}