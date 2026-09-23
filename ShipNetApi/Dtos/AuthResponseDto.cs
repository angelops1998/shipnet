namespace ShipNetApi.Dtos;

// Lo que devuelve la API al iniciar sesión o registrarse
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expira { get; set; }
    public string Email { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
