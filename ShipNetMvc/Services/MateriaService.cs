using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public class MateriaService : IMateriaService
{
    private readonly HttpClient _httpClient;
    public MateriaService(HttpClient httpClient) => _httpClient = httpClient;

    // GET api/Materias
    public async Task<List<MateriaDto>> GetMateriasAsync()
    {
        var materias = await _httpClient.GetFromJsonAsync<List<MateriaDto>>("Materias");
        return materias ?? new List<MateriaDto>();
    }

    // POST api/Materias
    public async Task<bool> CrearAsync(MateriaDto materia)
    {
        var respuesta = await _httpClient.PostAsJsonAsync("Materias", materia);
        return respuesta.IsSuccessStatusCode;
    }

    // DELETE api/Materias/{id}
    public async Task<bool> EliminarAsync(int id)
    {
        var respuesta = await _httpClient.DeleteAsync($"Materias/{id}");
        return respuesta.IsSuccessStatusCode;
    }
}
