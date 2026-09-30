using System.ComponentModel.DataAnnotations;

namespace ShipNetMvc.Models;

public class EvaluacionDto
{
    public int Id { get; set; }
    public int GrupoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int DuracionMinutos { get; set; }
    public string? PreguntasJson { get; set; }
    public GrupoDto Grupo { get; set; } = new();
    public List<IntentoEvaluacionDto> IntentosEvaluacion { get; set; } = new();
}

public class IntentoEvaluacionDto
{
    public int Id { get; set; }
    public int EvaluacionId { get; set; }
    public int EstudianteId { get; set; }
    public int EquipoId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public decimal? Puntaje { get; set; }
    public EvaluacionDto? Evaluacion { get; set; }
    public EstudianteDto? Estudiante { get; set; }
    public EquipoDto? Equipo { get; set; }
}

public class EvaluacionDisponibleDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int DuracionMinutos { get; set; }
    public GrupoDto Grupo { get; set; } = new();
    public bool YaRendido { get; set; }
    public int? IntentoPendienteId { get; set; }
    public decimal? PuntajeObtenido { get; set; }
}

public class CrearEvaluacionViewModel
{
    [Required(ErrorMessage = "Seleccione el grupo")]
    public int GrupoId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha y hora de inicio es obligatoria")]
    public DateTime FechaInicio { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "La fecha y hora de fin es obligatoria")]
    public DateTime FechaFin { get; set; } = DateTime.Now.AddHours(2);

    [Required(ErrorMessage = "Indique la duración en minutos")]
    [Range(5, 360, ErrorMessage = "La duración debe ser entre 5 y 360 minutos")]
    public int DuracionMinutos { get; set; } = 60;

    public string? PreguntasJson { get; set; }
}

public class PreguntaOpcionMultiple
{
    public int Numero { get; set; }
    public string Enunciado { get; set; } = string.Empty;
    public string? ImagenUrl { get; set; }
    public string OpcionA { get; set; } = string.Empty;
    public string? ImagenA { get; set; }
    public string OpcionB { get; set; } = string.Empty;
    public string? ImagenB { get; set; }
    public string OpcionC { get; set; } = string.Empty;
    public string? ImagenC { get; set; }
    public string OpcionD { get; set; } = string.Empty;
    public string? ImagenD { get; set; }
    public string Correcta { get; set; } = "A"; // "A", "B", "C", "D"
}

