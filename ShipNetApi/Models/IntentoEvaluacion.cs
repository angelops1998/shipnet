using System;

namespace ShipNetApi.Models
{
    public class IntentoEvaluacion
    {
        public int Id { get; set; }
        public int EvaluacionId { get; set; }
        public int EstudianteId { get; set; }
        public int EquipoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public decimal? Puntaje { get; set; }
        public Evaluacion Evaluacion { get; set; } = null!;
        public Estudiante Estudiante { get; set; } = null!;
        public Equipo Equipo { get; set; } = null!;
    }
}