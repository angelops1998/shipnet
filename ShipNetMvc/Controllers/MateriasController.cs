using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize(Roles = "Admin")]
public class MateriasController : Controller
{
    private readonly IMateriaService _materiaService;
    public MateriasController(IMateriaService materiaService) => _materiaService = materiaService;

    public async Task<IActionResult> Index() => View(await _materiaService.GetMateriasAsync());

    public IActionResult Crear() => View();

    [HttpPost]
    public async Task<IActionResult> Crear(MateriaDto materia)
    {
        if (await _materiaService.CrearAsync(materia))
        {
            TempData["Mensaje"] = "Materia guardada.";
            TempData["TipoMensaje"] = "success";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo guardar la materia (¿código repetido?).";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        await _materiaService.EliminarAsync(id);
        TempData["Mensaje"] = "Materia eliminada.";
        TempData["TipoMensaje"] = "warning";
        return RedirectToAction("Index");
    }
}
