namespace PeriodicApiCaller.ApiFetcher
{
    public interface IPeriodicApiFetcher
    {
        Task StartFetching(string city);
        void StopFetching();
    }
}
