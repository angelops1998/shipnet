using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shipnet.Data;
using shipnet.Models;
using shipnet.ViewModels;

namespace shipnet.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _db;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _db = db;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        var resultado = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, true, false);
        if (resultado.Succeeded) return RedirectToAction("Index", "Home");

        TempData["Mensaje"] = "Correo o contraseña incorrectos.";
        TempData["TipoMensaje"] = "danger";
        return View(vm);
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        var user = new ApplicationUser { UserName = vm.Email, Email = vm.Email, NombreCompleto = vm.Nombre + " " + vm.Apellido };
        var resultado = await _userManager.CreateAsync(user, vm.Password);
        if (!resultado.Succeeded)
        {
            TempData["Mensaje"] = "No se pudo registrar: " + resultado.Errors.First().Description;
            TempData["TipoMensaje"] = "danger";
            return View(vm);
        }

        await _userManager.AddToRoleAsync(user, "Estudiante");
        _db.Estudiantes.Add(new Estudiante { Nombre = vm.Nombre, Apellido = vm.Apellido, CI = vm.CI, ApplicationUserId = user.Id });
        await _db.SaveChangesAsync();

        await _signInManager.SignInAsync(user, true);
        TempData["Mensaje"] = "Registro exitoso. ¡Bienvenido!";
        TempData["TipoMensaje"] = "success";
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() => View();
}