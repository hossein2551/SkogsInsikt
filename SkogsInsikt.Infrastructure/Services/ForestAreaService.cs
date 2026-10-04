using Microsoft.EntityFrameworkCore;
using SkogsInsikt.Application.Interfaces;
using SkogsInsikt.Domain.Entities;
using SkogsInsikt.Infrastructure.Data;

namespace SkogsInsikt.Infrastructure.Services;

public class ForestAreaService : IForestAreaService
{
    private readonly SkogsInsiktDbContext _context;

    public ForestAreaService(SkogsInsiktDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ForestArea>> GetAllAsync(
        string userId)
    {
        return await _context.ForestAreas
            .AsNoTracking()
            .Where(area => area.UserId == userId)
            .ToListAsync();
    }

    public async Task<ForestArea?> GetByIdAsync(
        int id,
        string userId)
    {
        return await _context.ForestAreas
            .AsNoTracking()
            .FirstOrDefaultAsync(
                area =>
                    area.Id == id &&
                    area.UserId == userId);
    }

    public async Task<ForestArea> CreateAsync(
        ForestArea forestArea,
        string userId)
    {
        forestArea.UserId = userId;

        _context.ForestAreas.Add(forestArea);
        await _context.SaveChangesAsync();

        return forestArea;
    }

    public async Task<bool> UpdateAsync(
        int id,
        ForestArea forestArea,
        string userId)
    {
        var existingArea =
            await _context.ForestAreas
                .FirstOrDefaultAsync(
                    area =>
                        area.Id == id &&
                        area.UserId == userId);

        if (existingArea is null)
            return false;

        existingArea.Name = forestArea.Name;
        existingArea.AreaHectares = forestArea.AreaHectares;
        existingArea.TreeSpecies = forestArea.TreeSpecies;
        existingArea.PlantingYear = forestArea.PlantingYear;
        existingArea.Latitude = forestArea.Latitude;
        existingArea.Longitude = forestArea.Longitude;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        string userId)
    {
        var forestArea =
            await _context.ForestAreas
                .FirstOrDefaultAsync(
                    area =>
                        area.Id == id &&
                        area.UserId == userId);

        if (forestArea is null)
            return false;

        _context.ForestAreas.Remove(forestArea);
        await _context.SaveChangesAsync();

        return true;
    }
}
