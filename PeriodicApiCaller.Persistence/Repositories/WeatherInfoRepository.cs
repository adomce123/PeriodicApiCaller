using PeriodicApiCaller.Persistence.Entities;
using PeriodicApiCaller.Persistence.Repositories.Interfaces;

namespace PeriodicApiCaller.Persistence.Repositories
{
    public class WeatherInfoRepository : IWeatherInfoRepository
    {
        private readonly WeatherInfoDbContext _context;

        public WeatherInfoRepository(WeatherInfoDbContext context)
        {
            _context = context;
        }

        public async Task SaveWeatherInfoAsync(WeatherInfo weatherInfo)
        {
            _context.WeatherInfos.Add(weatherInfo);
            await _context.SaveChangesAsync();
        }
    }
}
