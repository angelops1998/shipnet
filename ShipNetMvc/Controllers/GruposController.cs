using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize(Roles = "Admin")]
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

    public async Task<IActionResult> Index() => View(await _grupoService.GetGruposAsync());

    public async Task<IActionResult> Crear()
    {
        ViewBag.Materias = new SelectList(await _materiaService.GetMateriasAsync(), "Id", "Nombre");
        var docentes = await _docenteService.GetDocentesAsync();
        ViewBag.Docentes = new SelectList(docentes.Select(d => new { d.Id, Nombre = d.Nombre + " " + d.Apellido }), "Id", "Nombre");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(GrupoDto grupo)
    {
        if (await _grupoService.CrearAsync(grupo))
        {
            TempData["Mensaje"] = "Grupo guardado.";
            TempData["TipoMensaje"] = "success";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo guardar el grupo.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        await _grupoService.EliminarAsync(id);
        TempData["Mensaje"] = "Grupo eliminado.";
        TempData["TipoMensaje"] = "warning";
        return RedirectToAction("Index");
    }
}
