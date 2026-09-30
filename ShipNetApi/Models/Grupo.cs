using System.Collections.Generic;

namespace ShipNetApi.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int MateriaId { get; set; }
        public int DocenteId { get; set; }
        public string Modulo { get; set; } = "Módulo 9 (Septiembre)";
        public string Turno { get; set; } = "Mañana (07:30 - 10:00)";
        public bool Activo { get; set; } = true;
        public Materia Materia { get; set; } = null!;
        public Docente Docente { get; set; } = null!;
        public ICollection<SesionAsistencia> SesionesAsistencia { get; set; } = new List<SesionAsistencia>();
        public ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();
        public ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}