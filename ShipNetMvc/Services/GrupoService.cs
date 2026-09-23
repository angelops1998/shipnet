using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public class GrupoService : IGrupoService
{
    private readonly HttpClient _httpClient;
    public GrupoService(HttpClient httpClient) => _httpClient = httpClient;

    // GET api/Grupos
    public async Task<List<GrupoDto>> GetGruposAsync()
    {
        var grupos = await _httpClient.GetFromJsonAsync<List<GrupoDto>>("Grupos");
        return grupos ?? new List<GrupoDto>();
    }

    // POST api/Grupos
    public async Task<bool> CrearAsync(GrupoDto grupo)
    {
        var respuesta = await _httpClient.PostAsJsonAsync("Grupos", grupo);
        return respuesta.IsSuccessStatusCode;
    }

    // DELETE api/Grupos/{id}
    public async Task<bool> EliminarAsync(int id)
    {
        var respuesta = await _httpClient.DeleteAsync($"Grupos/{id}");
        return respuesta.IsSuccessStatusCode;
    }
}
