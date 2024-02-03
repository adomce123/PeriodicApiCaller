namespace PeriodicApiCaller.ApiFetcher
{
    public interface IApiService
    {
        Task<string> GetAllCities();
        Task<string> GetCityWeather(string city);
    }
}