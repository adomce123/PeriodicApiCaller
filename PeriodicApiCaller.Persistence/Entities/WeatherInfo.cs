namespace PeriodicApiCaller.Persistence.Entities
{
    public class WeatherInfo
    {
        public int Id { get; set; }
        public string? City { get; set; }
        public decimal TemperatureC { get; set; }
        public DateTime Inserted { get; set; }
    }
}
