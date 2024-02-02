namespace PeriodicApiCaller.ApiFetcher
{
    public interface IPeriodicApiFetcher
    {
        Task StartFetching();
        void StopFetching();
    }
}
