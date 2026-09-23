using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipNetApi.Data;
using ShipNetApi.Models;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class MateriasController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public MateriasController(ApplicationDbContext context) => _context = context;

    // GET: api/Materias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Materia>>> GetMaterias()
    {
        var materias = await _context.Materias.ToListAsync();
        return Ok(materias);
    }

    // GET: api/Materias/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Materia>> GetMateria(int id)
    {
        var materia = await _context.Materias.FindAsync(id);
        if (materia == null) return NotFound();
        return Ok(materia);
    }

    // POST: api/Materias
    [HttpPost]
    public async Task<ActionResult<Materia>> PostMateria(Materia materia)
    {
        _context.Materias.Add(materia);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMateria), new { id = materia.Id }, materia);
    }

    // PUT: api/Materias/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMateria(int id, Materia materia)
    {
        if (id != materia.Id) return BadRequest("El id de la URL no coincide con el del cuerpo.");
        if (!await _context.Materias.AnyAsync(m => m.Id == id)) return NotFound();

        _context.Entry(materia).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Materias/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMateria(int id)
    {
        var materia = await _context.Materias.FindAsync(id);
        if (materia == null) return NotFound();

        _context.Materias.Remove(materia);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
