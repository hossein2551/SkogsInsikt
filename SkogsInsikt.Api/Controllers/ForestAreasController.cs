using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkogsInsikt.Domain.Entities;
using SkogsInsikt.Infrastructure.Data;

namespace SkogsInsikt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ForestAreasController : ControllerBase
{
    private readonly SkogsInsiktDbContext _context;

    public ForestAreasController(SkogsInsiktDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ForestArea>>> GetAll()
    {
        return Ok(await _context.ForestAreas.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ForestArea>> GetById(int id)
    {
        var forestArea = await _context.ForestAreas.FindAsync(id);

        if (forestArea is null)
            return NotFound();

        return Ok(forestArea);
    }

    [HttpPost]
    public async Task<ActionResult<ForestArea>> Create(ForestArea forestArea)
    {
        _context.ForestAreas.Add(forestArea);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = forestArea.Id },
            forestArea);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ForestArea forestArea)
    {
        if (id != forestArea.Id)
            return BadRequest();

        var exists = await _context.ForestAreas.AnyAsync(x => x.Id == id);

        if (!exists)
            return NotFound();

        _context.Entry(forestArea).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var forestArea = await _context.ForestAreas.FindAsync(id);

        if (forestArea is null)
            return NotFound();

        _context.ForestAreas.Remove(forestArea);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
