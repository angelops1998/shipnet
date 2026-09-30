namespace ShipNetMvc.Models;

public class EstudianteConAsistenciasDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string CI { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string ApplicationUserId { get; set; } = string.Empty;
    public int? GrupoId { get; set; }
    public GrupoDto? Grupo { get; set; }
    public string? MacAddress { get; set; }
    public string? Carrera { get; set; }
    public int Semestre { get; set; } = 1;
    public int TotalAsistencias { get; set; }
    public List<InscripcionMateriaDto> Inscripciones { get; set; } = new();
}
