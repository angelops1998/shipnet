using System;
using System.Collections.Generic;

namespace ShipNetApi.Models
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public int GrupoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DuracionMinutos { get; set; }
        public Grupo Grupo { get; set; } = null!;
        public ICollection<IntentoEvaluacion> IntentosEvaluacion { get; set; } = new List<IntentoEvaluacion>();
    }
}