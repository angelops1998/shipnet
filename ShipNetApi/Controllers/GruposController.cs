using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipNetApi.Data;
using ShipNetApi.Models;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GruposController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public GruposController(ApplicationDbContext context) => _context = context;

    private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    // GET: api/Grupos  (incluye materia, carrera y docente para mostrarlos en la tabla)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Grupo>>> GetGrupos()
    {
        var grupos = await _context.Grupos
            .Include(g => g.Materia).ThenInclude(m => m.Carrera)
            .Include(g => g.Docente)
            .OrderByDescending(g => g.Activo)
            .ThenBy(g => g.Modulo)
            .ToListAsync();
        return Ok(grupos);
    }

    // GET: api/Grupos/docente
    [HttpGet("docente")]
    [Authorize(Roles = "Docente")]
    public async Task<ActionResult<IEnumerable<Grupo>>> GetGruposDocente()
    {
        var grupos = await _context.Grupos
            .Include(g => g.Materia).ThenInclude(m => m.Carrera)
            .Include(g => g.Docente)
            .Where(g => g.Docente.ApplicationUserId == UserId)
            .OrderByDescending(g => g.Activo)
            .ThenBy(g => g.Modulo)
            .ToListAsync();

        return Ok(grupos);
    }

    // GET: api/Grupos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Grupo>> GetGrupo(int id)
    {
        var grupo = await _context.Grupos
            .Include(g => g.Materia).ThenInclude(m => m.Carrera)
            .Include(g => g.Docente)
            .FirstOrDefaultAsync(g => g.Id == id);
        if (grupo == null) return NotFound();
        return Ok(grupo);
    }

    // POST: api/Grupos
    [HttpPost]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin,Docente")]
    public async Task<IActionResult> PutGrupo(int id, Grupo grupo)
    {
        if (id != grupo.Id) return BadRequest("El id de la URL no coincide con el del cuerpo.");
        
        var existing = await _context.Grupos.Include(g => g.Docente).FirstOrDefaultAsync(g => g.Id == id);
        if (existing == null) return NotFound();

        // Si es docente, solo puede editar su propio grupo
        if (User.IsInRole("Docente") && existing.Docente?.ApplicationUserId != UserId)
        {
            return Forbid();
        }

        existing.Nombre = grupo.Nombre;
        existing.MateriaId = grupo.MateriaId;
        if (User.IsInRole("Admin"))
        {
            existing.DocenteId = grupo.DocenteId;
        }
        existing.Modulo = grupo.Modulo;
        existing.Turno = grupo.Turno;
        existing.Activo = grupo.Activo;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/Grupos/toggle-activo/5
    [HttpPost("toggle-activo/{id}")]
    [Authorize(Roles = "Admin,Docente")]
    public async Task<IActionResult> ToggleActivo(int id)
    {
        var grupo = await _context.Grupos.Include(g => g.Docente).FirstOrDefaultAsync(g => g.Id == id);
        if (grupo == null) return NotFound();

        if (User.IsInRole("Docente") && grupo.Docente?.ApplicationUserId != UserId)
        {
            return Forbid();
        }

        grupo.Activo = !grupo.Activo;
        await _context.SaveChangesAsync();

        return Ok(new { grupo.Id, grupo.Activo, mensaje = grupo.Activo ? "Materia activada para este módulo." : "Materia desactivada." });
    }

    // DELETE: api/Grupos/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGrupo(int id)
    {
        var grupo = await _context.Grupos.FindAsync(id);
        if (grupo == null) return NotFound();

        _context.Grupos.Remove(grupo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
