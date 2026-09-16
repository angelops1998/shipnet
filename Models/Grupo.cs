using System.Collections.Generic;

namespace shipnet.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int MateriaId { get; set; }
        public int DocenteId { get; set; }
        public Materia Materia { get; set; } = null!;
        public Docente Docente { get; set; } = null!;
        public ICollection<SesionAsistencia> SesionesAsistencia { get; set; } = new List<SesionAsistencia>();
        public ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();
    }
}