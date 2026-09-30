using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize(Roles = "Admin,Docente")]
public class GruposController : Controller
{
    private readonly IGrupoService _grupoService;
    private readonly IMateriaService _materiaService;
    private readonly IDocenteService _docenteService;

    public GruposController(IGrupoService grupoService, IMateriaService materiaService, IDocenteService docenteService)
    {
        _grupoService = grupoService;
        _materiaService = materiaService;
        _docenteService = docenteService;
    }

    public async Task<IActionResult> Index()
    {
        List<GrupoDto> grupos;
        if (User.IsInRole("Admin"))
        {
            grupos = await _grupoService.GetGruposAsync();
        }
        else
        {
            grupos = await _grupoService.GetGruposDocenteAsync();
        }
        return View(grupos);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Crear()
    {
        await CargarViewBagsAsync();
        return View(new GrupoDto { Modulo = UpdsCalendario.ModuloActual.Nombre + " (" + UpdsCalendario.ModuloActual.Mes + ")", Turno = "Mañana (07:30 - 10:00)", Activo = true });
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Crear(GrupoDto grupo)
    {
        if (await _grupoService.CrearAsync(grupo))
        {
            TempData["Mensaje"] = "Materia y docente asignados correctamente al módulo.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        TempData["Mensaje"] = "No se pudo guardar la asignación.";
        TempData["TipoMensaje"] = "danger";
        await CargarViewBagsAsync();
        return View(grupo);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var grupo = await _grupoService.GetByIdAsync(id);
        if (grupo == null)
        {
            TempData["Mensaje"] = "Asignación de grupo no encontrada.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction("Index");
        }
        await CargarViewBagsAsync(grupo.MateriaId, grupo.DocenteId);
        return View(grupo);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, GrupoDto grupo)
    {
        // Si el usuario es Docente, preservar su DocenteId original para evitar alteraciones
        if (User.IsInRole("Docente"))
        {
            var actual = await _grupoService.GetByIdAsync(id);
            if (actual != null)
            {
                grupo.DocenteId = actual.DocenteId;
            }
        }

        if (await _grupoService.ActualizarAsync(id, grupo))
        {
            TempData["Mensaje"] = "Materia y grupo modular actualizados con éxito.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        TempData["Mensaje"] = "No se pudo actualizar el grupo.";
        TempData["TipoMensaje"] = "danger";
        await CargarViewBagsAsync(grupo.MateriaId, grupo.DocenteId);
        return View(grupo);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleActivo(int id)
    {
        if (await _grupoService.ToggleActivoAsync(id))
        {
            TempData["Mensaje"] = "Estado del módulo actualizado.";
            TempData["TipoMensaje"] = "info";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo cambiar el estado del módulo.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _grupoService.EliminarAsync(id);
        TempData["Mensaje"] = "Grupo eliminado.";
        TempData["TipoMensaje"] = "warning";
        return RedirectToAction("Index");
    }

    private async Task CargarViewBagsAsync(int? materiaId = null, int? docenteId = null)
    {
        var materias = await _materiaService.GetMateriasAsync();
        ViewBag.Materias = new SelectList(materias.Select(m => new {
            m.Id,
            Nombre = $"{m.Nombre} — {m.Carrera?.Nombre ?? "Tronco Común"} ({m.Semestre}° Semestre)"
        }), "Id", "Nombre", materiaId);

        if (User.IsInRole("Admin"))
        {
            var docentes = await _docenteService.GetDocentesAsync();
            ViewBag.Docentes = new SelectList(docentes.Select(d => new { 
                d.Id, 
                Nombre = $"{d.Nombre} {d.Apellido}{(string.IsNullOrEmpty(d.Especialidad) ? "" : $" [{d.Especialidad}]")}" 
            }), "Id", "Nombre", docenteId);
        }

        var modulos = UpdsCalendario.ObtenerModulos2026()
            .Select(m => $"{m.Nombre} ({m.Mes})")
            .ToList();
        ViewBag.Modulos = new SelectList(modulos, UpdsCalendario.ModuloActual.Nombre + " (" + UpdsCalendario.ModuloActual.Mes + ")");

        ViewBag.Turnos = new SelectList(new List<string>
        {
            "Mañana (07:30 - 10:00)",
            "Mediodía (11:00 - 13:30)",
            "Tarde (14:00 - 16:30)",
            "Noche (19:00 - 21:30)"
        });
    }
}
