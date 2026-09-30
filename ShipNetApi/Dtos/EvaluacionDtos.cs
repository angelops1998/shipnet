namespace ShipNetApi.Dtos;

public class CrearEvaluacionDto
{
    public int GrupoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int DuracionMinutos { get; set; }
    public string? PreguntasJson { get; set; }
}


public class IniciarIntentoDto
{
    public int EvaluacionId { get; set; }
    public string? MacAddress { get; set; }
}

public class FinalizarIntentoDto
{
    public int IntentoId { get; set; }
    public decimal Puntaje { get; set; }
}
