using SkogsInsikt.Domain.Entities;

namespace SkogsInsikt.Application.Interfaces;

public interface IForestAreaService
{
    Task<IEnumerable<ForestArea>> GetAllAsync();
    Task<ForestArea?> GetByIdAsync(int id);
    Task<ForestArea> CreateAsync(ForestArea forestArea);
    Task<bool> UpdateAsync(int id, ForestArea forestArea);
    Task<bool> DeleteAsync(int id);
}