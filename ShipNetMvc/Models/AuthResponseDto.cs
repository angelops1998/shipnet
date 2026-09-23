namespace ShipNetMvc.Models;

// Respuesta de POST api/Auth/login y api/Auth/register
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expira { get; set; }
    public string Email { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
