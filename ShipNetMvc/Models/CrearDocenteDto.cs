using System.ComponentModel.DataAnnotations;

namespace ShipNetMvc.Models;

public class CrearDocenteDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(4, ErrorMessage = "Mínimo 4 caracteres")]
    public string Password { get; set; } = string.Empty;
}
