using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IEstudianteService
{
    Task<List<EstudianteConAsistenciasDto>> GetEstudiantesAsync();
    Task<EstudianteConAsistenciasDto?> GetByIdAsync(int id);
    Task<MiMateriaDto?> GetMiMateriaAsync();
    Task<(bool Exito, string? Mensaje)> InscribirMateriaAsync(int grupoId);
    Task<(bool Exito, string? Mensaje)> RetirarMateriaAsync(int grupoId);
    Task<(bool Exito, string? Error)> ActualizarMacAsync(int estudianteId, ActualizarMacEstudianteDto dto);
    Task<bool> AutoVincularMacAsync(string mac);
}
