using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipNetApi.Data;
using ShipNetApi.Dtos;
using ShipNetApi.Models;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class DocentesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DocentesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Docente>>> GetDocentes()
    {
        return Ok(await _context.Docentes.OrderBy(d => d.Apellido).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Docente>> GetDocente(int id)
    {
        var d = await _context.Docentes.FindAsync(id);
        if (d == null) return NotFound();
        return Ok(d);
    }

    [HttpPost]
    public async Task<ActionResult<Docente>> PostDocente([FromBody] CrearDocenteDto dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
            return BadRequest("Ya existe un usuario con ese correo electrónico.");

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            NombreCompleto = $"{dto.Nombre} {dto.Apellido}",
            EmailConfirmed = true
        };

        var resultado = await _userManager.CreateAsync(user, dto.Password);
        if (!resultado.Succeeded)
            return BadRequest(resultado.Errors.First().Description);

        await _userManager.AddToRoleAsync(user, "Docente");

        var docente = new Docente
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            ApplicationUserId = user.Id,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Especialidad = dto.Especialidad
        };

        _context.Docentes.Add(docente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDocente), new { id = docente.Id }, docente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDocente(int id, Docente docente)
    {
        if (id != docente.Id) return BadRequest();
        var existing = await _context.Docentes.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Nombre = docente.Nombre;
        existing.Apellido = docente.Apellido;
        existing.Email = docente.Email;
        existing.Telefono = docente.Telefono;
        existing.Especialidad = docente.Especialidad;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocente(int id)
    {
        var docente = await _context.Docentes.FindAsync(id);
        if (docente == null) return NotFound();
        var user = await _userManager.FindByIdAsync(docente.ApplicationUserId);
        _context.Docentes.Remove(docente);
        await _context.SaveChangesAsync();
        if (user != null) await _userManager.DeleteAsync(user);
        return NoContent();
    }
}
