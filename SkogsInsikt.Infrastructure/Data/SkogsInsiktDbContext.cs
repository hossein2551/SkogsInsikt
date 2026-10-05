using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkogsInsikt.Domain.Entities;
using SkogsInsikt.Infrastructure.Identity;

namespace SkogsInsikt.Infrastructure.Data;

public class SkogsInsiktDbContext
    : IdentityDbContext<ApplicationUser>
{
    public SkogsInsiktDbContext(
        DbContextOptions<SkogsInsiktDbContext> options)
        : base(options)
    {
    }

    public DbSet<ForestArea> ForestAreas => Set<ForestArea>();
    public DbSet<ForestAnalysis> ForestAnalyses => Set<ForestAnalysis>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("insikt");

        modelBuilder.Entity<ForestArea>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(area => area.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ForestAnalysis>()
            .HasOne<ForestArea>()
            .WithMany()
            .HasForeignKey(analysis => analysis.ForestAreaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


