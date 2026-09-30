using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShipNetMvc.Models;
using ShipNetMvc.Services;
using ShipNetMvc.ViewModels;

namespace ShipNetMvc.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly ICarreraService _carreraService;
    private readonly INetworkService _networkService;

    public AccountController(IAuthService authService, ICarreraService carreraService, INetworkService networkService)
    {
        _authService = authService;
        _carreraService = carreraService;
        _networkService = networkService;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        var respuesta = await _authService.LoginAsync(vm);
        if (respuesta != null)
        {
            await IniciarSesion(respuesta);
            return RedirectToAction("Index", "Home");
        }

        TempData["Mensaje"] = "Correo o contraseña incorrectos.";
        TempData["TipoMensaje"] = "danger";
        return View(vm);
    }

    public async Task<IActionResult> Register()
    {
        var mac = _networkService.ObtenerMacCliente(HttpContext);
        ViewBag.Mac = mac;
        ViewBag.Carreras = await _carreraService.GetCarrerasAsync();
        return View(new RegisterViewModel { MacAddress = mac });
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        var mac = _networkService.ObtenerMacCliente(HttpContext);
        ViewBag.Mac = mac;

        if (string.IsNullOrWhiteSpace(vm.MacAddress))
        {
            vm.MacAddress = mac;
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Carreras = await _carreraService.GetCarrerasAsync();
            return View(vm);
        }

        var (respuesta, error) = await _authService.RegisterAsync(vm);
        if (respuesta == null)
        {
            ViewBag.Carreras = await _carreraService.GetCarrerasAsync();
            TempData["Mensaje"] = error ?? "No se pudo completar el registro.";
            TempData["TipoMensaje"] = "danger";
            return View(vm);
        }

        await IniciarSesion(respuesta);
        TempData["Mensaje"] = "Registro exitoso. ¡Bienvenido a UPDS ShipNet!";
        TempData["TipoMensaje"] = "success";
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() => View();

    // Arma la cookie con lo que devolvió la API: correo, roles y el token para las próximas llamadas
    private async Task IniciarSesion(AuthResponseDto respuesta)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, respuesta.Email),
            new("NombreCompleto", respuesta.NombreCompleto),
            new("token", respuesta.Token)
        };
        claims.AddRange(respuesta.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        // La cookie vence junto con el token: así nunca queda una sesión abierta con un token ya inválido
        var propiedades = new AuthenticationProperties { IsPersistent = true, ExpiresUtc = respuesta.Expira };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad), propiedades);
    }
}
