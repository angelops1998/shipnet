using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize(Roles = "Admin")]
public class CarrerasController : Controller
{
    private readonly ICarreraService _carreraService;
    private readonly IMateriaService _materiaService;

    public CarrerasController(ICarreraService carreraService, IMateriaService materiaService)
    {
        _carreraService = carreraService;
        _materiaService = materiaService;
    }

    public async Task<IActionResult> Index()
    {
        var carreras = await _carreraService.GetCarrerasAsync();
        return View(carreras);
    }

    public IActionResult Crear() => View(new CarreraDto());

    [HttpPost]
    public async Task<IActionResult> Crear(CarreraDto carrera)
    {
        if (!ModelState.IsValid)
            return View(carrera);

        var (exito, error) = await _carreraService.CrearAsync(carrera);
        if (exito)
        {
            TempData["Mensaje"] = "Carrera creada con éxito.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        TempData["Mensaje"] = error ?? "No se pudo crear la carrera.";
        TempData["TipoMensaje"] = "danger";
        return View(carrera);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var carrera = await _carreraService.GetByIdAsync(id);
        if (carrera == null) return NotFound();
        return View(carrera);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, CarreraDto carrera)
    {
        carrera.Id = id;
        if (await _carreraService.ActualizarAsync(id, carrera))
        {
            TempData["Mensaje"] = "Carrera actualizada correctamente.";
            TempData["TipoMensaje"] = "success";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo actualizar la carrera.";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        if (await _carreraService.EliminarAsync(id))
        {
            TempData["Mensaje"] = "Carrera eliminada.";
            TempData["TipoMensaje"] = "warning";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo eliminar la carrera (puede tener materias asociadas).";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }
}
