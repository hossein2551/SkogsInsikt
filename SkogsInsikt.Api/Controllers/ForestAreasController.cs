using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkogsInsikt.Application.Interfaces;
using SkogsInsikt.Domain.Entities;

namespace SkogsInsikt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ForestAreasController : ControllerBase
{
    private readonly IForestAreaService _forestAreaService;

    public ForestAreasController(
        IForestAreaService forestAreaService)
    {
        _forestAreaService = forestAreaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ForestArea>>> GetAll()
    {
        var forestAreas =
            await _forestAreaService.GetAllAsync();

        return Ok(forestAreas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ForestArea>> GetById(int id)
    {
        var forestArea =
            await _forestAreaService.GetByIdAsync(id);

        if (forestArea is null)
            return NotFound();

        return Ok(forestArea);
    }

    [HttpPost]
    public async Task<ActionResult<ForestArea>> Create(
        ForestArea forestArea)
    {
        var createdArea =
            await _forestAreaService.CreateAsync(forestArea);

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
        if (id != forestArea.Id)
            return BadRequest();

        var updated =
            await _forestAreaService.UpdateAsync(
                id,
                forestArea);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _forestAreaService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
