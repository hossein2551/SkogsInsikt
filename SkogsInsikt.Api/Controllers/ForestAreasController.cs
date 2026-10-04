using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkogsInsikt.Application.Interfaces;
using SkogsInsikt.Domain.Entities;

namespace SkogsInsikt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ForestAreasController : ControllerBase
{
    private readonly IForestAreaService _forestAreaService;

    public ForestAreasController(
        IForestAreaService forestAreaService)
    {
        _forestAreaService = forestAreaService;
    }

    private string? GetUserId()
    {
        return User.FindFirstValue(
            ClaimTypes.NameIdentifier);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ForestArea>>> GetAll()
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        var forestAreas =
            await _forestAreaService.GetAllAsync(userId);

        return Ok(forestAreas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ForestArea>> GetById(int id)
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        var forestArea =
            await _forestAreaService.GetByIdAsync(
                id,
                userId);

        if (forestArea is null)
            return NotFound();

        return Ok(forestArea);
    }

    [HttpPost]
    public async Task<ActionResult<ForestArea>> Create(
        ForestArea forestArea)
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        var createdArea =
            await _forestAreaService.CreateAsync(
                forestArea,
                userId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdArea.Id },
            createdArea);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        ForestArea forestArea)
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        if (id != forestArea.Id)
            return BadRequest();

        var updated =
            await _forestAreaService.UpdateAsync(
                id,
                forestArea,
                userId);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        var deleted =
            await _forestAreaService.DeleteAsync(
                id,
                userId);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
