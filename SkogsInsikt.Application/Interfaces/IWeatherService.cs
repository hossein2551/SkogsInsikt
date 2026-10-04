namespace SkogsInsikt.Application.Interfaces;

public interface IWeatherService
{
    Task<WeatherData> GetCurrentWeatherAsync(
        double latitude,
        double longitude);
}

public class WeatherData
{
    public double Temperature { get; set; }

    public double Precipitation { get; set; }

    public double WindSpeed { get; set; }
}