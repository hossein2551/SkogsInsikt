using System.ComponentModel.DataAnnotations;

namespace SkogsInsikt.Domain.Entities;

public class ForestArea
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Namn måste anges.")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(0.1, 100000, ErrorMessage = "Areal måste vara större än 0 hektar.")]
    public double AreaHectares { get; set; }

    [Required(ErrorMessage = "Trädslag måste anges.")]
    [StringLength(50)]
    public string TreeSpecies { get; set; } = string.Empty;

    [Range(1800, 2100, ErrorMessage = "Planteringsår måste vara mellan 1800 och 2100.")]
    public int PlantingYear { get; set; }

    [Range(-90, 90, ErrorMessage = "Latitud måste vara mellan -90 och 90.")]
    public double Latitude { get; set; }

    [Range(-180, 180, ErrorMessage = "Longitud måste vara mellan -180 och 180.")]
    public double Longitude { get; set; }
}
