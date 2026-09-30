using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IEquipoService
{
    Task<List<EquipoDto>> GetEquiposAsync();
    Task<EquipoDto?> GetByIdAsync(int id);
    Task<bool> CrearAsync(EquipoDto equipo);
    Task<bool> ActualizarAsync(int id, EquipoDto equipo);
    Task<bool> EliminarAsync(int id);
}

