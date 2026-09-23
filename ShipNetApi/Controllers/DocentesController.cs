using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipNetApi.Data;
using ShipNetApi.Models;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class DocentesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public DocentesController(ApplicationDbContext context) => _context = context;

    // GET: api/Docentes  (se usa para llenar el combo al crear un grupo)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Docente>>> GetDocentes()
    {
        var docentes = await _context.Docentes.ToListAsync();
        return Ok(docentes);
    }
}
