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

    // GET api/Grupos/docente
    public async Task<List<GrupoDto>> GetGruposDocenteAsync()
    {
        try
        {
            var grupos = await _httpClient.GetFromJsonAsync<List<GrupoDto>>("Grupos/docente");
            return grupos ?? new List<GrupoDto>();
        }
        catch
        {
            return new List<GrupoDto>();
        }
    }

    // GET api/Grupos/{id}
    public async Task<GrupoDto?> GetByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<GrupoDto>($"Grupos/{id}");
        }
        catch
        {
            return null;
        }
    }

    // POST api/Grupos
    public async Task<bool> CrearAsync(GrupoDto grupo)
    {
        var respuesta = await _httpClient.PostAsJsonAsync("Grupos", grupo);
        return respuesta.IsSuccessStatusCode;
    }

    // PUT api/Grupos/{id}
    public async Task<bool> ActualizarAsync(int id, GrupoDto grupo)
    {
        var respuesta = await _httpClient.PutAsJsonAsync($"Grupos/{id}", grupo);
        return respuesta.IsSuccessStatusCode;
    }

    // POST api/Grupos/toggle-activo/{id}
    public async Task<bool> ToggleActivoAsync(int id)
    {
        var respuesta = await _httpClient.PostAsync($"Grupos/toggle-activo/{id}", null);
        return respuesta.IsSuccessStatusCode;
    }

    // DELETE api/Grupos/{id}
    public async Task<bool> EliminarAsync(int id)
    {
        var respuesta = await _httpClient.DeleteAsync($"Grupos/{id}");
        return respuesta.IsSuccessStatusCode;
    }
}
