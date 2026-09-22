using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shipnet.Data;
using shipnet.Models;

namespace shipnet.Controllers;

[Authorize(Roles = "Admin")]
public class MateriasController : Controller
{
    private readonly ApplicationDbContext _db;
    public MateriasController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Materias.ToListAsync());

    public IActionResult Crear() => View();

    [HttpPost]
    public async Task<IActionResult> Crear(Materia materia)
    {
        _db.Materias.Add(materia);
        await _db.SaveChangesAsync();
        TempData["Mensaje"] = "Materia guardada.";
        TempData["TipoMensaje"] = "success";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        var materia = await _db.Materias.FindAsync(id);
        if (materia != null)
        {
            _db.Materias.Remove(materia);
            await _db.SaveChangesAsync();
        }
        TempData["Mensaje"] = "Materia eliminada.";
        TempData["TipoMensaje"] = "warning";
        return RedirectToAction("Index");
    }
}