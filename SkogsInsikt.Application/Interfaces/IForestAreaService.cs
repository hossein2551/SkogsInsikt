using SkogsInsikt.Domain.Entities;

namespace SkogsInsikt.Application.Interfaces;

public interface IForestAreaService
{
    Task<IEnumerable<ForestArea>> GetAllAsync(string userId);

    Task<ForestArea?> GetByIdAsync(
        int id,
        string userId);

    Task<ForestArea> CreateAsync(
        ForestArea forestArea,
        string userId);

    Task<bool> UpdateAsync(
        int id,
        ForestArea forestArea,
        string userId);

    Task<bool> DeleteAsync(
        int id,
        string userId);
}
