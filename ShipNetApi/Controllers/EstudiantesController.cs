using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipNetApi.Data;
using ShipNetApi.Models;
using ShipNetApi.Dtos;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EstudiantesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EstudiantesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    // GET: api/Estudiantes (Admin, Docente)
    [HttpGet]
    [Authorize(Roles = "Admin,Docente")]
    public async Task<ActionResult<IEnumerable<object>>> GetEstudiantes()
    {
        var estudiantes = await (from e in _context.Estudiantes
                                 join u in _context.Users on e.ApplicationUserId equals u.Id into ug
                                 from u in ug.DefaultIfEmpty()
                                 select new
                                 {
                                     e.Id,
                                     e.Nombre,
                                     e.Apellido,
                                     e.CI,
                                     Email = u != null ? u.Email : null,
                                     e.ApplicationUserId,
                                     e.GrupoId,
                                     e.MacAddress,
                                     e.Carrera,
                                     e.Semestre,
                                     Grupo = e.Grupo != null ? new
                                     {
                                         e.Grupo.Id,
                                         e.Grupo.Nombre,
                                         e.Grupo.Modulo,
                                         e.Grupo.Turno,
                                         e.Grupo.MateriaId,
                                         e.Grupo.DocenteId,
                                         e.Grupo.Activo,
                                         Materia = e.Grupo.Materia != null ? new
                                         {
                                             e.Grupo.Materia.Id,
                                             e.Grupo.Materia.Nombre,
                                             e.Grupo.Materia.Codigo,
                                             e.Grupo.Materia.Semestre
                                         } : null,
                                         Docente = e.Grupo.Docente != null ? new
                                         {
                                             e.Grupo.Docente.Id,
                                             e.Grupo.Docente.Nombre,
                                             e.Grupo.Docente.Apellido
                                         } : null
                                     } : null,
                                     Inscripciones = e.Inscripciones.Select(i => new
                                     {
                                         InscripcionId = i.Id,
                                         i.GrupoId,
                                         GrupoNombre = i.Grupo.Nombre,
                                         Turno = i.Grupo.Turno,
                                         Modulo = i.Grupo.Modulo,
                                         Activo = i.Grupo.Activo,
                                         MateriaNombre = i.Grupo.Materia != null ? i.Grupo.Materia.Nombre : "",
                                         DocenteNombre = i.Grupo.Docente != null ? $"{i.Grupo.Docente.Nombre} {i.Grupo.Docente.Apellido}" : "",
                                         Materia = i.Grupo.Materia != null ? new
                                         {
                                             i.Grupo.Materia.Id,
                                             i.Grupo.Materia.Nombre,
                                             i.Grupo.Materia.Codigo,
                                             i.Grupo.Materia.Semestre
                                         } : null,
                                         Docente = i.Grupo.Docente != null ? new
                                         {
                                             i.Grupo.Docente.Id,
                                             i.Grupo.Docente.Nombre,
                                             i.Grupo.Docente.Apellido
                                         } : null,
                                         i.FechaInscripcion
                                     }),
                                     TotalAsistencias = e.RegistrosAsistencia.Count
                                 })
                                 .ToListAsync();

        return Ok(estudiantes);
    }

    // GET: api/Estudiantes/mi-materia (Estudiante)
    [HttpGet("mi-materia")]
    [Authorize(Roles = "Estudiante")]
    public async Task<ActionResult<object>> GetMiMateria()
    {
        var estudiante = await _context.Estudiantes
            .Include(e => e.Inscripciones).ThenInclude(i => i.Grupo).ThenInclude(g => g.Materia)
            .Include(e => e.Inscripciones).ThenInclude(i => i.Grupo).ThenInclude(g => g.Docente)
            .Include(e => e.Grupo).ThenInclude(g => g!.Materia)
            .Include(e => e.Grupo).ThenInclude(g => g!.Docente)
            .FirstOrDefaultAsync(e => e.ApplicationUserId == UserId);

        if (estudiante == null) return NotFound("Estudiante no encontrado.");

        var listaInscripciones = estudiante.Inscripciones.Select(i => new
        {
            InscripcionId = i.Id,
            GrupoId = i.GrupoId,
            GrupoNombre = i.Grupo.Nombre,
            Modulo = i.Grupo.Modulo,
            Turno = i.Grupo.Turno,
            Activo = i.Grupo.Activo,
            Materia = i.Grupo.Materia != null ? new { i.Grupo.Materia.Id, i.Grupo.Materia.Nombre, i.Grupo.Materia.Codigo, i.Grupo.Materia.Semestre } : null,
            Docente = i.Grupo.Docente != null ? new { i.Grupo.Docente.Id, i.Grupo.Docente.Nombre, i.Grupo.Docente.Apellido } : null,
            FechaInscripcion = i.FechaInscripcion
        }).ToList();

        var turnosOcupados = estudiante.Inscripciones
            .Where(i => !string.IsNullOrWhiteSpace(i.Grupo.Turno))
            .Select(i => i.Grupo.Turno)
            .ToList();

        var tieneMateria = listaInscripciones.Any() || estudiante.Grupo != null;

        return Ok(new
        {
            TieneMateria = tieneMateria,
            MacAddress = estudiante.MacAddress,
            Carrera = estudiante.Carrera,
            Semestre = estudiante.Semestre,
            TotalInscritas = listaInscripciones.Count,
            LimiteAlcanzado = listaInscripciones.Count >= 7,
            TurnosOcupados = turnosOcupados,
            Inscripciones = listaInscripciones,
            Grupo = estudiante.Grupo != null ? new
            {
                estudiante.Grupo.Id,
                estudiante.Grupo.Nombre,
                estudiante.Grupo.Modulo,
                estudiante.Grupo.Turno,
                estudiante.Grupo.Activo,
                Materia = estudiante.Grupo.Materia != null ? new { estudiante.Grupo.Materia.Id, estudiante.Grupo.Materia.Nombre, estudiante.Grupo.Materia.Codigo } : null,
                Docente = estudiante.Grupo.Docente != null ? new { estudiante.Grupo.Docente.Id, estudiante.Grupo.Docente.Nombre, estudiante.Grupo.Docente.Apellido } : null
            } : null
        });
    }

    // POST: api/Estudiantes/inscribir/5 (Estudiante se inscribe a su materia: máx 1 por turno, máx 7 por semestre)
    [HttpPost("inscribir/{grupoId}")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> InscribirMateria(int grupoId)
    {
        var estudiante = await _context.Estudiantes
            .Include(e => e.Inscripciones).ThenInclude(i => i.Grupo).ThenInclude(g => g.Materia)
            .FirstOrDefaultAsync(e => e.ApplicationUserId == UserId);
        if (estudiante == null) return NotFound("Estudiante no encontrado.");

        var grupo = await _context.Grupos.Include(g => g.Materia).FirstOrDefaultAsync(g => g.Id == grupoId);
        if (grupo == null) return BadRequest("El grupo modular seleccionado no existe.");

        // Regla 1: Máximo 7 materias por semestre
        if (estudiante.Inscripciones.Count >= 7)
        {
            return BadRequest("Límite alcanzado: En la UPDS solo puedes inscribir hasta un máximo de 7 materias por semestre.");
        }

        // Regla 2: Solo una materia por turno
        var turnoConflicto = estudiante.Inscripciones.FirstOrDefault(i =>
            !string.IsNullOrWhiteSpace(grupo.Turno) &&
            string.Equals(i.Grupo.Turno?.Trim(), grupo.Turno?.Trim(), StringComparison.OrdinalIgnoreCase));
        if (turnoConflicto != null)
        {
            var nombreConflicto = turnoConflicto.Grupo?.Materia?.Nombre ?? "otra materia";
            return BadRequest($"Conflicto de horario: Ya tienes inscrita la materia '{nombreConflicto}' en el turno '{grupo.Turno}'. Solo puedes inscribir una materia por turno.");
        }

        // Regla 3: No duplicar la misma materia en distintos paralelos
        if (estudiante.Inscripciones.Any(i => i.Grupo.MateriaId == grupo.MateriaId))
        {
            return BadRequest($"Ya estás inscrito en la materia '{grupo.Materia?.Nombre}'. No puedes inscribir la misma materia más de una vez.");
        }

        // Registrar inscripción
        var inscripcion = new Inscripcion
        {
            EstudianteId = estudiante.Id,
            GrupoId = grupo.Id,
            FechaInscripcion = DateTime.Now
        };
        _context.Inscripciones.Add(inscripcion);

        // Mantener grupoId principal actualizado
        estudiante.GrupoId = grupo.Id;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Mensaje = $"Inscrito exitosamente a {grupo.Materia?.Nombre} en el turno {grupo.Turno}.",
            TotalInscritas = estudiante.Inscripciones.Count + 1
        });
    }

    // POST: api/Estudiantes/retirar/5 (Estudiante da de baja una materia para liberar su turno)
    [HttpPost("retirar/{grupoId}")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> RetirarMateria(int grupoId)
    {
        var estudiante = await _context.Estudiantes
            .Include(e => e.Inscripciones).ThenInclude(i => i.Grupo)
            .FirstOrDefaultAsync(e => e.ApplicationUserId == UserId);
        if (estudiante == null) return NotFound("Estudiante no encontrado.");

        var inscripcion = estudiante.Inscripciones.FirstOrDefault(i => i.GrupoId == grupoId);
        if (inscripcion == null)
            return BadRequest("No te encuentras inscrito en este grupo.");

        _context.Inscripciones.Remove(inscripcion);

        // Si era el grupo principal asignado, apuntar al siguiente o null
        if (estudiante.GrupoId == grupoId)
        {
            var siguiente = estudiante.Inscripciones.FirstOrDefault(i => i.GrupoId != grupoId);
            estudiante.GrupoId = siguiente?.GrupoId;
        }

        await _context.SaveChangesAsync();
        return Ok(new { Mensaje = "Materia retirada correctamente. El turno ha quedado liberado." });
    }

    // PUT: api/Estudiantes/{id}/mac (Admin updates student MAC)
    [HttpPut("{id}/mac")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActualizarMac(int id, [FromBody] ActualizarMacDto dto)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);
        if (estudiante == null) return NotFound();
        var mac = string.IsNullOrWhiteSpace(dto.MacAddress) ? null : dto.MacAddress.Replace('-', ':').ToUpper();
        estudiante.MacAddress = mac;
        estudiante.Carrera = dto.Carrera;
        estudiante.Semestre = dto.Semestre;

        // Si se asignó una MAC (ej. teléfono o terminal), asegurar que exista como equipo autorizado en el sistema
        if (!string.IsNullOrEmpty(mac))
        {
            var equipoExistente = await _context.Equipos.FirstOrDefaultAsync(e => e.MacAddress == mac);
            if (equipoExistente == null)
            {
                _context.Equipos.Add(new Equipo
                {
                    Codigo = $"MOVIL-{estudiante.Id}",
                    MacAddress = mac,
                    Ubicacion = $"Móvil - {estudiante.Nombre} {estudiante.Apellido}"
                });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { estudiante.Id, estudiante.MacAddress, estudiante.Carrera, estudiante.Semestre, Mensaje = "Datos del estudiante y MAC autorizada actualizados correctamente." });
    }

    // POST: api/Estudiantes/auto-vincular-mac (Auto-guarda la MAC detectada si el estudiante no tiene MAC asignada)
    [HttpPost("auto-vincular-mac")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> AutoVincularMac([FromBody] ActualizarMacDto dto)
    {
        var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.ApplicationUserId == UserId);
        if (estudiante == null) return NotFound("Estudiante no encontrado.");

        if (string.IsNullOrWhiteSpace(dto.MacAddress) || dto.MacAddress == "00:00:00:00:00:00")
            return BadRequest("MAC no válida.");

        var mac = dto.MacAddress.Replace('-', ':').ToUpper();

        // Verificar si la MAC ya pertenece a otro estudiante
        var otroEstudiante = await _context.Estudiantes
            .FirstOrDefaultAsync(e => e.Id != estudiante.Id && e.MacAddress == mac);
        if (otroEstudiante != null)
        {
            return BadRequest($"Este dispositivo (MAC: {mac}) ya se encuentra registrado con el estudiante {otroEstudiante.Nombre} {otroEstudiante.Apellido}. Solo se permite una cuenta por dispositivo.");
        }

        // Si no tiene MAC asignada, auto-vincularla inmediatamente
        if (string.IsNullOrEmpty(estudiante.MacAddress))
        {
            estudiante.MacAddress = mac;

            // Asegurar que quede registrada en la tabla de Equipos autorizados
            var equipoExistente = await _context.Equipos.FirstOrDefaultAsync(e => e.MacAddress == mac);
            if (equipoExistente == null)
            {
                _context.Equipos.Add(new Equipo
                {
                    Codigo = $"MOVIL-{estudiante.Id}",
                    MacAddress = mac,
                    Ubicacion = $"Móvil - {estudiante.Nombre} {estudiante.Apellido}"
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { Exito = true, MacAddress = mac, Mensaje = "Dispositivo vinculado automáticamente a tu cuenta." });
        }

        return Ok(new { Exito = true, MacAddress = estudiante.MacAddress, Mensaje = "Ya tienes un dispositivo vinculado." });
    }
}
