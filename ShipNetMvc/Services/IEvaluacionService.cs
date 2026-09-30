using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IEvaluacionService
{
    // Docente
    Task<List<EvaluacionDto>> GetEvaluacionesDocenteAsync();
    Task<bool> CrearEvaluacionAsync(CrearEvaluacionViewModel vm);
    Task<EvaluacionDto?> GetDetalleEvaluacionAsync(int id);
    Task<bool> EliminarEvaluacionAsync(int id);

    // Admin
    Task<List<EvaluacionDto>> GetTodasEvaluacionesAsync();

    // Estudiante
    Task<List<EvaluacionDisponibleDto>> GetEvaluacionesDisponiblesAsync();
    Task<(bool Exito, IntentoEvaluacionDto? Intento, string? Error)> IniciarIntentoAsync(int evaluacionId, string? macAddress);
    Task<bool> FinalizarIntentoAsync(int intentoId, decimal puntaje);
}
