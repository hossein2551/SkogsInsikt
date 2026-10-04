namespace SkogsInsikt.Domain.Entities;

public class ForestAnalysis
{
    public int Id { get; set; }

    public int ForestAreaId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string RiskLevel { get; set; } = string.Empty;

    public string Recommendation { get; set; } = string.Empty;

    public double Temperature { get; set; }

    public double Precipitation { get; set; }

    public double WindSpeed { get; set; }
}