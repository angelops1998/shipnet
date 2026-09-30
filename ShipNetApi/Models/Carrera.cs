using System.Collections.Generic;

namespace ShipNetApi.Models
{
    public class Carrera
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Facultad { get; set; } = string.Empty;
        public int Duracion { get; set; } = 10;
        public bool Activa { get; set; } = true;
        public ICollection<Materia> Materias { get; set; } = new List<Materia>();
    }
}
