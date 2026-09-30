using System;

namespace ShipNetApi.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public int GrupoId { get; set; }
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        public Estudiante Estudiante { get; set; } = null!;
        public Grupo Grupo { get; set; } = null!;
    }
}
