namespace PeriodicApiCaller.Core.Interfaces
{
    public interface ICityValidator
    {
        Task<IEnumerable<string>> ValidateCities(IEnumerable<string> cities);
    }
}
