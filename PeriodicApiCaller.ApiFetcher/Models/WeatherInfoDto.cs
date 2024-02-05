namespace PeriodicApiCaller.ApiFetcher.Models
{
    public class WeatherInfoDto
    {
        public string? City { get; set; }
        public double Temperature { get; set; }
        public double Precipitation { get; set; }
        public double WindSpeed { get; set; }
        public string? Summary { get; set; }
    }
}
