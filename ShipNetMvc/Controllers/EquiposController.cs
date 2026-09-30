using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipNetMvc.Models;
using ShipNetMvc.Services;

namespace ShipNetMvc.Controllers;

[Authorize(Roles = "Admin")]
public class EquiposController : Controller
{
    private readonly IEquipoService _equipoService;
    private readonly INetworkService _networkService;

    public EquiposController(IEquipoService equipoService, INetworkService networkService)
    {
        _equipoService = equipoService;
        _networkService = networkService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.MacActual = _networkService.ObtenerMacCliente(HttpContext);
        return View(await _equipoService.GetEquiposAsync());
    }

    public IActionResult Crear()
    {
        ViewBag.MacActual = _networkService.ObtenerMacCliente(HttpContext);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(EquipoDto equipo)
    {
        if (await _equipoService.CrearAsync(equipo))
        {
            TempData["Mensaje"] = $"Equipo {equipo.Codigo} guardado exitosamente con MAC {equipo.MacAddress}.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        TempData["Mensaje"] = "No se pudo guardar el equipo. Verifica que la dirección MAC no esté ya registrada.";
        TempData["TipoMensaje"] = "danger";
        ViewBag.MacActual = _networkService.ObtenerMacCliente(HttpContext);
        return View(equipo);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var equipo = await _equipoService.GetByIdAsync(id);
        if (equipo == null)
        {
            TempData["Mensaje"] = "Equipo no encontrado.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction("Index");
        }
        ViewBag.MacActual = _networkService.ObtenerMacCliente(HttpContext);
        return View(equipo);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, EquipoDto equipo)
    {
        if (await _equipoService.ActualizarAsync(id, equipo))
        {
            TempData["Mensaje"] = "Datos del equipo actualizados.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        TempData["Mensaje"] = "No se pudo actualizar el equipo.";
        TempData["TipoMensaje"] = "danger";
        ViewBag.MacActual = _networkService.ObtenerMacCliente(HttpContext);
        return View(equipo);
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        await _equipoService.EliminarAsync(id);
        TempData["Mensaje"] = "Equipo eliminado del catálogo.";
        TempData["TipoMensaje"] = "warning";
        return RedirectToAction("Index");
    }
}
