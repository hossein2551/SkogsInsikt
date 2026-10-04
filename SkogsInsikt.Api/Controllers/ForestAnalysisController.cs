using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkogsInsikt.Application.Services;
using SkogsInsikt.Domain.Entities;
using SkogsInsikt.Infrastructure.Data;

namespace SkogsInsikt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpPost("{forestAreaId:int}")]
    public async Task<ActionResult<ForestAnalysis>> Analyze(int forestAreaId)
    {
        var forestArea = await _context.ForestAreas.FindAsync(forestAreaId);

        if (forestArea is null)
            return NotFound();

        var analysis = await _analysisService.AnalyzeAsync(forestArea);

        _context.ForestAnalyses.Add(analysis);
        await _context.SaveChangesAsync();

        return Ok(analysis);
    }

    [HttpGet("area/{forestAreaId:int}")]
    public async Task<ActionResult<IEnumerable<ForestAnalysis>>> GetByForestArea(
        int forestAreaId)
    {
        var analyses = await _context.ForestAnalyses
            .Where(x => x.ForestAreaId == forestAreaId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(analyses);
    }
}
