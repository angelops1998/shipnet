using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IMateriaService
{
    Task<List<MateriaDto>> GetMateriasAsync();
    Task<bool> CrearAsync(MateriaDto materia);
    Task<bool> EliminarAsync(int id);
}
