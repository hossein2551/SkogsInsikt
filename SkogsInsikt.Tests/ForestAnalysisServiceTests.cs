using SkogsInsikt.Application.Interfaces;
using SkogsInsikt.Application.Services;
using SkogsInsikt.Domain.Entities;

namespace SkogsInsikt.Tests;

public class ForestAnalysisServiceTests
{
    [Fact]
    public async Task AnalyzeAsync_HighWind_ReturnsHighRisk()
    {
        var weatherService = new FakeWeatherService
        {
            Weather = new WeatherData
            {
                Temperature = 15,
                Precipitation = 0,
                WindSpeed = 20
            }
        };

        var service = new ForestAnalysisService(weatherService);

        var forestArea = new ForestArea
        {
            Id = 1,
            Name = "Testområde",
            AreaHectares = 25,
            TreeSpecies = "Gran",
            PlantingYear = 2015,
            Latitude = 56.6634,
            Longitude = 16.3568
        };

        var result = await service.AnalyzeAsync(forestArea);

        Assert.Equal("High", result.RiskLevel);
        Assert.Equal(20, result.WindSpeed);
    }

    [Fact]
    public async Task AnalyzeAsync_HighPrecipitation_ReturnsMediumRisk()
    {
        var weatherService = new FakeWeatherService
        {
            Weather = new WeatherData
            {
                Temperature = 12,
                Precipitation = 8,
                WindSpeed = 5
            }
        };

        var service = new ForestAnalysisService(weatherService);

        var forestArea = new ForestArea
        {
            Id = 2,
            Name = "Regnigt testområde",
            AreaHectares = 30,
            TreeSpecies = "Tall",
            PlantingYear = 2010,
            Latitude = 56.6634,
            Longitude = 16.3568
        };

        var result = await service.AnalyzeAsync(forestArea);

        Assert.Equal("Medium", result.RiskLevel);
        Assert.Equal(8, result.Precipitation);
    }

    [Fact]
    public async Task AnalyzeAsync_NormalWeather_ReturnsLowRisk()
    {
        var weatherService = new FakeWeatherService
        {
            Weather = new WeatherData
            {
                Temperature = 18,
                Precipitation = 1,
                WindSpeed = 6
            }
        };

        var service = new ForestAnalysisService(weatherService);

        var forestArea = new ForestArea
        {
            Id = 3,
            Name = "Lugnt testområde",
            AreaHectares = 20,
            TreeSpecies = "Björk",
            PlantingYear = 2018,
            Latitude = 56.6634,
            Longitude = 16.3568
        };

        var result = await service.AnalyzeAsync(forestArea);

        Assert.Equal("Low", result.RiskLevel);
        Assert.Equal(18, result.Temperature);
        Assert.Equal(1, result.Precipitation);
        Assert.Equal(6, result.WindSpeed);
    }

    private class FakeWeatherService : IWeatherService
    {
        public WeatherData Weather { get; set; } = new();

        public Task<WeatherData> GetCurrentWeatherAsync(
            double latitude,
            double longitude)
        {
            return Task.FromResult(Weather);
        }
    }
}