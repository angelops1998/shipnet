using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipNetApi.Data;
using ShipNetApi.Models;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class CarrerasController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public CarrerasController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Carrera>>> GetCarreras()
    {
        return Ok(await _context.Carreras.OrderBy(c => c.Facultad).ThenBy(c => c.Nombre).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Carrera>> GetCarrera(int id)
    {
        var c = await _context.Carreras.FindAsync(id);
        if (c == null) return NotFound();
        return Ok(c);
    }

    [HttpPost]
    public async Task<ActionResult<Carrera>> PostCarrera(Carrera carrera)
    {
        if (await _context.Carreras.AnyAsync(c => c.Codigo == carrera.Codigo))
            return BadRequest("El código de carrera ya existe.");
        _context.Carreras.Add(carrera);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCarrera), new { id = carrera.Id }, carrera);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCarrera(int id, Carrera carrera)
    {
        if (id != carrera.Id) return BadRequest();
        var existing = await _context.Carreras.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Nombre = carrera.Nombre;
        existing.Codigo = carrera.Codigo;
        existing.Facultad = carrera.Facultad;
        existing.Duracion = carrera.Duracion;
        existing.Activa = carrera.Activa;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCarrera(int id)
    {
        var c = await _context.Carreras.FindAsync(id);
        if (c == null) return NotFound();
        _context.Carreras.Remove(c);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
