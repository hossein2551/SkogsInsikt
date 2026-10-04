using System.Text.Json.Serialization;
using System.Net.Http.Json;
using SkogsInsikt.Application.Interfaces;

namespace SkogsInsikt.Infrastructure.Services;

public class OpenMeteoWeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;

    public OpenMeteoWeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherData> GetCurrentWeatherAsync(
        double latitude,
        double longitude)
    {
        var url =
            $"https://api.open-meteo.com/v1/forecast" +
            $"?latitude={latitude}" +
            $"&longitude={longitude}" +
            $"&current=temperature_2m,precipitation,wind_speed_10m";

        var response =
            await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);

        if (response?.Current is null)
        {
            throw new InvalidOperationException(
                "Kunde inte hämta väderdata från Open-Meteo.");
        }

        return new WeatherData
        {
            Temperature = response.Current.Temperature,
            Precipitation = response.Current.Precipitation,
            WindSpeed = response.Current.WindSpeed
        };
    }

    private class OpenMeteoResponse
    {
        public CurrentWeather? Current { get; set; }
    }
private class CurrentWeather
{
    [JsonPropertyName("temperature_2m")]
    public double Temperature { get; set; }

    [JsonPropertyName("precipitation")]
    public double Precipitation { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double WindSpeed { get; set; }
}
}