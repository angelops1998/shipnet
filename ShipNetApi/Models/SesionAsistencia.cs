using System;
using System.Collections.Generic;

namespace ShipNetApi.Models
{
    public class SesionAsistencia
    {
        public int Id { get; set; }
        public int GrupoId { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }
        public bool Abierta { get; set; } = true;
        public Grupo Grupo { get; set; } = null!;
        public ICollection<RegistroAsistencia> RegistrosAsistencia { get; set; } = new List<RegistroAsistencia>();
    }
}