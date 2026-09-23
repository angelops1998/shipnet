using System.Collections.Generic;

namespace ShipNetApi.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
    }
}