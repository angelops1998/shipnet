using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IEquipoService
{
    Task<List<EquipoDto>> GetEquiposAsync();
    Task<bool> CrearAsync(EquipoDto equipo);
    Task<bool> EliminarAsync(int id);
}
