using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shipnet.Data;
using shipnet.Models;

namespace shipnet.Controllers;

[Authorize(Roles = "Admin")]
public class GruposController : Controller
{
    private readonly ApplicationDbContext _db;
    public GruposController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Grupos.Include(g => g.Materia).Include(g => g.Docente).ToListAsync());

    public async Task<IActionResult> Crear()
    {
        ViewBag.Materias = new SelectList(await _db.Materias.ToListAsync(), "Id", "Nombre");
        var docentes = await _db.Docentes.ToListAsync();
        ViewBag.Docentes = new SelectList(docentes.Select(d => new { d.Id, Nombre = d.Nombre + " " + d.Apellido }), "Id", "Nombre");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Grupo grupo)
    {
        _db.Grupos.Add(grupo);
        await _db.SaveChangesAsync();
        TempData["Mensaje"] = "Grupo guardado.";
        TempData["TipoMensaje"] = "success";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        var grupo = await _db.Grupos.FindAsync(id);
        if (grupo != null)
        {
            _db.Grupos.Remove(grupo);
            await _db.SaveChangesAsync();
        }
        TempData["Mensaje"] = "Grupo eliminado.";
        TempData["TipoMensaje"] = "warning";
        return RedirectToAction("Index");
    }
}