namespace ShipNetMvc.Models;

public class SesionAsistenciaDto
{
    public int Id { get; set; }
    public int GrupoId { get; set; }
    public DateOnly Fecha { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraFin { get; set; }
    public bool Abierta { get; set; }
    public GrupoDto Grupo { get; set; } = new();
    public List<RegistroAsistenciaDto> RegistrosAsistencia { get; set; } = new();
}
