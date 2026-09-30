using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize]
public class EvaluacionesController : Controller
{
    private readonly IEvaluacionService _evaluacionService;
    private readonly IAsistenciaService _asistenciaService;
    private readonly INetworkService _networkService;

    public EvaluacionesController(
        IEvaluacionService evaluacionService,
        IAsistenciaService asistenciaService,
        INetworkService networkService)
    {
        _evaluacionService = evaluacionService;
        _asistenciaService = asistenciaService;
        _networkService = networkService;
    }

    // ---------- DOCENTE ----------

    [Authorize(Roles = "Docente")]
    public async Task<IActionResult> Index()
    {
        var evaluaciones = await _evaluacionService.GetEvaluacionesDocenteAsync();
        return View(evaluaciones);
    }

    [Authorize(Roles = "Docente")]
    public async Task<IActionResult> Crear()
    {
        var grupos = await _asistenciaService.GetGruposDocenteAsync();
        ViewBag.Grupos = new SelectList(grupos.Select(g => new { g.Id, Nombre = (g.Materia != null ? g.Materia.Nombre + " - " : "") + g.Nombre }), "Id", "Nombre");
        return View(new CrearEvaluacionViewModel());
    }

    [HttpPost, Authorize(Roles = "Docente")]
    public async Task<IActionResult> Crear(CrearEvaluacionViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var grupos = await _asistenciaService.GetGruposDocenteAsync();
            ViewBag.Grupos = new SelectList(grupos.Select(g => new { g.Id, Nombre = (g.Materia != null ? g.Materia.Nombre + " - " : "") + g.Nombre }), "Id", "Nombre");
            return View(vm);
        }

        if (await _evaluacionService.CrearEvaluacionAsync(vm))
        {
            TempData["Mensaje"] = "Evaluación programada con éxito.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        TempData["Mensaje"] = "No se pudo crear la evaluación. Revisa las fechas ingresadas.";
        TempData["TipoMensaje"] = "danger";
        return View(vm);
    }

    [Authorize(Roles = "Docente")]
    public async Task<IActionResult> Detalle(int id)
    {
        var evaluacion = await _evaluacionService.GetDetalleEvaluacionAsync(id);
        if (evaluacion == null)
        {
            TempData["Mensaje"] = "Evaluación no encontrada.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction("Index");
        }
        return View(evaluacion);
    }

    [Authorize(Roles = "Docente")]
    public async Task<IActionResult> Eliminar(int id)
    {
        if (await _evaluacionService.EliminarEvaluacionAsync(id))
        {
            TempData["Mensaje"] = "Evaluación eliminada.";
            TempData["TipoMensaje"] = "warning";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo eliminar la evaluación.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }

    // ---------- ESTUDIANTE ----------

    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Disponibles()
    {
        var mac = _networkService.ObtenerMacCliente(HttpContext);
        ViewBag.Mac = mac;
        var evaluaciones = await _evaluacionService.GetEvaluacionesDisponiblesAsync();
        return View(evaluaciones);
    }

    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Rendir(int id)
    {
        var mac = _networkService.ObtenerMacCliente(HttpContext);
        var (exito, intento, error) = await _evaluacionService.IniciarIntentoAsync(id, mac);

        if (!exito || intento == null)
        {
            TempData["Mensaje"] = error ?? "No puedes rendir esta evaluación desde este dispositivo.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction("Disponibles");
        }

        var evaluacion = await _evaluacionService.GetDetalleEvaluacionAsync(id);
        if (evaluacion == null)
        {
            TempData["Mensaje"] = "No se pudo cargar la información de la evaluación.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction("Disponibles");
        }

        ViewBag.IntentoId = intento.Id;
        ViewBag.Mac = mac;
        return View(evaluacion);
    }


    [HttpPost, Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Finalizar(int intentoId, decimal puntaje)
    {
        if (await _evaluacionService.FinalizarIntentoAsync(intentoId, puntaje))
        {
            TempData["Mensaje"] = $"¡Evaluación enviada con éxito! Puntaje obtenido: {puntaje:0.0} / 100";
            TempData["TipoMensaje"] = "success";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo registrar la finalización del intento.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Disponibles");
    }
}
