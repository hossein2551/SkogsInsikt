using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkogsInsikt.Application.Services;
using SkogsInsikt.Domain.Entities;
using SkogsInsikt.Infrastructure.Data;

namespace SkogsInsikt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ForestAnalysisController : ControllerBase
{
    private readonly ForestAnalysisService _analysisService;
    private readonly SkogsInsiktDbContext _context;

    public ForestAnalysisController(
        ForestAnalysisService analysisService,
        SkogsInsiktDbContext context)
    {
        _analysisService = analysisService;
        _context = context;
    }

    private string? GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    [HttpPost("{forestAreaId:int}")]
    public async Task<ActionResult<ForestAnalysis>> Analyze(
        int forestAreaId)
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        var forestArea = await _context.ForestAreas
            .FirstOrDefaultAsync(
                area =>
                    area.Id == forestAreaId &&
                    area.UserId == userId);

        if (forestArea is null)
            return NotFound();

        var analysis =
            await _analysisService.AnalyzeAsync(forestArea);

        _context.ForestAnalyses.Add(analysis);
        await _context.SaveChangesAsync();

        return Ok(analysis);
    }

    [HttpGet("area/{forestAreaId:int}")]
    public async Task<ActionResult<IEnumerable<ForestAnalysis>>> GetByForestArea(
        int forestAreaId)
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        var ownsArea = await _context.ForestAreas
            .AnyAsync(
                area =>
                    area.Id == forestAreaId &&
                    area.UserId == userId);

        if (!ownsArea)
            return NotFound();

        var analyses = await _context.ForestAnalyses
            .AsNoTracking()
            .Where(analysis =>
                analysis.ForestAreaId == forestAreaId)
            .OrderByDescending(analysis => analysis.CreatedAt)
            .ToListAsync();

        return Ok(analyses);
    }
}
