using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShipNetApi.Models
{
    public class Docente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Especialidad { get; set; }
        [JsonIgnore]
        public ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
    }
}