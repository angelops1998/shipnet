using System.Collections.Generic;

namespace shipnet.Models
{
    public class Equipo
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public string? Ubicacion { get; set; }
        public ICollection<RegistroAsistencia> RegistrosAsistencia { get; set; } = new List<RegistroAsistencia>();
        public ICollection<IntentoEvaluacion> IntentosEvaluacion { get; set; } = new List<IntentoEvaluacion>();
    }
}