using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BonusSystem.Api.Data;

namespace BonusSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasuresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MeasuresController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMeasures()
    {
        var measures = await _context.Measures
            .Select(m => new { m.Id, m.Name, m.Description })
            .ToListAsync();

        return Ok(measures);
    }
}