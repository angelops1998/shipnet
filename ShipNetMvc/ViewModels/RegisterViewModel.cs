using System.ComponentModel.DataAnnotations;

namespace ShipNetMvc.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El carnet de identidad (CI) es obligatorio")]
    public string CI { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debes seleccionar tu carrera")]
    public string Carrera { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debes indicar tu semestre actual")]
    [Range(1, 10, ErrorMessage = "El semestre debe estar entre 1 y 10")]
    public int Semestre { get; set; } = 1;

    public string? MacAddress { get; set; }
}