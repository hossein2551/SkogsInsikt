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

        var forestAge = DateTime.UtcNow.Year - forestArea.PlantingYear;

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
        else if (weather.Temperature >= 28 &&
                 weather.Precipitation < 1)
        {
            riskLevel = "Medium";
            recommendation =
                "Varmt och torrt väder. Var extra uppmärksam på torra förhållanden och undvik aktiviteter som kan öka brandrisken.";
        }
        else if (forestAge >= 50 &&
                 weather.WindSpeed >= 10)
        {
            riskLevel = "Medium";
            recommendation =
                "Äldre skog i kombination med förhöjd vind. Kontrollera området för instabila eller skadade träd.";
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