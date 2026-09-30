namespace ShipNetMvc.Models;

public class MiMateriaDto
{
    public bool TieneMateria { get; set; }
    public string? MacAddress { get; set; }
    public string? Carrera { get; set; }
    public int Semestre { get; set; } = 1;
    public int TotalInscritas { get; set; }
    public bool LimiteAlcanzado { get; set; }
    public List<string> TurnosOcupados { get; set; } = new();
    public List<InscripcionMateriaDto> Inscripciones { get; set; } = new();
    public GrupoDto? Grupo { get; set; }
}

public class InscripcionMateriaDto
{
    public int InscripcionId { get; set; }
    public int GrupoId { get; set; }
    public string GrupoNombre { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string Turno { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public string MateriaNombre { get; set; } = string.Empty;
    public string DocenteNombre { get; set; } = string.Empty;
    public MateriaDto? Materia { get; set; }
    public DocenteDto? Docente { get; set; }
    public DateTime FechaInscripcion { get; set; }
}
