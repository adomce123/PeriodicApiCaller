namespace PeriodicApiCaller.ApiFetcher.Interfaces
{
    public interface IApiFetcher
    {
        Task<Stream> AttemptFetchWeatherData(string url);
    }
}
