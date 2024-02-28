namespace PeriodicApiCaller.ApiFetcher.Models
{
    public class WeatherInfoDto
    {
        public DateTime Date { get; set; }
        public string? City { get; set; }
        public decimal TemperatureC { get; set; }
    }
}
