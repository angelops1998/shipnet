using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IMateriaService
{
    Task<List<MateriaDto>> GetMateriasAsync();
    Task<MateriaDto?> GetByIdAsync(int id);
    Task<bool> CrearAsync(MateriaDto materia);
    Task<bool> ActualizarAsync(int id, MateriaDto materia);
    Task<bool> EliminarAsync(int id);
}

