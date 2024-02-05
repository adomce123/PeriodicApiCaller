namespace PeriodicApiCaller.ApiFetcher.Interfaces
{
    public interface IAuthTokenService
    {
        Task<string> GetToken(bool refreshToken = false);
    }
}
