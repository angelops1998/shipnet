using System.Collections.Generic;

namespace shipnet.Models
{
    public class Docente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
        public ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
    }
}