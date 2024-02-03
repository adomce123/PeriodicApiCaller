namespace PeriodicApiCaller.Core
{
    public interface ICityValidatorService
    {
        Task<IEnumerable<string>> ValidateCities(IList<string> cities);
    }
}
