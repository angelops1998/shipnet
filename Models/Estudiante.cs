using System.Collections.Generic;

namespace shipnet.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
        public ICollection<RegistroAsistencia> RegistrosAsistencia { get; set; } = new List<RegistroAsistencia>();
        public ICollection<IntentoEvaluacion> IntentosEvaluacion { get; set; } = new List<IntentoEvaluacion>();
    }
}