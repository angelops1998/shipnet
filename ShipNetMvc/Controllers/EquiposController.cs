using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize(Roles = "Admin")]
public class EquiposController : Controller
{
    private readonly IEquipoService _equipoService;
    public EquiposController(IEquipoService equipoService) => _equipoService = equipoService;

    public async Task<IActionResult> Index() => View(await _equipoService.GetEquiposAsync());

    public IActionResult Crear() => View();

    [HttpPost]
    public async Task<IActionResult> Crear(EquipoDto equipo)
    {
        if (await _equipoService.CrearAsync(equipo))
        {
            TempData["Mensaje"] = "Equipo guardado.";
            TempData["TipoMensaje"] = "success";
        }
        else
        {
            TempData["Mensaje"] = "No se pudo guardar el equipo (¿MAC repetida?).";
            TempData["TipoMensaje"] = "danger";
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        await _equipoService.EliminarAsync(id);
        TempData["Mensaje"] = "Equipo eliminado.";
        TempData["TipoMensaje"] = "warning";
        return RedirectToAction("Index");
    }
}
