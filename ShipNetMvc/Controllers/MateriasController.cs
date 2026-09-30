using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize(Roles = "Admin")]
public class MateriasController : Controller
{
    private readonly IMateriaService _materiaService;
    private readonly ICarreraService _carreraService;

    public MateriasController(IMateriaService materiaService, ICarreraService carreraService)
    {
        _materiaService = materiaService;
        _carreraService = carreraService;
    }

    public async Task<IActionResult> Index()
    {
        var materias = await _materiaService.GetMateriasAsync();
        return View(materias);
    }

    public async Task<IActionResult> Crear()
    {
        var carreras = await _carreraService.GetCarrerasAsync();
        ViewBag.Carreras = new SelectList(carreras, "Id", "Nombre");
        return View(new MateriaDto());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(MateriaDto materia)
    {
        if (await _materiaService.CrearAsync(materia))
        {
            TempData["Mensaje"] = "Materia guardada.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        TempData["Mensaje"] = "No se pudo guardar la materia (¿código repetido?).";
        TempData["TipoMensaje"] = "danger";
        var carreras = await _carreraService.GetCarrerasAsync();
        ViewBag.Carreras = new SelectList(carreras, "Id", "Nombre", materia.CarreraId);
        return View(materia);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var materia = await _materiaService.GetByIdAsync(id);
        if (materia == null)
        {
            TempData["Mensaje"] = "Materia no encontrada.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction("Index");
        }
        var carreras = await _carreraService.GetCarrerasAsync();
        ViewBag.Carreras = new SelectList(carreras, "Id", "Nombre", materia.CarreraId);
        return View(materia);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, MateriaDto materia)
    {
        if (await _materiaService.ActualizarAsync(id, materia))
        {
            TempData["Mensaje"] = "Materia actualizada.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        TempData["Mensaje"] = "No se pudo actualizar la materia.";
        TempData["TipoMensaje"] = "danger";
        var carreras = await _carreraService.GetCarrerasAsync();
        ViewBag.Carreras = new SelectList(carreras, "Id", "Nombre", materia.CarreraId);
        return View(materia);
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        await _materiaService.EliminarAsync(id);
        TempData["Mensaje"] = "Materia eliminada.";
        TempData["TipoMensaje"] = "warning";
        return RedirectToAction("Index");
    }
}
