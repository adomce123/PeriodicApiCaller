using PeriodicApiCaller.ApiFetcher;

namespace PeriodicApiCaller
{
    internal class InputProcessor : IInputProcessor
    {
        private readonly IPeriodicApiFetcher _periodicApiFetcher;

        public InputProcessor(IPeriodicApiFetcher periodicApiFetcher)
        {
            _periodicApiFetcher = periodicApiFetcher;
        }

        public async Task ReadInput()
        {
            await _periodicApiFetcher.StartFetching();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
