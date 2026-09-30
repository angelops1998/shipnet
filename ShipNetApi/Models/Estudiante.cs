using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShipNetApi.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
        public int? GrupoId { get; set; }
        public string? MacAddress { get; set; }
        public string? Carrera { get; set; }
        public int Semestre { get; set; } = 1;
        [JsonIgnore]
        public Grupo? Grupo { get; set; }
        [JsonIgnore]
        public ICollection<RegistroAsistencia> RegistrosAsistencia { get; set; } = new List<RegistroAsistencia>();
        [JsonIgnore]
        public ICollection<IntentoEvaluacion> IntentosEvaluacion { get; set; } = new List<IntentoEvaluacion>();
        [JsonIgnore]
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}