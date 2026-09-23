using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public class EquipoService : IEquipoService
{
    private readonly HttpClient _httpClient;
    public EquipoService(HttpClient httpClient) => _httpClient = httpClient;

    // GET api/Equipos
    public async Task<List<EquipoDto>> GetEquiposAsync()
    {
        var equipos = await _httpClient.GetFromJsonAsync<List<EquipoDto>>("Equipos");
        return equipos ?? new List<EquipoDto>();
    }

    // POST api/Equipos
    public async Task<bool> CrearAsync(EquipoDto equipo)
    {
        var respuesta = await _httpClient.PostAsJsonAsync("Equipos", equipo);
        return respuesta.IsSuccessStatusCode;
    }

    // DELETE api/Equipos/{id}
    public async Task<bool> EliminarAsync(int id)
    {
        var respuesta = await _httpClient.DeleteAsync($"Equipos/{id}");
        return respuesta.IsSuccessStatusCode;
    }
}
