using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IGrupoService
{
    Task<List<GrupoDto>> GetGruposAsync();
    Task<bool> CrearAsync(GrupoDto grupo);
    Task<bool> EliminarAsync(int id);
}
