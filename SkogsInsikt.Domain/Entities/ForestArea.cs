namespace SkogsInsikt.Domain.Entities;

public class ForestArea
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public double AreaHectares { get; set; }

    public string TreeSpecies { get; set; } = string.Empty;

    public int PlantingYear { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}