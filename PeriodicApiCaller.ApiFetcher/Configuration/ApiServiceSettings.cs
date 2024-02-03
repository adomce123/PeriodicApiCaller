namespace PeriodicApiCaller.Configuration
{
    public class ApiServiceSettings
    {
        public string? BaseUrl { get; set; }
        public string? AuthEndpoint { get; set; }
        public string? CitiesEndpoint { get; set; }
        public string? CityWeatherEndpoint { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
