using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface ICarreraService
{
    Task<List<CarreraDto>> GetCarrerasAsync();
    Task<CarreraDto?> GetByIdAsync(int id);
    Task<(bool Exito, string? Error)> CrearAsync(CarreraDto carrera);
    Task<bool> ActualizarAsync(int id, CarreraDto carrera);
    Task<bool> EliminarAsync(int id);
}
