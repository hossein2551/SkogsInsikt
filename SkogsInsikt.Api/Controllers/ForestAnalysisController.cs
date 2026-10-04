using Microsoft.AspNetCore.Mvc;
using SkogsInsikt.Application.Services;
using SkogsInsikt.Domain.Entities;

namespace SkogsInsikt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ForestAnalysisController : ControllerBase
{
    private readonly ForestAnalysisService _analysisService;

    public ForestAnalysisController(ForestAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpPost]
    public async Task<ActionResult<ForestAnalysis>> Analyze(
        [FromBody] ForestArea forestArea)
    {
        var analysis = await _analysisService.AnalyzeAsync(forestArea);

        return Ok(analysis);
    }
}