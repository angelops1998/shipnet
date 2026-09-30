using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipNetApi.Data;
using ShipNetApi.Models;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MateriasController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public MateriasController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Materia>>> GetMaterias()
    {
        var materias = await _context.Materias
            .Include(m => m.Carrera)
            .OrderBy(m => m.CarreraId)
            .ThenBy(m => m.Semestre)
            .ThenBy(m => m.Nombre)
            .ToListAsync();
        return Ok(materias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Materia>> GetMateria(int id)
    {
        var materia = await _context.Materias.Include(m => m.Carrera).FirstOrDefaultAsync(m => m.Id == id);
        if (materia == null) return NotFound();
        return Ok(materia);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Materia>> PostMateria(Materia materia)
    {
        _context.Materias.Add(materia);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMateria), new { id = materia.Id }, materia);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutMateria(int id, Materia materia)
    {
        if (id != materia.Id) return BadRequest();
        var existing = await _context.Materias.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Nombre = materia.Nombre;
        existing.Codigo = materia.Codigo;
        existing.CarreraId = materia.CarreraId;
        existing.Semestre = materia.Semestre;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMateria(int id)
    {
        var materia = await _context.Materias.FindAsync(id);
        if (materia == null) return NotFound();
        _context.Materias.Remove(materia);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
