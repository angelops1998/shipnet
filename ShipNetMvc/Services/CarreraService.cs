using System.Net.Http.Json;
using ShipNetMvc.Models;

namespace ShipNetMvc.Services;

public class CarreraService : ICarreraService
{
    private readonly HttpClient _httpClient;
    public CarreraService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<List<CarreraDto>> GetCarrerasAsync()
    {
        try
        {
            var carreras = await _httpClient.GetFromJsonAsync<List<CarreraDto>>("Carreras");
            return carreras ?? new List<CarreraDto>();
        }
        catch
        {
            return new List<CarreraDto>();
        }
    }

    public async Task<CarreraDto?> GetByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CarreraDto>($"Carreras/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool Exito, string? Error)> CrearAsync(CarreraDto carrera)
    {
        var respuesta = await _httpClient.PostAsJsonAsync("Carreras", carrera);
        if (respuesta.IsSuccessStatusCode) return (true, null);
        var error = await respuesta.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(error) ? "No se pudo crear la carrera." : error);
    }

    public async Task<bool> ActualizarAsync(int id, CarreraDto carrera)
    {
        var respuesta = await _httpClient.PutAsJsonAsync($"Carreras/{id}", carrera);
        return respuesta.IsSuccessStatusCode;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var respuesta = await _httpClient.DeleteAsync($"Carreras/{id}");
        return respuesta.IsSuccessStatusCode;
    }
}
