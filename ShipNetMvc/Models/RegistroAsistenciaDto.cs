namespace ShipNetMvc.Models;

public class RegistroAsistenciaDto
{
    public int Id { get; set; }
    public int SesionAsistenciaId { get; set; }
    public int EstudianteId { get; set; }
    public int EquipoId { get; set; }
    public TimeSpan HoraRegistro { get; set; }
    public string Estado { get; set; } = "Presente";
    public EstudianteDto Estudiante { get; set; } = new();
    public EquipoDto Equipo { get; set; } = new();
}
