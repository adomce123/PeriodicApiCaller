namespace PeriodicApiCaller.Persistence.Entities
{
    public class WeatherInfo
    {
        public int Id { get; set; }
        public string? City { get; set; }
        public double Temperature { get; set; }
        public double Precipitation { get; set; }
        public double WindSpeed { get; set; }
        public string? Summary { get; set; }
        public DateTime Inserted { get; set; }
    }
}
