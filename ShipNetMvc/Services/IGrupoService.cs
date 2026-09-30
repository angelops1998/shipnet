using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IGrupoService
{
    Task<List<GrupoDto>> GetGruposAsync();
    Task<List<GrupoDto>> GetGruposDocenteAsync();
    Task<GrupoDto?> GetByIdAsync(int id);
    Task<bool> CrearAsync(GrupoDto grupo);
    Task<bool> ActualizarAsync(int id, GrupoDto grupo);
    Task<bool> ToggleActivoAsync(int id);
    Task<bool> EliminarAsync(int id);
}
