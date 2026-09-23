using ShipNetMvc.Models;
using ShipNetMvc.ViewModels;

namespace ShipNetMvc.Services;

public interface IAuthService
{
    // null = correo o contraseña incorrectos
    Task<AuthResponseDto?> LoginAsync(LoginViewModel vm);
    // Error != null = la API rechazó el registro (ej: correo repetido)
    Task<(AuthResponseDto? Respuesta, string? Error)> RegisterAsync(RegisterViewModel vm);
}
