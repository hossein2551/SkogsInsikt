using SkogsInsikt.Application.Interfaces;
using SkogsInsikt.Domain.Entities;

namespace SkogsInsikt.Application.Services;

public class ForestAnalysisService
{
    private readonly IWeatherService _weatherService;

    public ForestAnalysisService(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<ForestAnalysis> AnalyzeAsync(ForestArea forestArea)
    {
        var weather = await _weatherService.GetCurrentWeatherAsync(
            forestArea.Latitude,
            forestArea.Longitude);

        var riskLevel = "Low";
        var recommendation =
            "Inga särskilda väderrelaterade åtgärder rekommenderas just nu.";

        if (weather.WindSpeed >= 15)
        {
            riskLevel = "High";
            recommendation =
                "Hög vindrisk. Kontrollera området för riskträd och undvik skogsarbete vid kraftig vind.";
        }
        else if (weather.Precipitation >= 5)
        {
            riskLevel = "Medium";
            recommendation =
                "Förhöjd nederbörd. Var uppmärksam på markförhållanden och planera körning för att minska markskador.";
        }

        return new ForestAnalysis
        {
            ForestAreaId = forestArea.Id,
            RiskLevel = riskLevel,
            Recommendation = recommendation,
            Temperature = weather.Temperature,
            Precipitation = weather.Precipitation,
            WindSpeed = weather.WindSpeed
        };
    }
}