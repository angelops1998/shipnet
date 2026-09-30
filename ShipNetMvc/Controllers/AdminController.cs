using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IEstudianteService _estudianteService;
    private readonly IGrupoService _grupoService;
    private readonly IEquipoService _equipoService;
    private readonly IMateriaService _materiaService;
    private readonly IDocenteService _docenteService;
    private readonly IAsistenciaService _asistenciaService;
    private readonly IEvaluacionService _evaluacionService;
    private readonly INetworkService _networkService;
    private readonly ICarreraService _carreraService;

    public AdminController(
        IEstudianteService estudianteService,
        IGrupoService grupoService,
        IEquipoService equipoService,
        IMateriaService materiaService,
        IDocenteService docenteService,
        IAsistenciaService asistenciaService,
        IEvaluacionService evaluacionService,
        INetworkService networkService,
        ICarreraService carreraService)
    {
        _estudianteService = estudianteService;
        _grupoService = grupoService;
        _equipoService = equipoService;
        _materiaService = materiaService;
        _docenteService = docenteService;
        _asistenciaService = asistenciaService;
        _evaluacionService = evaluacionService;
        _networkService = networkService;
        _carreraService = carreraService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.ModuloActual = UpdsCalendario.ModuloActual;
        ViewBag.MacTerminal = _networkService.ObtenerMacCliente(HttpContext);

        var equipos = await _equipoService.GetEquiposAsync();
        var materias = await _materiaService.GetMateriasAsync();
        var docentes = await _docenteService.GetDocentesAsync();
        var estudiantes = await _estudianteService.GetEstudiantesAsync();
        var grupos = await _grupoService.GetGruposAsync();
        var sesiones = await _asistenciaService.GetTodasSesionesAsync();
        var evaluaciones = await _evaluacionService.GetTodasEvaluacionesAsync();
        var carreras = await _carreraService.GetCarrerasAsync();

        ViewBag.TotalEquipos = equipos.Count;
        ViewBag.TotalMaterias = materias.Count;
        ViewBag.TotalDocentes = docentes.Count;
        ViewBag.TotalEstudiantes = estudiantes.Count;
        ViewBag.TotalGrupos = grupos.Count;
        ViewBag.TotalSesiones = sesiones.Count;
        ViewBag.TotalEvaluaciones = evaluaciones.Count;
        ViewBag.TotalCarreras = carreras.Count;

        ViewBag.GruposRecientes = grupos.Take(6).ToList();
        ViewBag.SesionesRecientes = sesiones.Take(5).ToList();
        ViewBag.EvaluacionesRecientes = evaluaciones.Take(5).ToList();

        return View();
    }

    public async Task<IActionResult> Estudiantes()
    {
        var estudiantes = await _estudianteService.GetEstudiantesAsync();
        return View(estudiantes);
    }

    // GET: Gestión de MACs de estudiantes
    public async Task<IActionResult> Macs()
    {
        var estudiantes = await _estudianteService.GetEstudiantesAsync();
        return View(estudiantes);
    }

    // POST: Actualizar MAC de un estudiante
    [HttpPost]
    public async Task<IActionResult> ActualizarMac(int id, string? macAddress, string? carrera, int semestre)
    {
        var dto = new ActualizarMacEstudianteDto
        {
            MacAddress = string.IsNullOrWhiteSpace(macAddress) ? null : macAddress.Trim(),
            Carrera = string.IsNullOrWhiteSpace(carrera) ? null : carrera.Trim(),
            Semestre = semestre > 0 ? semestre : 1
        };

        var (exito, error) = await _estudianteService.ActualizarMacAsync(id, dto);
        if (exito)
        {
            TempData["Mensaje"] = "Datos del estudiante actualizados correctamente.";
            TempData["TipoMensaje"] = "success";
        }
        else
        {
            TempData["Mensaje"] = error ?? "No se pudo actualizar los datos.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Macs");
    }

    public async Task<IActionResult> Examenes()
    {
        var evaluaciones = await _evaluacionService.GetTodasEvaluacionesAsync();
        return View(evaluaciones);
    }

    public async Task<IActionResult> Asistencias()
    {
        var sesiones = await _asistenciaService.GetTodasSesionesAsync();
        return View(sesiones);
    }

    [AllowAnonymous]
    public IActionResult Calendario()
    {
        var modulos = UpdsCalendario.ObtenerModulos2026();
        return View(modulos);
    }
}