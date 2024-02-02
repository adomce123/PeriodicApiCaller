namespace PeriodicApiCaller.ApiFetcher
{
    public class PeriodicApiFetcher : IPeriodicApiFetcher, IDisposable
    {
        private readonly IApiService _apiService;
        private Timer? _timer;
        private bool _disposed = false;
        private const int _interval = 15;

        public PeriodicApiFetcher(IApiService apiService)
        {
            _apiService = apiService;
        }

        public Task StartFetching()
        {
            _timer = new Timer(async _ =>
            {
                try
                {
                    var result = await _apiService.FetchDataAsync();
                    await Console.Out.WriteLineAsync($"Fetched: {result}");
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(_interval));

            return Task.CompletedTask;
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
