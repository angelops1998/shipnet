using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public class DocenteService : IDocenteService
{
    private readonly HttpClient _httpClient;
    public DocenteService(HttpClient httpClient) => _httpClient = httpClient;

    // GET api/Docentes
    public async Task<List<DocenteDto>> GetDocentesAsync()
    {
        var docentes = await _httpClient.GetFromJsonAsync<List<DocenteDto>>("Docentes");
        return docentes ?? new List<DocenteDto>();
    }
}
