using CommandLine;
using PeriodicApiCaller.Core.Interfaces;
using PeriodicApiCaller.Interfaces;

namespace PeriodicApiCaller
{
    internal class InputProcessor : IInputProcessor
    {
        private readonly IJobOrchestrator _jobOrchestrator;
        private readonly ICityValidatorService _cityValidatorService;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public InputProcessor(
            IJobOrchestrator jobOrchestrator,
            ICityValidatorService cityValidatorService)
        {
            _jobOrchestrator = jobOrchestrator;
            _cityValidatorService = cityValidatorService;

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Cancellation requested. Stopping...");
                _cts.Cancel();
                e.Cancel = true; // Prevent the process from terminating immediately.
            };
        }

        public async Task ReadInput(string[] args)
        {
            var parserResult = Parser.Default.ParseArguments<InputOptions>(args);
            IEnumerable<string> cities = Enumerable.Empty<string>();

            parserResult.WithParsed(options =>
            {
                cities = options.Cities.Select(city => city.Trim());
            });

            cities = new[] { "Vilnius", "Lala", "Kaunas" };

            var validatedCities = await _cityValidatorService.ValidateCities(cities);

            if (validatedCities.Any())
            {
                await _jobOrchestrator.StartFetchingForCities(validatedCities, _cts.Token);
            }
        }
    }
}
