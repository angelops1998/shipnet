using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShipNetApi.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public int? CarreraId { get; set; }
        public int Semestre { get; set; } = 1;
        [JsonIgnore]
        public Carrera? Carrera { get; set; }
        [JsonIgnore]
        public ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
    }
}