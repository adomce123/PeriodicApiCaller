using PeriodicApiCaller.Core.Interfaces;

namespace PeriodicApiCaller.Core
{
    public class ValidatedCitiesProvider : IValidatedCitiesProvider
    {
        private readonly ICityValidator _cityValidator;
        private IEnumerable<string> _validatedCities = Enumerable.Empty<string>();
        private bool _isInitialized = false;

        public ValidatedCitiesProvider(ICityValidator cityValidator)
        {
            _cityValidator = cityValidator;
        }

        public async Task<IEnumerable<string>> GetValidatedCitiesAsync()
        {

            if (!_isInitialized)
            {
                _validatedCities = await _cityValidator.ValidateCities(cities);
                _isInitialized = true;
            }

            return _validatedCities;
        }
    }
}
