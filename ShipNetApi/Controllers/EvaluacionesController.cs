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
public class EvaluacionesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EvaluacionesController(ApplicationDbContext context)
    {
        _context = context;
    }

    private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    // ---------- DOCENTE ----------

    // GET: api/Evaluaciones/docente
    [HttpGet("docente")]
    [Authorize(Roles = "Docente")]
    public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionesDocente()
    {
        var evaluaciones = await _context.Evaluaciones
            .Include(e => e.Grupo).ThenInclude(g => g.Materia)
            .Include(e => e.IntentosEvaluacion)
            .Where(e => e.Grupo.Docente.ApplicationUserId == UserId)
            .OrderByDescending(e => e.Id)
            .ToListAsync();

        return Ok(evaluaciones);
    }

    // POST: api/Evaluaciones
    [HttpPost]
    [Authorize(Roles = "Docente")]
    public async Task<ActionResult<Evaluacion>> Crear([FromBody] CrearEvaluacionDto dto)
    {
        var grupo = await _context.Grupos
            .Include(g => g.Docente)
            .FirstOrDefaultAsync(g => g.Id == dto.GrupoId && g.Docente.ApplicationUserId == UserId);

        if (grupo == null)
            return BadRequest("El grupo no existe o no pertenece al docente autenticado.");

        if (dto.FechaFin <= dto.FechaInicio)
            return BadRequest("La fecha de fin debe ser posterior a la fecha de inicio.");

        var evaluacion = new Evaluacion
        {
            GrupoId = dto.GrupoId,
            Titulo = dto.Titulo,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            DuracionMinutos = dto.DuracionMinutos <= 0 ? 60 : dto.DuracionMinutos,
            PreguntasJson = dto.PreguntasJson
        };


        _context.Evaluaciones.Add(evaluacion);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Detalle), new { id = evaluacion.Id }, evaluacion);
    }

    // GET: api/Evaluaciones/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Evaluacion>> Detalle(int id)
    {
        var evaluacion = await _context.Evaluaciones
            .Include(e => e.Grupo).ThenInclude(g => g.Materia)
            .Include(e => e.IntentosEvaluacion).ThenInclude(i => i.Estudiante)
            .Include(e => e.IntentosEvaluacion).ThenInclude(i => i.Equipo)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (evaluacion == null)
            return NotFound("La evaluación no existe.");

        if (User.IsInRole("Docente"))
        {
            var grupo = await _context.Grupos.Include(g => g.Docente).FirstOrDefaultAsync(g => g.Id == evaluacion.GrupoId);
            if (grupo?.Docente?.ApplicationUserId != UserId)
                return Forbid();
        }

        return Ok(evaluacion);
    }

    // GET: api/Evaluaciones/todas (ADMIN)
    [HttpGet("todas")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Evaluacion>>> GetTodas()
    {
        var evaluaciones = await _context.Evaluaciones
            .Include(e => e.Grupo).ThenInclude(g => g.Materia)
            .Include(e => e.Grupo).ThenInclude(g => g.Docente)
            .Include(e => e.IntentosEvaluacion)
            .OrderByDescending(e => e.Id)
            .ToListAsync();

        return Ok(evaluaciones);
    }

    // DELETE: api/Evaluaciones/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Docente,Admin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var evaluacion = await _context.Evaluaciones
            .Include(e => e.Grupo).ThenInclude(g => g.Docente)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (evaluacion == null) return NotFound();

        if (User.IsInRole("Docente") && evaluacion.Grupo?.Docente?.ApplicationUserId != UserId)
            return Forbid();

        _context.Evaluaciones.Remove(evaluacion);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ---------- ESTUDIANTE ----------

    // GET: api/Evaluaciones/disponibles
    [HttpGet("disponibles")]
    [Authorize(Roles = "Estudiante")]
    public async Task<ActionResult<IEnumerable<object>>> GetEvaluacionesDisponibles()
    {
        var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.ApplicationUserId == UserId);
        if (estudiante == null) return BadRequest("Perfil de estudiante no encontrado.");

        var ahora = DateTime.Now;
        var evaluaciones = await _context.Evaluaciones
            .Include(e => e.Grupo).ThenInclude(g => g.Materia)
            .Include(e => e.IntentosEvaluacion)
            .Where(e => e.FechaInicio <= ahora && e.FechaFin >= ahora)
            .Select(e => new
            {
                e.Id,
                e.Titulo,
                e.FechaInicio,
                e.FechaFin,
                e.DuracionMinutos,
                Grupo = new { e.Grupo.Nombre, Materia = new { e.Grupo.Materia.Nombre, e.Grupo.Materia.Codigo } },
                YaRendido = e.IntentosEvaluacion.Any(i => i.EstudianteId == estudiante.Id && i.FechaFin != null),
                IntentoPendienteId = e.IntentosEvaluacion
                    .Where(i => i.EstudianteId == estudiante.Id && i.FechaFin == null)
                    .Select(i => (int?)i.Id)
                    .FirstOrDefault(),
                PuntajeObtenido = e.IntentosEvaluacion
                    .Where(i => i.EstudianteId == estudiante.Id && i.FechaFin != null)
                    .Select(i => i.Puntaje)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(evaluaciones);
    }

    // POST: api/Evaluaciones/iniciar
    [HttpPost("iniciar")]
    [Authorize(Roles = "Estudiante")]
    public async Task<ActionResult<IntentoEvaluacion>> IniciarIntento([FromBody] IniciarIntentoDto dto)
    {
        var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.ApplicationUserId == UserId);
        if (estudiante == null) return BadRequest("Estudiante no encontrado.");

        var evaluacion = await _context.Evaluaciones.FindAsync(dto.EvaluacionId);
        if (evaluacion == null) return NotFound("La evaluación no existe.");

        var ahora = DateTime.Now;
        if (ahora < evaluacion.FechaInicio || ahora > evaluacion.FechaFin)
            return BadRequest("La evaluación no se encuentra disponible en este horario.");

        // Validar si ya la completó
        var yaCompleto = await _context.IntentosEvaluacion
            .AnyAsync(i => i.EvaluacionId == dto.EvaluacionId && i.EstudianteId == estudiante.Id && i.FechaFin != null);
        if (yaCompleto)
            return BadRequest("Ya has completado esta evaluación.");

        // Si ya tiene un intento en curso, devolverlo
        var intentoEnCurso = await _context.IntentosEvaluacion
            .FirstOrDefaultAsync(i => i.EvaluacionId == dto.EvaluacionId && i.EstudianteId == estudiante.Id && i.FechaFin == null);
        if (intentoEnCurso != null)
            return Ok(intentoEnCurso);

        // Validar MAC y Equipo de Laboratorio
        var mac = string.IsNullOrWhiteSpace(dto.MacAddress) ? "00:00:00:00:00:00" : dto.MacAddress.Replace('-', ':').ToUpper();

        // Validar si el estudiante ya tiene un dispositivo registrado
        if (!string.IsNullOrEmpty(estudiante.MacAddress) && estudiante.MacAddress != mac)
        {
            return BadRequest($"Acceso denegado: Tu cuenta solo puede rendir evaluaciones desde tu dispositivo registrado (MAC: {estudiante.MacAddress}). Tu dispositivo actual detectado es '{mac}'.");
        }

        if (string.IsNullOrEmpty(estudiante.MacAddress) && mac != "00:00:00:00:00:00")
        {
            var otroEstudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.Id != estudiante.Id && e.MacAddress == mac);
            if (otroEstudiante != null)
            {
                return BadRequest($"Acceso denegado: Este dispositivo (MAC: {mac}) ya pertenece a otro estudiante ({otroEstudiante.Nombre} {otroEstudiante.Apellido}).");
            }
            estudiante.MacAddress = mac;
            await _context.SaveChangesAsync();
        }

        var equipo = await _context.Equipos.FirstOrDefaultAsync(e => e.MacAddress == mac);
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
            return BadRequest($"Acceso denegado: El equipo (MAC: {mac}) no es un equipo autorizado del laboratorio ni coincide con tu dispositivo registrado.");

        var intento = new IntentoEvaluacion
        {
            EvaluacionId = dto.EvaluacionId,
            EstudianteId = estudiante.Id,
            EquipoId = equipo.Id,
            FechaInicio = DateTime.Now
        };

        _context.IntentosEvaluacion.Add(intento);
        await _context.SaveChangesAsync();

        return Ok(intento);
    }

    // POST: api/Evaluaciones/finalizar
    [HttpPost("finalizar")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> FinalizarIntento([FromBody] FinalizarIntentoDto dto)
    {
        var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.ApplicationUserId == UserId);
        if (estudiante == null) return BadRequest("Estudiante no encontrado.");

        var intento = await _context.IntentosEvaluacion
            .FirstOrDefaultAsync(i => i.Id == dto.IntentoId && i.EstudianteId == estudiante.Id);

        if (intento == null) return NotFound("El intento de evaluación no existe.");

        if (intento.FechaFin != null)
            return BadRequest("Este intento ya fue finalizado previamente.");

        intento.FechaFin = DateTime.Now;
        intento.Puntaje = Math.Clamp(dto.Puntaje, 0, 100);

        await _context.SaveChangesAsync();
        return Ok(intento);
    }
}
