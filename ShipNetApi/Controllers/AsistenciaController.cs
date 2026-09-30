using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipNetApi.Data;
using ShipNetApi.Dtos;
using ShipNetApi.Models;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AsistenciaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AsistenciaController(ApplicationDbContext context)
    {
        _context = context;
    }

    private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    // ---------- DOCENTE (SN-09) ----------

    // GET: api/Asistencia/docente/sesiones
    [HttpGet("docente/sesiones")]
    [Authorize(Roles = "Docente")]
    public async Task<ActionResult<IEnumerable<SesionAsistencia>>> GetSesionesDocente()
    {
        var sesiones = await _context.SesionesAsistencia
            .Include(s => s.Grupo).ThenInclude(g => g.Materia)
            .Include(s => s.Grupo).ThenInclude(g => g.Docente)
            .Where(s => s.Grupo.Docente.ApplicationUserId == UserId)
            .OrderByDescending(s => s.Id)
            .ToListAsync();

        return Ok(sesiones);
    }

    // GET: api/Asistencia/docente/grupos
    [HttpGet("docente/grupos")]
    [Authorize(Roles = "Docente")]
    public async Task<ActionResult<IEnumerable<Grupo>>> GetGruposDocente()
    {
        var grupos = await _context.Grupos
            .Include(g => g.Materia)
            .Where(g => g.Docente.ApplicationUserId == UserId)
            .ToListAsync();

        return Ok(grupos);
    }

    // POST: api/Asistencia/abrir
    [HttpPost("abrir")]
    [Authorize(Roles = "Docente")]
    public async Task<ActionResult<SesionAsistencia>> Abrir([FromBody] AbrirSesionDto dto)
    {
        var grupo = await _context.Grupos
            .Include(g => g.Docente)
            .FirstOrDefaultAsync(g => g.Id == dto.GrupoId && g.Docente.ApplicationUserId == UserId);

        if (grupo == null)
            return BadRequest("El grupo no existe o no pertenece al docente autenticado.");

        var sesion = new SesionAsistencia
        {
            GrupoId = dto.GrupoId,
            Fecha = DateOnly.FromDateTime(DateTime.Now),
            HoraInicio = DateTime.Now.TimeOfDay,
            Abierta = true
        };

        _context.SesionesAsistencia.Add(sesion);
        await _context.SaveChangesAsync();

        return Ok(sesion);
    }

    // POST: api/Asistencia/cerrar/5
    [HttpPost("cerrar/{id}")]
    [Authorize(Roles = "Docente")]
    public async Task<IActionResult> Cerrar(int id)
    {
        var sesion = await _context.SesionesAsistencia
            .Include(s => s.Grupo).ThenInclude(g => g.Docente)
            .FirstOrDefaultAsync(s => s.Id == id && s.Grupo.Docente.ApplicationUserId == UserId);

        if (sesion == null)
            return NotFound("La sesión no existe o no pertenece al docente autenticado.");

        sesion.Abierta = false;
        sesion.HoraFin = DateTime.Now.TimeOfDay;
        await _context.SaveChangesAsync();

        return Ok();
    }

    // GET: api/Asistencia/{id}
    [HttpGet("{id}")]
    [Authorize(Roles = "Docente,Admin")]
    public async Task<ActionResult<SesionAsistencia>> Detalle(int id)
    {
        var query = _context.SesionesAsistencia
            .Include(s => s.Grupo).ThenInclude(g => g.Materia)
            .Include(s => s.Grupo).ThenInclude(g => g.Docente)
            .Include(s => s.RegistrosAsistencia).ThenInclude(r => r.Estudiante)
            .Include(s => s.RegistrosAsistencia).ThenInclude(r => r.Equipo)
            .AsQueryable();

        if (User.IsInRole("Docente"))
        {
            query = query.Where(s => s.Grupo.Docente.ApplicationUserId == UserId);
        }

        var sesion = await query.FirstOrDefaultAsync(s => s.Id == id);

        if (sesion == null)
            return NotFound("La sesión no existe o no tiene permisos para verla.");

        return Ok(sesion);
    }

    // GET: api/Asistencia/todas (ADMIN)
    [HttpGet("todas")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<SesionAsistencia>>> GetTodasSesiones()
    {
        var sesiones = await _context.SesionesAsistencia
            .Include(s => s.Grupo).ThenInclude(g => g.Materia)
            .Include(s => s.Grupo).ThenInclude(g => g.Docente)
            .Include(s => s.RegistrosAsistencia)
            .OrderByDescending(s => s.Fecha)
            .ThenByDescending(s => s.Id)
            .ToListAsync();

        return Ok(sesiones);
    }

    // ---------- ESTUDIANTE (SN-11) ----------

    // GET: api/Asistencia/abiertas
    [HttpGet("abiertas")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<SesionAsistencia>>> GetSesionesAbiertas()
    {
        var sesiones = await _context.SesionesAsistencia
            .Include(s => s.Grupo).ThenInclude(g => g.Materia)
            .Include(s => s.Grupo).ThenInclude(g => g.Docente)
            .Where(s => s.Abierta)
            .OrderByDescending(s => s.Id)
            .ToListAsync();

        return Ok(sesiones);
    }

    // POST: api/Asistencia/marcar
    [HttpPost("marcar")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Marcar([FromBody] MarcarAsistenciaDto dto)
    {
        var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.ApplicationUserId == UserId);
        if (estudiante == null)
            return BadRequest("El usuario autenticado no tiene un perfil de estudiante asociado.");

        var sesion = await _context.SesionesAsistencia.FirstOrDefaultAsync(s => s.Id == dto.SesionId && s.Abierta);
        if (sesion == null)
            return BadRequest("La sesión no existe o ya ha sido cerrada por el docente.");

        var yaMarco = await _context.RegistrosAsistencia
            .AnyAsync(r => r.SesionAsistenciaId == dto.SesionId && r.EstudianteId == estudiante.Id);

        if (yaMarco)
            return BadRequest("Ya has registrado tu asistencia para esta sesión.");

        var mac = string.IsNullOrWhiteSpace(dto.MacAddress) ? "00:00:00:00:00:00" : dto.MacAddress.Replace('-', ':').ToUpper();

        // Si el estudiante aún no tiene MAC en la BD (primer uso / demo), auto-vincular este dispositivo
        if (string.IsNullOrEmpty(estudiante.MacAddress) && mac != "00:00:00:00:00:00")
        {
            var otroEstudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.Id != estudiante.Id && e.MacAddress == mac);
            if (otroEstudiante != null)
            {
                return BadRequest($"Este dispositivo (MAC: {mac}) ya está registrado a nombre del estudiante {otroEstudiante.Nombre} {otroEstudiante.Apellido}. No puedes usar este dispositivo con otra cuenta.");
            }

            estudiante.MacAddress = mac;
            await _context.SaveChangesAsync();
        }

        // Si ya tiene MAC registrada, verificar que esté marcando desde su dispositivo
        if (!string.IsNullOrEmpty(estudiante.MacAddress) && estudiante.MacAddress != mac)
        {
            return BadRequest($"Tu cuenta solo está autorizada para marcar desde tu dispositivo registrado (MAC: {estudiante.MacAddress}). Tu dispositivo actual detectado es '{mac}'.");
        }

        var equipo = await _context.Equipos.FirstOrDefaultAsync(e => e.MacAddress == mac);

        // Si es la MAC autorizada del estudiante y aún no estaba en la tabla Equipos, registrarla automáticamente
        if (equipo == null && !string.IsNullOrEmpty(estudiante.MacAddress) && estudiante.MacAddress == mac)
        {
            equipo = new Equipo
            {
                Codigo = $"MOVIL-{estudiante.Id}",
                MacAddress = mac,
                Ubicacion = $"Móvil - {estudiante.Nombre} {estudiante.Apellido}"
            };
            _context.Equipos.Add(equipo);
            await _context.SaveChangesAsync();
        }

        if (equipo == null)
            return BadRequest($"El equipo o dispositivo con dirección MAC '{mac}' no está registrado ni autorizado en el laboratorio.");

        var registro = new RegistroAsistencia
        {
            SesionAsistenciaId = dto.SesionId,
            EstudianteId = estudiante.Id,
            EquipoId = equipo.Id,
            HoraRegistro = DateTime.Now.TimeOfDay,
            Estado = "Presente"
        };

        _context.RegistrosAsistencia.Add(registro);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // GET: api/Asistencia/estudiante/historial
    [HttpGet("estudiante/historial")]
    [Authorize(Roles = "Estudiante")]
    public async Task<ActionResult<IEnumerable<RegistroAsistencia>>> GetHistorialEstudiante()
    {
        var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.ApplicationUserId == UserId);
        if (estudiante == null) return NotFound("Estudiante no encontrado.");

        var registros = await _context.RegistrosAsistencia
            .Include(r => r.SesionAsistencia).ThenInclude(s => s.Grupo).ThenInclude(g => g.Materia)
            .Include(r => r.Equipo)
            .Where(r => r.EstudianteId == estudiante.Id)
            .OrderByDescending(r => r.SesionAsistencia.Fecha)
            .ThenByDescending(r => r.HoraRegistro)
            .ToListAsync();

        return Ok(registros);
    }
}

