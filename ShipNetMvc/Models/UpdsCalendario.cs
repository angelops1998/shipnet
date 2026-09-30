namespace ShipNetMvc.Models;

public class ModuloAcademico
{
    public int Numero { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Mes { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Feriados { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public bool EsModuloActual { get; set; }
}

public static class UpdsCalendario
{
    public static List<ModuloAcademico> ObtenerModulos2026()
    {
        var hoy = DateTime.Now;
        var modulos = new List<ModuloAcademico>
        {
            new() { Numero = 1, Nombre = "Módulo 1", Mes = "Enero - Febrero", FechaInicio = new DateTime(2026, 1, 5), FechaFin = new DateTime(2026, 2, 2), Feriados = "01 Ene (Año Nuevo), 22 Ene (Estado Plurinacional)", Detalle = "Inicio de Gestión 2026" },
            new() { Numero = 2, Nombre = "Módulo 2", Mes = "Febrero - Marzo", FechaInicio = new DateTime(2026, 2, 9), FechaFin = new DateTime(2026, 3, 10), Feriados = "16-17 Feb (Carnaval)", Detalle = "Descanso pedagógico previo: 3-6 Feb" },
            new() { Numero = 3, Nombre = "Módulo 3", Mes = "Marzo - Abril", FechaInicio = new DateTime(2026, 3, 11), FechaFin = new DateTime(2026, 4, 8), Feriados = "03 Abr (Viernes Santo)", Detalle = "Evaluación por competencias" },
            new() { Numero = 4, Nombre = "Módulo 4", Mes = "Abril - Mayo", FechaInicio = new DateTime(2026, 4, 9), FechaFin = new DateTime(2026, 5, 7), Feriados = "01 May (Día del Trabajo)", Detalle = "Límite pago: 22 Abr" },
            new() { Numero = 5, Nombre = "Módulo 5", Mes = "Mayo - Junio", FechaInicio = new DateTime(2026, 5, 8), FechaFin = new DateTime(2026, 6, 5), Feriados = "04 Jun (Corpus Christi)", Detalle = "Límite pago: 21 May" },
            new() { Numero = 6, Nombre = "Módulo 6", Mes = "Junio - Julio", FechaInicio = new DateTime(2026, 6, 8), FechaFin = new DateTime(2026, 7, 3), Feriados = "21 Jun (Año Nuevo Andino)", Detalle = "Cierre primer semestre" },
            new() { Numero = 7, Nombre = "Módulo 7", Mes = "Julio", FechaInicio = new DateTime(2026, 7, 6), FechaFin = new DateTime(2026, 7, 31), Feriados = "16 Jul (La Paz)", Detalle = "Límite pago: 17 Jul" },
            new() { Numero = 8, Nombre = "Módulo 8", Mes = "Agosto - Septiembre", FechaInicio = new DateTime(2026, 8, 5), FechaFin = new DateTime(2026, 9, 2), Feriados = "06 Ago (Independencia de Bolivia)", Detalle = "Descanso pedagógico: 3-4 Ago" },
            new() { Numero = 9, Nombre = "Módulo 9", Mes = "Septiembre", FechaInicio = new DateTime(2026, 9, 3), FechaFin = new DateTime(2026, 9, 30), Feriados = "14 Sep (Cbba), 24 Sep (Sta Cruz)", Detalle = "Módulo en curso actual" },
            new() { Numero = 10, Nombre = "Módulo 10", Mes = "Octubre", FechaInicio = new DateTime(2026, 10, 1), FechaFin = new DateTime(2026, 10, 28), Feriados = "Ninguno", Detalle = "Siguiente módulo académico" },
            new() { Numero = 11, Nombre = "Módulo 11", Mes = "Octubre - Noviembre", FechaInicio = new DateTime(2026, 10, 29), FechaFin = new DateTime(2026, 11, 26), Feriados = "02 Nov (Todos Santos)", Detalle = "Límite pago: 12 Nov" },
            new() { Numero = 12, Nombre = "Módulo 12", Mes = "Noviembre - Diciembre", FechaInicio = new DateTime(2026, 11, 27), FechaFin = new DateTime(2026, 12, 23), Feriados = "25 Dic (Navidad)", Detalle = "Conclusión de Gestión 2026" },
        };

        foreach (var m in modulos)
        {
            if (hoy >= m.FechaInicio && hoy <= m.FechaFin.AddDays(1))
            {
                m.EsModuloActual = true;
            }
        }

        // Si la fecha actual estuviera fuera de rango, marcar por defecto el Módulo 9
        if (!modulos.Any(m => m.EsModuloActual))
        {
            modulos[8].EsModuloActual = true; // Módulo 9
        }

        return modulos;
    }

    public static ModuloAcademico ModuloActual => ObtenerModulos2026().First(m => m.EsModuloActual);
}
