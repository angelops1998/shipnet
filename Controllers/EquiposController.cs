using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shipnet.Data;
using shipnet.Models;

namespace shipnet.Controllers;

[Authorize(Roles = "Admin")]
public class EquiposController : Controller
{
    private readonly ApplicationDbContext _db;
    public EquiposController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Equipos.ToListAsync());

    public IActionResult Crear() => View();

    [HttpPost]
    public async Task<IActionResult> Crear(Equipo equipo)
    {
        equipo.MacAddress = equipo.MacAddress.Replace('-', ':').ToUpper();
        _db.Equipos.Add(equipo);
        await _db.SaveChangesAsync();
        TempData["Mensaje"] = "Equipo guardado.";
        TempData["TipoMensaje"] = "success";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        var equipo = await _db.Equipos.FindAsync(id);
        if (equipo != null)
        {
            _db.Equipos.Remove(equipo);
            await _db.SaveChangesAsync();
        }
        TempData["Mensaje"] = "Equipo eliminado.";
        TempData["TipoMensaje"] = "warning";
        return RedirectToAction("Index");
    }
}