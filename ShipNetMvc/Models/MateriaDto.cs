namespace ShipNetMvc.Models;

public class MateriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int? CarreraId { get; set; }
    public int Semestre { get; set; } = 1;
    public CarreraDto? Carrera { get; set; }
}
