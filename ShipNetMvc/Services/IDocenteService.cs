using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public interface IDocenteService
{
    Task<List<DocenteDto>> GetDocentesAsync();
}
