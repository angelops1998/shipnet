using ShipNetMvc.Models;
using ShipNetMvc.ViewModels;

namespace ShipNetMvc.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    public AuthService(HttpClient httpClient) => _httpClient = httpClient;

    // POST api/Auth/login
    public async Task<AuthResponseDto?> LoginAsync(LoginViewModel vm)
    {
        var respuesta = await _httpClient.PostAsJsonAsync("Auth/login", vm);
        if (!respuesta.IsSuccessStatusCode) return null;
        return await respuesta.Content.ReadFromJsonAsync<AuthResponseDto>();
    }

    // POST api/Auth/register
    public async Task<(AuthResponseDto? Respuesta, string? Error)> RegisterAsync(RegisterViewModel vm)
    {
        var respuesta = await _httpClient.PostAsJsonAsync("Auth/register", vm);
        if (!respuesta.IsSuccessStatusCode)
            return (null, await respuesta.Content.ReadAsStringAsync());
        return (await respuesta.Content.ReadFromJsonAsync<AuthResponseDto>(), null);
    }
}
