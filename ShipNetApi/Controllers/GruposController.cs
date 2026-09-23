using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipNetApi.Data;
using ShipNetApi.Models;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class GruposController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public GruposController(ApplicationDbContext context) => _context = context;

    // GET: api/Grupos  (incluye materia y docente para mostrarlos en la tabla)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Grupo>>> GetGrupos()
    {
        var grupos = await _context.Grupos.Include(g => g.Materia).Include(g => g.Docente).ToListAsync();
        return Ok(grupos);
    }

    // GET: api/Grupos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Grupo>> GetGrupo(int id)
    {
        var grupo = await _context.Grupos.Include(g => g.Materia).Include(g => g.Docente)
            .FirstOrDefaultAsync(g => g.Id == id);
        if (grupo == null) return NotFound();
        return Ok(grupo);
    }

    // POST: api/Grupos
    [HttpPost]
    public async Task<ActionResult<Grupo>> PostGrupo(Grupo grupo)
    {
        if (!await _context.Materias.AnyAsync(m => m.Id == grupo.MateriaId)) return BadRequest("La materia no existe.");
        if (!await _context.Docentes.AnyAsync(d => d.Id == grupo.DocenteId)) return BadRequest("El docente no existe.");

        _context.Grupos.Add(grupo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGrupo), new { id = grupo.Id }, grupo);
    }

    // PUT: api/Grupos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGrupo(int id, Grupo grupo)
    {
        if (id != grupo.Id) return BadRequest("El id de la URL no coincide con el del cuerpo.");
        if (!await _context.Grupos.AnyAsync(g => g.Id == id)) return NotFound();

        _context.Entry(grupo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Grupos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGrupo(int id)
    {
        var grupo = await _context.Grupos.FindAsync(id);
        if (grupo == null) return NotFound();

        _context.Grupos.Remove(grupo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
