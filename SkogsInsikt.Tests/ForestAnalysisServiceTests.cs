using SkogsInsikt.Application.Interfaces;
using SkogsInsikt.Application.Services;
using SkogsInsikt.Domain.Entities;

namespace SkogsInsikt.Tests;

public class ForestAnalysisServiceTests
{
    [Fact]
    public async Task AnalyzeAsync_HighWind_ReturnsHighRisk()
    {
        var service = CreateService(
            temperature: 15,
            precipitation: 0,
            windSpeed: 20);

        var result = await service.AnalyzeAsync(
            CreateForestArea(2015));

        Assert.Equal("High", result.RiskLevel);
        Assert.Contains("vindrisk", result.Recommendation);
        Assert.Equal(20, result.WindSpeed);
    }

    [Fact]
    public async Task AnalyzeAsync_HighPrecipitation_ReturnsMediumRisk()
    {
        var service = CreateService(
            temperature: 12,
            precipitation: 8,
            windSpeed: 5);

        var result = await service.AnalyzeAsync(
            CreateForestArea(2010));

        Assert.Equal("Medium", result.RiskLevel);
        Assert.Contains("nederbörd", result.Recommendation);
        Assert.Equal(8, result.Precipitation);
    }

    [Fact]
    public async Task AnalyzeAsync_HotAndDryWeather_ReturnsMediumRisk()
    {
        var service = CreateService(
            temperature: 30,
            precipitation: 0,
            windSpeed: 5);

        var result = await service.AnalyzeAsync(
            CreateForestArea(2018));

        Assert.Equal("Medium", result.RiskLevel);
        Assert.Contains("torrt", result.Recommendation);
    }

    [Fact]
    public async Task AnalyzeAsync_OldForestAndElevatedWind_ReturnsMediumRisk()
    {
        var service = CreateService(
            temperature: 15,
            precipitation: 0,
            windSpeed: 12);

        var result = await service.AnalyzeAsync(
            CreateForestArea(DateTime.UtcNow.Year - 60));

        Assert.Equal("Medium", result.RiskLevel);
        Assert.Contains("Äldre skog", result.Recommendation);
    }

    [Fact]
    public async Task AnalyzeAsync_NormalConditions_ReturnsLowRisk()
    {
        var service = CreateService(
            temperature: 18,
            precipitation: 1,
            windSpeed: 6);

        var result = await service.AnalyzeAsync(
            CreateForestArea(2018));

        Assert.Equal("Low", result.RiskLevel);
        Assert.Equal(18, result.Temperature);
        Assert.Equal(1, result.Precipitation);
        Assert.Equal(6, result.WindSpeed);
    }

    [Fact]
    public async Task AnalyzeAsync_HighWindTakesPriorityOverOtherRisks()
    {
        var service = CreateService(
            temperature: 30,
            precipitation: 8,
            windSpeed: 20);

        var result = await service.AnalyzeAsync(
            CreateForestArea(DateTime.UtcNow.Year - 60));

        Assert.Equal("High", result.RiskLevel);
        Assert.Contains("vindrisk", result.Recommendation);
    }

    private static ForestAnalysisService CreateService(
        double temperature,
        double precipitation,
        double windSpeed)
    {
        var weatherService = new FakeWeatherService
        {
            Weather = new WeatherData
            {
                Temperature = temperature,
                Precipitation = precipitation,
                WindSpeed = windSpeed
            }
        };

        return new ForestAnalysisService(weatherService);
    }

    private static ForestArea CreateForestArea(int plantingYear)
    {
        return new ForestArea
        {
            Id = 1,
            Name = "Testområde",
            AreaHectares = 25,
            TreeSpecies = "Gran",
            PlantingYear = plantingYear,
            Latitude = 56.6634,
            Longitude = 16.3568
        };
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