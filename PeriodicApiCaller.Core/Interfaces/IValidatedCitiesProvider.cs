namespace PeriodicApiCaller.Core.Interfaces
{
    public interface IValidatedCitiesProvider
    {
        Task<IEnumerable<string>> GetValidatedCitiesAsync();
    }
}