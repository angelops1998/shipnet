using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipNetApi.Data;
using ShipNetApi.Models;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class EquiposController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public EquiposController(ApplicationDbContext context) => _context = context;

    // GET: api/Equipos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipos()
    {
        var equipos = await _context.Equipos.ToListAsync();
        return Ok(equipos);
    }

    // GET: api/Equipos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Equipo>> GetEquipo(int id)
    {
        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null) return NotFound();
        return Ok(equipo);
    }

    // POST: api/Equipos
    [HttpPost]
    public async Task<ActionResult<Equipo>> PostEquipo(Equipo equipo)
    {
        equipo.MacAddress = NormalizarMac(equipo.MacAddress);
        _context.Equipos.Add(equipo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEquipo), new { id = equipo.Id }, equipo);
    }

    // PUT: api/Equipos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEquipo(int id, Equipo equipo)
    {
        if (id != equipo.Id) return BadRequest("El id de la URL no coincide con el del cuerpo.");
        if (!await _context.Equipos.AnyAsync(e => e.Id == id)) return NotFound();

        equipo.MacAddress = NormalizarMac(equipo.MacAddress);
        _context.Entry(equipo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Equipos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipo(int id)
    {
        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null) return NotFound();

        _context.Equipos.Remove(equipo);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // AA-BB-CC-DD-EE-FF o aa:bb:... -> AA:BB:CC:DD:EE:FF
    private static string NormalizarMac(string mac) => mac.Replace('-', ':').ToUpper();
}
