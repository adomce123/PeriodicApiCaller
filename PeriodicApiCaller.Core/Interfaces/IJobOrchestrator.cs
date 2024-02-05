namespace PeriodicApiCaller.Core.Interfaces
{
    public interface IJobOrchestrator
    {
        Task StartFetchingForCities(IEnumerable<string> cities, CancellationToken cts);
    }
}
