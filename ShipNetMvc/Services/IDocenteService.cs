using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IDocenteService
{
    Task<List<DocenteDto>> GetDocentesAsync();
    Task<DocenteDto?> GetByIdAsync(int id);
    Task<(bool Exito, string? Error)> CrearDocenteAsync(CrearDocenteFormModel dto);
    Task<bool> ActualizarDocenteAsync(int id, DocenteDto dto);
    Task<bool> EliminarDocenteAsync(int id);
}
