using System.Net.Http.Json;
using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public class DocenteService : IDocenteService
{
    private readonly HttpClient _httpClient;
    public DocenteService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<List<DocenteDto>> GetDocentesAsync()
    {
        try
        {
            var docentes = await _httpClient.GetFromJsonAsync<List<DocenteDto>>("Docentes");
            return docentes ?? new List<DocenteDto>();
        }
        catch
        {
            return new List<DocenteDto>();
        }
    }

    public async Task<DocenteDto?> GetByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<DocenteDto>($"Docentes/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool Exito, string? Error)> CrearDocenteAsync(CrearDocenteFormModel dto)
    {
        var payload = new
        {
            dto.Nombre,
            dto.Apellido,
            dto.Email,
            dto.Password,
            dto.Telefono,
            dto.Especialidad
        };
        var respuesta = await _httpClient.PostAsJsonAsync("Docentes", payload);
        if (respuesta.IsSuccessStatusCode) return (true, null);
        var error = await respuesta.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(error) ? "No se pudo crear el docente." : error);
    }

    public async Task<bool> ActualizarDocenteAsync(int id, DocenteDto dto)
    {
        var respuesta = await _httpClient.PutAsJsonAsync($"Docentes/{id}", dto);
        return respuesta.IsSuccessStatusCode;
    }

    public async Task<bool> EliminarDocenteAsync(int id)
    {
        var respuesta = await _httpClient.DeleteAsync($"Docentes/{id}");
        return respuesta.IsSuccessStatusCode;
    }
}
