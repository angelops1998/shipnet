using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize(Roles = "Admin")]
public class DocentesController : Controller
{
    private readonly IDocenteService _docenteService;

    public DocentesController(IDocenteService docenteService)
    {
        _docenteService = docenteService;
    }

    public async Task<IActionResult> Index()
    {
        var docentes = await _docenteService.GetDocentesAsync();
        return View(docentes);
    }

    public IActionResult Crear() => View(new CrearDocenteFormModel());

    [HttpPost]
    public async Task<IActionResult> Crear(CrearDocenteFormModel modelo)
    {
        if (!ModelState.IsValid)
            return View(modelo);

        var (exito, error) = await _docenteService.CrearDocenteAsync(modelo);
        if (exito)
        {
            TempData["Mensaje"] = "Docente registrado con éxito.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        TempData["Mensaje"] = error ?? "No se pudo registrar al docente.";
        TempData["TipoMensaje"] = "danger";
        return View(modelo);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var docente = await _docenteService.GetByIdAsync(id);
        if (docente == null) return NotFound();
        return View(docente);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, DocenteDto dto)
    {
        dto.Id = id;
        if (await _docenteService.ActualizarDocenteAsync(id, dto))
        {
            TempData["Mensaje"] = "Docente actualizado correctamente.";
            TempData["TipoMensaje"] = "success";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo actualizar al docente.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        if (await _docenteService.EliminarDocenteAsync(id))
        {
            TempData["Mensaje"] = "Docente eliminado correctamente.";
            TempData["TipoMensaje"] = "warning";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo eliminar al docente.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }
}
