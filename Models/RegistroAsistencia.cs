using System;

namespace shipnet.Models
{
    public class RegistroAsistencia
    {
        public int Id { get; set; }
        public int SesionAsistenciaId { get; set; }
        public int EstudianteId { get; set; }
        public int EquipoId { get; set; }
        public TimeSpan HoraRegistro { get; set; }
        public string Estado { get; set; } = "Presente";
        public SesionAsistencia SesionAsistencia { get; set; } = null!;
        public Estudiante Estudiante { get; set; } = null!;
        public Equipo Equipo { get; set; } = null!;
    }
}