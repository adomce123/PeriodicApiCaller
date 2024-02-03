using PeriodicApiCaller.Core;

namespace PeriodicApiCaller
{
    internal class InputProcessor : IInputProcessor
    {
        private readonly IWeatherDataOrchestrator _weatherDataOrchestrator;
        private readonly ICityValidatorService _cityValidatorService;

        public InputProcessor(
            IWeatherDataOrchestrator weatherDataOrchestrator,
            ICityValidatorService cityValidatorService)
        {
            _weatherDataOrchestrator = weatherDataOrchestrator;
            _cityValidatorService = cityValidatorService;
        }

        public async Task ReadInput()
        {
            var inputCities = new List<string>
            {
                "Vienna",
                "Vilnius",
                "N'Djamena",
                "Stockholm",
                "Kaunas"
            };

            var validatedCities = await _cityValidatorService.ValidateCities(inputCities);

            if (validatedCities.Any())
            {
                await _weatherDataOrchestrator.Orchestrate(validatedCities);

            }
            Console.ReadKey();
        }
    }
}
