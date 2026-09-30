using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize]
public class AsistenciaController : Controller
{
    private readonly IAsistenciaService _asistenciaService;
    private readonly INetworkService _networkService;
    private readonly IEstudianteService _estudianteService;
    private readonly IGrupoService _grupoService;

    public AsistenciaController(
        IAsistenciaService asistenciaService,
        INetworkService networkService,
        IEstudianteService estudianteService,
        IGrupoService grupoService)
    {
        _asistenciaService = asistenciaService;
        _networkService = networkService;
        _estudianteService = estudianteService;
        _grupoService = grupoService;
    }

    // ---------- DOCENTE (SN-09) ----------

    [Authorize(Roles = "Docente")]
    public async Task<IActionResult> Index()
    {
        var sesiones = await _asistenciaService.GetSesionesDocenteAsync();
        var grupos = await _asistenciaService.GetGruposDocenteAsync();
        ViewBag.GruposDocente = grupos;
        return View(sesiones);
    }

    [Authorize(Roles = "Docente")]
    public async Task<IActionResult> Abrir()
    {
        var grupos = await _asistenciaService.GetGruposDocenteAsync();
        // Filtrar o priorizar activos
        var gruposOrdenados = grupos
            .OrderByDescending(g => g.Activo)
            .Select(g => new
            {
                g.Id,
                Nombre = (g.Materia != null ? g.Materia.Nombre + " - " : "") + g.Nombre + (g.Activo ? " [ACTIVA ESTE MES]" : " [Inactiva]")
            });

        ViewBag.Grupos = new SelectList(gruposOrdenados, "Id", "Nombre");
        return View();
    }

    [HttpPost, Authorize(Roles = "Docente")]
    public async Task<IActionResult> Abrir(int grupoId)
    {
        if (await _asistenciaService.AbrirSesionAsync(grupoId))
        {
            TempData["Mensaje"] = "Sesión de asistencia abierta con éxito.";
            TempData["TipoMensaje"] = "success";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo abrir la sesión de asistencia.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Docente")]
    public async Task<IActionResult> Cerrar(int id)
    {
        if (await _asistenciaService.CerrarSesionAsync(id))
        {
            TempData["Mensaje"] = "Sesión de asistencia cerrada.";
            TempData["TipoMensaje"] = "warning";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo cerrar la sesión.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Docente,Admin")]
    public async Task<IActionResult> Detalle(int id)
    {
        var sesion = await _asistenciaService.GetDetalleAsync(id);
        if (sesion == null)
        {
            TempData["Mensaje"] = "No se encontró la sesión de asistencia.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction(User.IsInRole("Admin") ? "Asistencias" : "Index", User.IsInRole("Admin") ? "Admin" : "Asistencia");
        }
        return View(sesion);
    }

    // ---------- ESTUDIANTE (SN-11) ----------

    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Marcar()
    {
        var mac = _networkService.ObtenerMacCliente(HttpContext);
        ViewBag.Mac = mac;

        var miMateria = await _estudianteService.GetMiMateriaAsync();

        // Auto-vinculación en BD para demostración: Si el estudiante aún no tiene MAC registrada, auto-vincularla en BD
        if (miMateria != null && string.IsNullOrEmpty(miMateria.MacAddress) && !string.IsNullOrEmpty(mac) && mac != "00:00:00:00:00:00")
        {
            if (await _estudianteService.AutoVincularMacAsync(mac))
            {
                miMateria.MacAddress = mac;
            }
        }

        ViewBag.MiMateria = miMateria;

        var todosGrupos = await _grupoService.GetGruposAsync();
        ViewBag.GruposDisponibles = todosGrupos.Where(g => g.Activo).ToList();

        var sesionesAbiertas = await _asistenciaService.GetSesionesAbiertasAsync();
        return View(sesionesAbiertas);
    }

    [HttpPost, Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> InscribirMateria(int grupoId)
    {
        var (exito, mensaje) = await _estudianteService.InscribirMateriaAsync(grupoId);
        if (exito)
        {
            TempData["Mensaje"] = mensaje ?? "Materia inscrita correctamente en tu horario.";
            TempData["TipoMensaje"] = "success";
        }
        else
        {
            TempData["Mensaje"] = mensaje ?? "No se pudo inscribir la materia seleccionada.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Marcar");
    }

    [HttpPost, Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> RetirarMateria(int grupoId)
    {
        var (exito, mensaje) = await _estudianteService.RetirarMateriaAsync(grupoId);
        if (exito)
        {
            TempData["Mensaje"] = mensaje ?? "Materia retirada correctamente. El turno ha quedado disponible.";
            TempData["TipoMensaje"] = "warning";
        }
        else
        {
            TempData["Mensaje"] = mensaje ?? "No se pudo retirar la materia.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Marcar");
    }

    [HttpPost, Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Marcar(int sesionId)
    {
        var mac = _networkService.ObtenerMacCliente(HttpContext);
        var (exito, error) = await _asistenciaService.MarcarAsistenciaAsync(sesionId, mac);
        if (exito)
        {
            TempData["Mensaje"] = "¡Asistencia registrada correctamente!";
            TempData["TipoMensaje"] = "success";
        }
        else
        {
            TempData["Mensaje"] = error ?? "No se pudo registrar la asistencia.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Marcar");
    }

    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Historial()
    {
        var historial = await _asistenciaService.GetHistorialEstudianteAsync();
        return View(historial);
    }
}
