namespace PeriodicApiCaller.Core.Interfaces
{
    public interface ICityValidatorService
    {
        Task<IEnumerable<string>> ValidateCities(IEnumerable<string> cities);
    }
}
