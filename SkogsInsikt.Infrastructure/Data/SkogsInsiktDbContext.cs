using Microsoft.EntityFrameworkCore;
using SkogsInsikt.Domain.Entities;

namespace SkogsInsikt.Infrastructure.Data;

public class SkogsInsiktDbContext : DbContext
{
    public SkogsInsiktDbContext(DbContextOptions<SkogsInsiktDbContext> options)
        : base(options)
    {
    }

    public DbSet<ForestArea> ForestAreas => Set<ForestArea>();

    public DbSet<ForestAnalysis> ForestAnalyses => Set<ForestAnalysis>();
}
