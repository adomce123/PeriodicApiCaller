namespace PeriodicApiCaller.Core
{
    public interface IWeatherDataOrchestrator
    {
        Task Orchestrate(IEnumerable<string> validatedCities);
    }
}
