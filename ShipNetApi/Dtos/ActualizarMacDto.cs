namespace ShipNetApi.Dtos;

public class ActualizarMacDto
{
    public string? MacAddress { get; set; }
    public string? Carrera { get; set; }
    public int Semestre { get; set; } = 1;
}
