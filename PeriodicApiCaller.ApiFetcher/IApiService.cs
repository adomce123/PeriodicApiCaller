namespace PeriodicApiCaller.ApiFetcher
{
    public interface IApiService
    {
        Task<string> FetchDataAsync();
    }
}