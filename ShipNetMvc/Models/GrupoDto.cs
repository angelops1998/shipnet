namespace ShipNetMvc.Models;

public class GrupoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int MateriaId { get; set; }
    public int DocenteId { get; set; }
    // La API las manda al listar (Include); al crear van en null
    public MateriaDto? Materia { get; set; }
    public DocenteDto? Docente { get; set; }
}
