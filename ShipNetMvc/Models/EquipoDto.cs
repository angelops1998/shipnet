namespace ShipNetMvc.Models;

// Lo que viaja en JSON desde/hacia GET/POST api/Equipos
public class EquipoDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
}
