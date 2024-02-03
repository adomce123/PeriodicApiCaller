using Microsoft.Extensions.Logging;

namespace PeriodicApiCaller.ApiFetcher
{
    public class PeriodicApiFetcher : IPeriodicApiFetcher, IDisposable
    {
        private readonly IApiService _apiService;
        private readonly ILogger<PeriodicApiFetcher> _logger;
        private Timer? _timer;
        private bool _disposed = false;
        private const int _interval = 15;

        public PeriodicApiFetcher(IApiService apiService, ILogger<PeriodicApiFetcher> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task StartFetching(string city)
        {
            _timer = new Timer(async _ =>
            {
                try
                {
                    var result = await _apiService.GetCityWeather(city);
                    _logger.LogInformation($"Fetched: {result}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failure in Periodic Fetcher");
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(_interval));

            await Task.CompletedTask;
        }

        public void StopFetching()
        {
            _timer?.Change(Timeout.Infinite, 0);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
