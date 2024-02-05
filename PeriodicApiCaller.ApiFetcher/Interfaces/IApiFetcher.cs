namespace PeriodicApiCaller.ApiFetcher.Interfaces
{
    public interface IApiFetcher
    {
        Task<string> AttemptFetchWeatherData(string url);
    }
}
